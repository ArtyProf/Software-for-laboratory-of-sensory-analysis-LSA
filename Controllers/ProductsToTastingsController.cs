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
    public class ProductsToTastingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsToTastingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductsToTastings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductToTastings.Include(p => p.Product).Include(p => p.Tasting);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductsToTastings/Details/5
        public async Task<IActionResult> Details(int? id, int? id2)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productToTasting = await _context.ProductToTastings
                .Include(p => p.Product)
                .Include(p => p.Tasting)
                .FirstOrDefaultAsync(m => m.ProductId == id && m.TastingId == id2);
            if (productToTasting == null)
            {
                return NotFound();
            }

            return View(productToTasting);
        }

        // GET: ProductsToTastings/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products.Where(c => !_context.ProductToTastings
                                                                    .Select(b => b.ProductId)
                                                                    .Contains(c.ProductId)), "ProductId", "ProductName");
            return View();
        }

        // POST: ProductsToTastings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,TastingId")] ProductToTasting productToTasting)
        {
            if (ModelState.IsValid)
            {
                productToTasting.TastingId = _context.Tastings.Where(a => a.IsFinished == false).Select(b => b.TastingId).First();
                _context.Add(productToTasting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", productToTasting.ProductId);
            ViewData["TastingId"] = new SelectList(_context.Tastings, "TastingId", "TastingId", productToTasting.TastingId);
            return View(productToTasting);
        }

        // GET: ProductsToTastings/Delete/5
        public async Task<IActionResult> Delete(int? id, int? id2)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productToTasting = await _context.ProductToTastings
                .Include(p => p.Product)
                .Include(p => p.Tasting)
                .FirstOrDefaultAsync(m => m.ProductId == id && m.TastingId == id2);
            if (productToTasting == null)
            {
                return NotFound();
            }

            return View(productToTasting);
        }

        // POST: ProductsToTastings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, int? id2)
        {
            var productToTasting = await _context.ProductToTastings.FirstOrDefaultAsync(m => m.ProductId == id && m.TastingId == id2);
            _context.ProductToTastings.Remove(productToTasting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductToTastingExists(int id)
        {
            return _context.ProductToTastings.Any(e => e.ProductId == id);
        }
    }
}
