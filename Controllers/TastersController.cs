﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LSA.Data;
using LSA.Entities;
using LSA.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LSA.Controllers
{
    [Authorize(Roles = "CEO,Laboratory")]
    public class TastersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessService _userAccessService;

        public TastersController(ApplicationDbContext context,
                                 IUserAccessService userAccessService)
        {
            _context = context;
            _userAccessService = userAccessService;
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
        public async Task<IActionResult> Create([Bind("TasterId,TasterName,TasterEmail,TasterSecondName")] Taster taster, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAccessService.RegisterUserFromForm(taster.TasterEmail, password);

                if (!result)
                {
                    return RedirectToAction("ErrorPage", "Home", new { message = "Something going wrong. Please check that user can exist already." });
                }

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
        public async Task<IActionResult> Edit(int id, [Bind("TasterId,TasterName,TasterEmail,TasterSecondName")] Taster taster)
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
                    if (!await _context.Tasters.AnyAsync(e => e.TasterId == taster.TasterId))
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
            if (id is null)
            {
                return NotFound();
            }

            var taster = await _context.Tasters
                .FirstOrDefaultAsync(m => m.TasterId == id);
            if (taster is null)
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
    }
}
