using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entities;
using LSA.Interfaces;
using LSA.Helpers;

namespace LSA.Controllers
{
    public class TastingHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastingHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TastingHistories
        public async Task<IActionResult> Index()
        {
            return View(await _context.TastingHistory.ToListAsync());
        }

        // GET: TastingHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TastingHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TastingHistory tastingHistory)
        {
            if (ModelState.IsValid)
            {
                if (tastingHistory == null)
                    throw new ArgumentNullException(nameof(tastingHistory));

                var tastingHistories = await _context.TastingHistory.ToListAsync();

                BlockChainHelper.VerifyBlockChain(tastingHistories);
                if (tastingHistories.Any(c => !c.IsValid))
                {
                    return RedirectToAction("ErrorPage", "Home", new { message = "Something going wrong. Blockchain was corrupted!" });
                }

                string previousBlockHash = null;
                if (tastingHistories.Any())
                {
                    var previousTastingHistory = tastingHistories.Last();
                    tastingHistory.TastingHistoryPreviousId = previousTastingHistory.TastingHistoryId;
                    previousBlockHash = previousTastingHistory.Hash;
                }

                int tastingId = await _context.Tastings.Where(c => c.IsFinished == false).Select(d => d.TastingId).FirstAsync();

                tastingHistory.TastingId = tastingId;

                int tasterId = 3;
                tastingHistory.TasterId = tasterId;

                List<int> productIds = await _context.ProductToTastings.Where(c => c.TastingId == tastingId).Select(d => d.ProductId).ToListAsync();

                List<int?> productIdsTastingHistory = await _context.TastingHistory.Where(c => c.TastingId == tastingId) //&& c.TasterId == tasterId (add later)
                                                                                   .Select(d => d.ProductId)
                                                                                   .ToListAsync();

                int productId = 0;

                if (productIds.Count > productIdsTastingHistory.Count)
                {
                    int dif = productIds.Count - productIdsTastingHistory.Count;

                    productId = productIds[productIds.Count - dif];
                }

                if (productIds.Count == productIdsTastingHistory.Count)
                {
                    return RedirectToAction("ErrorPage", "Home", new { message = "Something going wrong. Seems you tasted all products. Tasting is ended. Thank you!" });
                }

                tastingHistory.ProductId = productId;

                tastingHistory.TransactionDate = DateTime.Now;

                var blockText = BlockHelper.ConcatData(tastingHistory.ViewProse,
                        tastingHistory.ViewColour, tastingHistory.BouquetClean, tastingHistory.BouquetIntensity,
                        tastingHistory.BouquetQuality, tastingHistory.TasteColour, tastingHistory.TasteQuality,
                        tastingHistory.TasteIntensity, tastingHistory.TasteAftertaste, tastingHistory.TastePotencial,
                        tastingHistory.Garmony, tastingHistory.Penalty, tastingHistory.TransactionDate, previousBlockHash);
                tastingHistory.Hash = HashHelper.Hash(blockText);

                TastingHistory itemOnSave = _context.TastingHistory.Where
                         (x => x.TastingHistoryId == tastingHistory.TastingHistoryId).FirstOrDefault();

                itemOnSave = new TastingHistory()
                {
                    ViewProse = tastingHistory.ViewProse,
                    ViewColour = tastingHistory.ViewColour,
                    BouquetClean = tastingHistory.BouquetClean,
                    BouquetIntensity = tastingHistory.BouquetIntensity,
                    BouquetQuality = tastingHistory.BouquetQuality,
                    TasteColour = tastingHistory.TasteColour,
                    TasteQuality = tastingHistory.TasteQuality,
                    TasteIntensity = tastingHistory.TasteIntensity,
                    TasteAftertaste = tastingHistory.TasteAftertaste,
                    TastePotencial = tastingHistory.TastePotencial,
                    Garmony = tastingHistory.Garmony,
                    Penalty = tastingHistory.Penalty,
                    TransactionDate = tastingHistory.TransactionDate,
                    Hash = tastingHistory.Hash,
                    TasterId = tastingHistory.TasterId,
                    TastingId = tastingHistory.TastingId,
                    ProductId = tastingHistory.ProductId,
                    TastingHistoryPreviousId = tastingHistory.TastingHistoryPreviousId,
                };

                _context.TastingHistory.Add(itemOnSave);

                await _context.SaveChangesAsync();

                if (productIds.Count == productIdsTastingHistory.Count + 1)
                {
                    return RedirectToAction("ErrorPage", "Home", new { message = "Seems you tasted all products. Tasting is ended. Thank you!" });
                }

                return RedirectToAction(nameof(Index));
            }
            return View(tastingHistory);
        }

        private bool TastingHistoryExists(int id)
        {
            return _context.TastingHistory.Any(e => e.TastingHistoryId == id);
        }
    }
}
