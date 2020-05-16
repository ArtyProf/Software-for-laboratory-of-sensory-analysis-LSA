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
    public class TastersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tasters.ToListAsync());
        }

        // GET: Tasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taster = await _context.Tasters
                .FirstOrDefaultAsync(m => m.TasterId == id);
            if (taster == null)
            {
                return NotFound();
            }

            return View(taster);
        }

        // GET: Tasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TasterId,TasterName,TasterSecondName")] Taster taster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taster);
        }

        // GET: Tasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taster = await _context.Tasters.FindAsync(id);
            if (taster == null)
            {
                return NotFound();
            }
            return View(taster);
        }

        // POST: Tasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TasterId,TasterName,TasterSecondName")] Taster taster)
        {
            if (id != taster.TasterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasterExists(taster.TasterId))
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
            return View(taster);
        }

        // GET: Tasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taster = await _context.Tasters
                .FirstOrDefaultAsync(m => m.TasterId == id);
            if (taster == null)
            {
                return NotFound();
            }

            return View(taster);
        }

        // POST: Tasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taster = await _context.Tasters.FindAsync(id);
            _context.Tasters.Remove(taster);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasterExists(int id)
        {
            return _context.Tasters.Any(e => e.TasterId == id);
        }
    }
}
