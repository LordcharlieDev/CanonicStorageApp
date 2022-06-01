using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using CanonicStorageApp.Models;

namespace CanonicStorageApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly CNNCDbContext _context;

        public ProjectsController(CNNCDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return _context.Projects != null ?
                        View(await _context.Projects.ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Projects'  is null.");
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(x => x.Workers).Include(x => x.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var project = new Project();
            var workers = _context.Workers.ToList();
            var cl = new SelectList(_context.Clients.ToList(), "Id", "FullName");
            ViewBag.ClientsList = cl;
            return View(new ProjectViewModel(project, workers));
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewModel projectViewModel)
        {
            if (projectViewModel.SelectedWorkers.Length > 0)
            {
                Project project = projectViewModel.Project;
                project.Client = await _context.Clients.Where(x => x.Id == projectViewModel.Project.Client.Id).FirstOrDefaultAsync();
                foreach (var item in projectViewModel.SelectedWorkers)
                {
                    Worker worker = await _context.Workers.Where(x => x.Id == item).FirstOrDefaultAsync();
                    project.Workers.Add(worker);
                }
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            projectViewModel.Project.Client = await _context.Clients.Where(x => x.Id == projectViewModel.Project.Client.Id).FirstOrDefaultAsync();
            var workers = _context.Workers.ToList();
            var cl = new SelectList(_context.Clients.ToList(), "Id", "FullName");
            ViewBag.ClientsList = cl;
            projectViewModel.Workers = workers;
            Array.Clear(projectViewModel.SelectedWorkers);
            return View(projectViewModel);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Where(x => x.Id == id)
                                                 .Include(x => x.Workers)
                                                 .Include(x => x.Client)
                                                 .FirstOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }

            var workers = _context.Workers.ToList();
            var cl = new SelectList(_context.Clients.ToList(), "Id", "FullName");
            ViewBag.ClientsList = cl;
            return View(new ProjectViewModel(project, workers));
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectViewModel projectViewModel)
        {
            if (id != projectViewModel.Project.Id)
            {
                return NotFound();
            }

            if (projectViewModel.SelectedWorkers.Length > 0)
            {
                Project project = projectViewModel.Project;
                project.Client = await _context.Clients.Where(x => x.Id == projectViewModel.Project.Client.Id).FirstOrDefaultAsync();
                foreach (var item in projectViewModel.SelectedWorkers)
                {
                    Worker worker = await _context.Workers.Where(x => x.Id == item)
                                                          .Include(x => x.Position)
                                                          .Include(x => x.Location)
                                                          .FirstOrDefaultAsync();
                    project.Workers.Add(worker);
                }
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            projectViewModel.Project.Client = await _context.Clients.Where(x => x.Id == projectViewModel.Project.Client.Id).FirstOrDefaultAsync();
            var workers = _context.Workers.ToList();
            var cl = new SelectList(_context.Clients.ToList(), "Id", "FullName");
            ViewBag.ClientsList = cl;
            projectViewModel.Workers = workers;
            Array.Clear(projectViewModel.SelectedWorkers);
            return View(projectViewModel);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(x => x.Workers).Include(x => x.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'CNNCDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
