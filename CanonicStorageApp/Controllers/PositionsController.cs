﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;

namespace CanonicStorageApp.Controllers
{
    public class PositionsController : Controller
    {
        private readonly CNNCDbContext _context;

        public PositionsController(CNNCDbContext context)
        {
            _context = context;
        }

        // GET: Positions
        public async Task<IActionResult> Index()
        {
            return _context.Positions != null ?
                        View(await _context.Positions.ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Positions'  is null.");
        }

        // GET: Positions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Positions == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.Include(x => x.Department)
                                                   .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // GET: Positions/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.message = new SelectList(await _context.Departments.ToListAsync(), "Name", "Name"); //add
            return View();
        }

        // POST: Positions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Department")] Position position)
        {
            if (ModelState.IsValid)
            {
                position.Department = await _context.Departments.Where(x => x.Name == position.Department.Name)
                                                                .FirstOrDefaultAsync(); //add
                _context.Add(position);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.message = new SelectList(await _context.Departments.ToListAsync(), "Name", "Name"); //add
            return View(position);
        }

        // GET: Positions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Positions == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            ViewBag.message = new SelectList(await _context.Departments.ToListAsync(), "Name", "Name", position.Department.Id); //add
            return View(position);
        }

        // POST: Positions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Department")] Position position)
        {
            if (id != position.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                position.Department = await _context.Departments.Where(x => x.Name == position.Department.Name)
                                                                .FirstOrDefaultAsync(); //add
                try
                {
                    _context.Update(position);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(position.Id))
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
            ViewBag.message = new SelectList(await _context.Departments.ToListAsync(), "Name", "Name", position.Department.Id);
            return View(position);
        }

        // GET: Positions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Positions == null)
            {
                return NotFound();
            }

            var position = await _context.Positions.Include(x => x.Department)
                                                   .FirstOrDefaultAsync(m => m.Id == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Positions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Positions == null)
            {
                return Problem("Entity set 'CNNCDbContext.Positions'  is null.");
            }
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PositionExists(int id)
        {
            return (_context.Positions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
