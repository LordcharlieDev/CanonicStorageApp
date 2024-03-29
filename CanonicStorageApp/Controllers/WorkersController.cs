﻿using Microsoft.AspNetCore.Mvc;
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
        static private List<string> maritalStatus;
        static private List<string> sex;

        public WorkersController(CNNCDbContext context)
        {
            _context = context;
            maritalStatus = new List<string> { "Married", "Single", "Divorced", "Widowed" };
            sex = new List<string> { "Male", "Female" };
        }

        private static List<Worker> workers = null;

        // GET: Workers
        [Authorize]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sort) ? "lname_desc" : "";
            ViewBag.FirstNameSortParm = sort == "fname" ? "fname_desc" : "fname";
            ViewBag.BirthdateSortParm = sort == "date" ? "date_desc" : "date";
            ViewBag.SalarySortParm = sort == "salary" ? "salary_desc" : "salary";
            ViewBag.PremiumSortParm = sort == "premium" ? "premium_desc" : "premium";
            ViewBag.ExpSortParm = sort == "experience" ? "experience_desc" : "experience";
            workers = await _context.Workers.Include(x => x.Position)
                                           .Include(x => x.Location)
                                           .Include(x => x.Projects)
                                           .ToListAsync();
            if (sort == "fname")
            {
                workers = workers.OrderBy(d => d.FirstName).ToList();
            }
            else if (sort == "fname_desc")
            {
                workers = workers.OrderByDescending(d => d.FirstName).ToList();
            }
            else if (sort == "lname_desc")
            {
                workers = workers.OrderByDescending(d => d.LastName).ToList();
            }
            else if (sort == "date")
            {
                workers = workers.OrderBy(d => d.Birthdate).ToList();
            }
            else if (sort == "date_desc")
            {
                workers = workers.OrderByDescending(d => d.Birthdate).ToList();
            }
            else if (sort == "salary")
            {
                workers = workers.OrderBy(d => d.Salary).ToList();
            }
            else if (sort == "salary_desc")
            {
                workers = workers.OrderByDescending(d => d.Salary).ToList();
            }
            else if (sort == "premium")
            {
                workers = workers.OrderBy(d => d.Premium).ToList();
            }
            else if (sort == "premium_desc")
            {
                workers = workers.OrderByDescending(d => d.Premium).ToList();
            }
            else if (sort == "experience")
            {
                workers = workers.OrderBy(d => d.DateOfEmployment).ToList();
            }
            else if (sort == "experience_desc")
            {
                workers = workers.OrderByDescending(d => d.DateOfEmployment).ToList();
            }
            else
            {
                workers = workers.OrderBy(d => d.LastName).ToList();
            }
            return View(workers);
        }
        [Authorize]
        public async Task<IActionResult> Print()
        {
            if (workers == null)
            {
                return View(await _context.Workers.Include(x => x.Position)
                                                  .Include(x => x.Location)
                                                  .Include(x => x.Projects)
                                                  .OrderBy(x => x.LastName)
                                                  .ToListAsync());
            }
            return View(workers);
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
                                               .FirstOrDefaultAsync(x => x.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.PositionList = new SelectList(await _context.Positions.Include(x => x.Department)
                                                                          .OrderBy(x => x.Name)
                                                                          .ToListAsync(), "Name", "Name"); //add
            ViewBag.LocationList = new SelectList(await _context.Locations.ToListAsync(), "Name", "Name"); //add
            ViewBag.MaritalStatus = new SelectList(maritalStatus);
            ViewBag.Sex = new SelectList(sex);
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Email,Phone,Birthdate,Salary,Premium,Position,Location,DateOfEmployment,Address,Army,Passport,MaritalStatus,Childrens,Sex")] Worker worker)
        {
            worker.Position = await _context.Positions.Include(x => x.Department).Where(x => x.Name == worker.Position.Name).FirstOrDefaultAsync(); //add
            ModelState.Remove("Position.Department");
            if (ModelState.IsValid)
            {
                worker.Location = await _context.Locations.Where(x => x.Name == worker.Location.Name).FirstOrDefaultAsync(); //add
                _context.Add(worker);
                await _context.SaveChangesAsync();
                TempData["toastMsg"] = $"New worker [{worker.FirstName} {worker.LastName}] created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PositionList = new SelectList(await _context.Positions.Include(x => x.Department)
                                                                          .OrderBy(x => x.Name)
                                                                          .ToListAsync(), "Name", "Name"); //add
            ViewBag.LocationList = new SelectList(await _context.Locations.ToListAsync(), "Name", "Name"); //add
            ViewBag.MaritalStatus = new SelectList(maritalStatus);
            ViewBag.Sex = new SelectList(sex);
            return View(worker);
        }

        // GET: Workers/Edit/5
        [Authorize]
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
            ViewBag.MaritalStatus = new SelectList(maritalStatus);
            ViewBag.Sex = new SelectList(sex);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Email,Phone,Birthdate,Salary,Premium,Position,Location,DateOfEmployment,Address,Army,Passport,MaritalStatus,Childrens,Sex")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Position.Department");
            worker.Position = await _context.Positions.Include(x => x.Department).Where(x => x.Name == worker.Position.Name).FirstOrDefaultAsync(); //add
            worker.Location = await _context.Locations.Where(x => x.Name == worker.Location.Name).FirstOrDefaultAsync(); //add
            if (ModelState.IsValid)
            {
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
                TempData["toastMsg"] = $"Changed info about the worker [{worker.FirstName} {worker.LastName}] was saved successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PositionList = new SelectList(await _context.Positions.ToListAsync(), "Name", "Name", worker.Position.Id); //add
            ViewBag.LocationList = new SelectList(await _context.Locations.ToListAsync(), "Name", "Name", worker.Location.Id); //add
            ViewBag.MaritalStatus = new SelectList(maritalStatus);
            ViewBag.Sex = new SelectList(sex);
            return View(worker);
        }

        // GET: Workers/Delete/5
        [Authorize]
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
        [Authorize]
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
            TempData["toastMsg"] = $"Worker [{worker?.FirstName} {worker?.LastName}] deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return (_context.Workers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
