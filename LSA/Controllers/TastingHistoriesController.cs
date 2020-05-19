using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entities;
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

        private bool TastingHistoryExists(int id)
        {
            return _context.TastingHistory.Any(e => e.TastingHistoryId == id);
        }
    }
}
