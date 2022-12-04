using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using CanonicStorageApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CanonicStorageApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly CNNCDbContext _context;

        public ProjectsController(CNNCDbContext context)
        {
            _context = context;
        }
        private static List<Project> projects = null;

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewBag.BudgetSortParm = sort == "budget" ? "budget_desc" : "budget";
            ViewBag.StartDateSortParm = sort == "StartDate" ? "StartDate_desc" : "StartDate";
            ViewBag.EndDateSortParm = sort == "EndDate" ? "EndDate_desc" : "EndDate";
            ViewBag.FinalPriceSortParm = sort == "FinalCost" ? "FinalCost_desc" : "FinalCost";
            projects = await _context.Projects.Include(x => x.Workers).ToListAsync();
            if (sort == "name_desc")
            {
                projects = projects.OrderByDescending(d => d.Name).ToList();
            }
            else if (sort == "budget")
            {
                projects = projects.OrderBy(d => d.Budget).ToList();
            }
            else if (sort == "budget_desc")
            {
                projects = projects.OrderByDescending(d => d.Budget).ToList();
            }
            else if (sort == "StartDate")
            {
                projects = projects.OrderBy(d => d.StartDate).ToList();
            }
            else if (sort == "StartDate_desc")
            {
                projects = projects.OrderByDescending(d => d.StartDate).ToList();
            }
            else if (sort == "EndDate")
            {
                projects = projects.OrderBy(d => d.EndDate).ToList();
            }
            else if (sort == "EndDate_desc")
            {
                projects = projects.OrderByDescending(d => d.EndDate).ToList();
            }
            else if (sort == "FinalCost")
            {
                projects = projects.OrderBy(d => d.FinalCost).ToList();
            }
            else if (sort == "FinalCost_desc")
            {
                projects = projects.OrderByDescending(d => d.FinalCost).ToList();
            }
            else
            {
                projects = projects.OrderBy(d => d.Name).ToList();
            }
            return View(projects);
        }

        [Authorize]
        public async Task<IActionResult> Print()
        {
            if (projects == null)
            {
                return View(await _context.Projects.Include(x => x.Workers).OrderBy(x => x.Name).ToListAsync());
            }
            return View(projects);
        }

        // GET: Projects/Details/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
                TempData["toastMsg"] = $"New project [{project.Name}] created successfully!";
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int id, ProjectViewModel projectViewModel)
        {
            if (id != projectViewModel.Project.Id)
            {
                return NotFound();
            }

            if (projectViewModel.SelectedWorkers.Length > 0)
            {
                projectViewModel.Project.Client = await _context.Clients.Where(x => x.Id == projectViewModel.Project.Client.Id).FirstOrDefaultAsync();
                Project project = projectViewModel.Project;
                Project currentProject = await _context.Projects.Include(x => x.Workers).Include(x => x.Client).Where(x => x.Id == id).FirstOrDefaultAsync();
                currentProject.Name = project.Name;
                currentProject.Budget = project.Budget;
                currentProject.StartDate = project.StartDate;
                currentProject.EndDate = project.EndDate;
                currentProject.FinalCost = project.FinalCost;
                currentProject.Client = project.Client;
                foreach (var item in currentProject.Workers)
                {
                    if(!projectViewModel.SelectedWorkers.Contains(item.Id))
                    {
                        currentProject.Workers.Remove(item);
                    }
                }
                foreach (var item in projectViewModel.SelectedWorkers)
                {
                    Worker worker = await _context.Workers.Where(x => x.Id == item)
                                                          .Include(x => x.Position)
                                                          .Include(x => x.Location)
                                                          .FirstOrDefaultAsync();
                    if (!currentProject.Workers.Contains(worker))
                    {
                        currentProject.Workers.Add(worker);
                    }
                }
                try
                {
                    _context.Update(currentProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(currentProject.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["toastMsg"] = $"Changed info about the project [{project.Name}] was saved successfully!";
                return RedirectToAction(nameof(Index));
            }
            var workers = _context.Workers.ToList();
            var cl = new SelectList(_context.Clients.ToList(), "Id", "FullName", projectViewModel.SelectedWorkers);
            ViewBag.ClientsList = cl;
            projectViewModel.Workers = workers;
            Array.Clear(projectViewModel.SelectedWorkers);
            return View(projectViewModel);
        }

        // GET: Projects/Delete/5
        [Authorize]
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
            var cl = new SelectList(_context.Clients.ToList(), "Id", "FullName");
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
            TempData["toastMsg"] = $"Project [{project?.Name}] deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
