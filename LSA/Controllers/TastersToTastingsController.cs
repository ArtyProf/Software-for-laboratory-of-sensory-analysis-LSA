using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entity;

namespace LSA.Controllers
{
    public class TastersToTastingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastersToTastingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TastersToTastings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TasterToTastings.Include(t => t.Taster).Include(t => t.Tasting);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TastersToTastings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasterToTasting = await _context.TasterToTastings
                .Include(t => t.Taster)
                .Include(t => t.Tasting)
                .FirstOrDefaultAsync(m => m.TasterId == id);
            if (tasterToTasting == null)
            {
                return NotFound();
            }

            return View(tasterToTasting);
        }

        // GET: TastersToTastings/Create
        public IActionResult Create()
        {
            ViewData["TasterId"] = new SelectList(_context.Tasters, "TasterId", "TasterName");
            ViewData["TastingId"] = new SelectList(_context.Tastings, "TastingId", "TastingId");
            return View();
        }

        // POST: TastersToTastings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TasterId,TastingId")] TasterToTasting tasterToTasting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tasterToTasting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TasterId"] = new SelectList(_context.Tasters, "TasterId", "TasterName", tasterToTasting.TasterId);
            ViewData["TastingId"] = new SelectList(_context.Tastings, "TastingId", "TastingId", tasterToTasting.TastingId);
            return View(tasterToTasting);
        }

        // GET: TastersToTastings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasterToTasting = await _context.TasterToTastings.FindAsync(id);
            if (tasterToTasting == null)
            {
                return NotFound();
            }
            ViewData["TasterId"] = new SelectList(_context.Tasters, "TasterId", "TasterName", tasterToTasting.TasterId);
            ViewData["TastingId"] = new SelectList(_context.Tastings, "TastingId", "TastingId", tasterToTasting.TastingId);
            return View(tasterToTasting);
        }

        // POST: TastersToTastings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TasterId,TastingId")] TasterToTasting tasterToTasting)
        {
            if (id != tasterToTasting.TasterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasterToTasting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasterToTastingExists(tasterToTasting.TasterId))
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
            ViewData["TasterId"] = new SelectList(_context.Tasters, "TasterId", "TasterName", tasterToTasting.TasterId);
            ViewData["TastingId"] = new SelectList(_context.Tastings, "TastingId", "TastingId", tasterToTasting.TastingId);
            return View(tasterToTasting);
        }

        // GET: TastersToTastings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tasterToTasting = await _context.TasterToTastings
                .Include(t => t.Taster)
                .Include(t => t.Tasting)
                .FirstOrDefaultAsync(m => m.TasterId == id);
            if (tasterToTasting == null)
            {
                return NotFound();
            }

            return View(tasterToTasting);
        }

        // POST: TastersToTastings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tasterToTasting = await _context.TasterToTastings.FindAsync(id);
            _context.TasterToTastings.Remove(tasterToTasting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasterToTastingExists(int id)
        {
            return _context.TasterToTastings.Any(e => e.TasterId == id);
        }
    }
}
