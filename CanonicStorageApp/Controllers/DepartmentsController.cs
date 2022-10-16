using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authorization;

namespace CanonicStorageApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly CNNCDbContext _context;

        public DepartmentsController(CNNCDbContext context)
        {
            _context = context;
        }

        static private List<Department> departments = null;

        // GET: Departments
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Departments != null ? 
        //                  View(await _context.Departments.ToListAsync()) :
        //                  Problem("Entity set 'CNNCDbContext.Departments'  is null.");
        //}

        [Authorize]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            departments = await _context.Departments.ToListAsync();
            if(sort == "name_desc")
            {
                departments = departments.OrderByDescending(d => d.Name).ToList();
            }
            else
            {
                departments = departments.OrderBy(d => d.Name).ToList();
            }
            return View(departments);
        }

        [Authorize]
        public async Task<IActionResult> Print()
        {
            if(departments == null)
            {
                return View(await _context.Departments.OrderBy(x => x.Name).ToListAsync());
            }
            return View(departments);
        }


        // GET: Departments/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.Include(x => x.Positions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                TempData["toastMsg"] = $"New department [{department.Name}] created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["toastMsg"] = $"Department [{department.Name}] saved changes info successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departments == null)
            {
                return Problem("Entity set 'CNNCDbContext.Departments'  is null.");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            TempData["toastMsg"] = $"Department [{department?.Name}] deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
          return (_context.Departments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
