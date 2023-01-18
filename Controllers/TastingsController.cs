using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entities;
using Microsoft.AspNetCore.Authorization;

namespace LSA.Controllers
{
    [Authorize(Roles = "CEO,Laboratory")]
    public class TastingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tastings/Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tastings.Where(a => a.IsFinished == false).ToListAsync());
        }

        // GET: Tastings/All
        public async Task<IActionResult> All ()
        {
            return View(await _context.Tastings.ToListAsync());
        }

        // GET: Tastings/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings
                .FirstOrDefaultAsync(m => m.TastingId == id);
            if (tasting is null)
            {
                return NotFound();
            }

            return View(tasting);
        }

        // GET: Tastings/Create
        public async Task<IActionResult> Create()
        {
            var tastings = await _context.Tastings.ToListAsync();

            if (tastings.Any(c => c.IsFinished == false))
            {
                return RedirectToAction("ErrorPage", "Home", new { message = "Something going wrong. You can not create a new tasting, if you do not finish active one!" });
            }

            return View();
        }

        // POST: Tastings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TastingId,TastingName")] Tasting tasting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tasting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasting);
        }

        // GET: Tastings/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings.FindAsync(id);
            if (tasting is null)
            {
                return NotFound();
            }
            return View(tasting);
        }

        // POST: Tastings/Edit/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TastingId,TastingName")] Tasting tasting)
        {
            if (id != tasting.TastingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TastingExists(tasting.TastingId))
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
            return View(tasting);
        }

        // GET: Tastings/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings
                .FirstOrDefaultAsync(m => m.TastingId == id);
            if (tasting is null)
            {
                return NotFound();
            }

            return View(tasting);
        }

        // POST: Tastings/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasting = await _context.Tastings.FindAsync(id);
            _context.Tastings.Remove(tasting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tastings/FinishTasting/{id}
        public async Task<IActionResult> Finish(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings
                .FirstOrDefaultAsync(m => m.TastingId == id);
            if (tasting is null)
            {
                return NotFound();
            }

            return View(tasting);
        }

        // POST: Tastings/FinishTasting/{id}
        [HttpPost, ActionName("Finish")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishTasting(int id)
        {
            var tasting = await _context.Tastings.FindAsync(id);
            tasting.IsFinished = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Tastings");
        }

        // GET: Tastings/TastingResult/{id}
        public async Task<IActionResult> TastingResult(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings.FindAsync(id);
            if (tasting is null)
            {
                return NotFound();
            }

            List<int> products = await _context.ProductToTastings.Where(a => a.TastingId == id).Select(b => b.ProductId).ToListAsync();
            int tasters = _context.TasterToTastings.Where(a => a.TastingId == id).Select(b => b.TasterId).Count();

            List<string> productNames = new();
            List<double> results = new();
            foreach(var product in products)
            {
                string productName = await _context.Products.Where(a => a.ProductId == product).Select(b => b.ProductName).FirstOrDefaultAsync();
                if (productName is not null)
                {
                    productNames.Add(productName);
                }

                var productInformation = await _context.TastingHistory.Where(a => a.TastingId == id && a.ProductId == product).ToListAsync();
                int viewColour = productInformation.Select(b => b.ViewColour).Sum();
                int viewProse = productInformation.Select(b => b.ViewProse).Sum();
                int bouquetClean = productInformation.Select(b => b.BouquetClean).Sum();
                int bouquetIntensity = productInformation.Select(b => b.BouquetIntensity).Sum();
                int bouquetQuality = productInformation.Select(b => b.BouquetQuality).Sum();
                int tasteColour = productInformation.Select(b => b.TasteColour).Sum();
                int tasteIntensity = productInformation.Select(b => b.TasteIntensity).Sum();
                int tasteAftertaste = productInformation.Select(b => b.TasteAftertaste).Sum();
                int tastePotencial = productInformation.Select(b => b.TastePotencial).Sum();
                int tasteQuality = productInformation.Select(b => b.TasteQuality).Sum();
                int garmony = productInformation.Select(b => b.Garmony).Sum();
                int penalty = productInformation.Select(b => b.Penalty).Sum();

                double result = (double)(viewColour + viewProse + bouquetClean + bouquetIntensity + bouquetQuality + tasteColour + tasteIntensity + tasteAftertaste + tastePotencial + tasteQuality + garmony - penalty) / tasters;
                result = Math.Round(result, 2);
                results.Add(result);
            }
            ViewBag.count = results.Count;
            ViewBag.results = results;
            ViewBag.products = productNames;

            return View();
        }

        private bool TastingExists(int id)
        {
            return _context.Tastings.Any(e => e.TastingId == id);
        }
    }
}
