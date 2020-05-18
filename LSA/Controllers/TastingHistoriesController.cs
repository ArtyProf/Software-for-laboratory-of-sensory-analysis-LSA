using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entity;
using LSA.Services;
using LSA.Interfaces;

namespace LSA.Controllers
{
    public class TastingHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITastingHistory _tastingHistoryService;

        public TastingHistoriesController(ApplicationDbContext context,
                                            ITastingHistory tastingHistoryService)
        {
            _context = context;
            _tastingHistoryService = tastingHistoryService;
        }

        // GET: TastingHistories
        public async Task<IActionResult> Index()
        {
            return View(await _context.TastingHistory.ToListAsync());
        }

        // GET: TastingHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingHistory = await _context.TastingHistory
                .FirstOrDefaultAsync(m => m.TastingHistoryId == id);
            if (tastingHistory == null)
            {
                return NotFound();
            }

            return View(tastingHistory);
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
                await _tastingHistoryService.CreateTastingHistory(tastingHistory);
                return RedirectToAction(nameof(Index));
            }
            return View(tastingHistory);
        }

        // GET: TastingHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingHistory = await _context.TastingHistory.FindAsync(id);
            if (tastingHistory == null)
            {
                return NotFound();
            }
            return View(tastingHistory);
        }

        // POST: TastingHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TastingHistoryId,TastingHistoryPreviousId,ViewProse,ViewColour,BouquetClean,BouquetIntensity,BouquetQuality,TasteColour,TasteIntensity,TasteAftertaste,TastePotencial,TasteQuality,Garmony,Penalty,TransactionDate,Hash")] TastingHistory tastingHistory)
        {
            if (id != tastingHistory.TastingHistoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tastingHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TastingHistoryExists(tastingHistory.TastingHistoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tastingHistory);
        }

        // GET: TastingHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tastingHistory = await _context.TastingHistory
                .FirstOrDefaultAsync(m => m.TastingHistoryId == id);
            if (tastingHistory == null)
            {
                return NotFound();
            }

            return View(tastingHistory);
        }

        // POST: TastingHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tastingHistory = await _context.TastingHistory.FindAsync(id);
            _context.TastingHistory.Remove(tastingHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TastingHistoryExists(int id)
        {
            return _context.TastingHistory.Any(e => e.TastingHistoryId == id);
        }
    }
}
