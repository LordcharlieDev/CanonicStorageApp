﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authorization;

namespace CanonicStorageApp.Controllers
{
    public class WorkersController : Controller
    {
        private readonly CNNCDbContext _context;

        public WorkersController(CNNCDbContext context)
        {
            _context = context;
        }

        // GET: Workers
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Workers != null ?
                        View(await _context.Workers.ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Workers'  is null.");
        }

        // GET: Workers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.Include(x => x.Position)
                                               .Include(x => x.Location)
                                               .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.PositionList = new SelectList(await _context.Positions.Include(x => x.Department)
                                                                          .ToListAsync(), "Name", "Name"); //add
            ViewBag.LocationList = new SelectList(await _context.Locations.ToListAsync(), "Name", "Name"); //add
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Email,Phone,Birthdate,Salary,Premium,Position,Location")] Worker worker)
        {
            worker.Position = await _context.Positions.Include(x => x.Department).Where(x => x.Name == worker.Position.Name).FirstOrDefaultAsync(); //add
            ModelState.Remove("Position.Department");
            if (ModelState.IsValid)
            {
                worker.Location = await _context.Locations.Where(x => x.Name == worker.Location.Name).FirstOrDefaultAsync(); //add
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PositionList = new SelectList(await _context.Positions.ToListAsync(), "Name", "Name"); //add
            ViewBag.LocationList = new SelectList(await _context.Locations.ToListAsync(), "Name", "Name"); //add
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewBag.PositionList = new SelectList(await _context.Positions.ToListAsync(), "Name", "Name", worker.Position.Id); //add
            ViewBag.LocationList = new SelectList(await _context.Locations.ToListAsync(), "Name", "Name", worker.Location.Id); //add
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Email,Phone,Birthdate,Salary,Premium,Position,Location")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Position.Department");
            worker.Position = await _context.Positions.Include(x => x.Department).Where(x => x.Name == worker.Position.Name).FirstOrDefaultAsync(); //add
            if (ModelState.IsValid)
            {
                worker.Location = await _context.Locations.Where(x => x.Name == worker.Location.Name).FirstOrDefaultAsync(); //add
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Id))
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
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Workers == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.Include(x => x.Position)
                                               .Include(x => x.Location)
                                               .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Workers == null)
            {
                return Problem("Entity set 'CNNCDbContext.Workers'  is null.");
            }
            var worker = await _context.Workers.FindAsync(id);
            if (worker != null)
            {
                _context.Workers.Remove(worker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return (_context.Workers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
