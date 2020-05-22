using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entities;

namespace LSA.Controllers
{
    public class TastingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tastings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tastings.ToListAsync());
        }

        // GET: Tastings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings
                .FirstOrDefaultAsync(m => m.TastingId == id);
            if (tasting == null)
            {
                return NotFound();
            }

            return View(tasting);
        }

        // GET: Tastings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tastings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TastingId,TastingName,IsFinished")] Tasting tasting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tasting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tasting);
        }

        // GET: Tastings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings.FindAsync(id);
            if (tasting == null)
            {
                return NotFound();
            }
            return View(tasting);
        }

        // POST: Tastings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TastingId,TastingName,IsFinished")] Tasting tasting)
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

        // GET: Tastings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasting = await _context.Tastings
                .FirstOrDefaultAsync(m => m.TastingId == id);
            if (tasting == null)
            {
                return NotFound();
            }

            return View(tasting);
        }

        // POST: Tastings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasting = await _context.Tastings.FindAsync(id);
            _context.Tastings.Remove(tasting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TastingExists(int id)
        {
            return _context.Tastings.Any(e => e.TastingId == id);
        }
    }
}
