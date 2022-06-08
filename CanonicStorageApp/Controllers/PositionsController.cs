using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authorization;

namespace CanonicStorageApp.Controllers
{
    public class PositionsController : Controller
    {
        private readonly CNNCDbContext _context;

        public PositionsController(CNNCDbContext context)
        {
            _context = context;
        }
        static private List<Position> positions = null;

        // GET: Positions
        //public async Task<IActionResult> Index(List<Position> p = null)
        //{
        //    return _context.Positions != null ?
        //                View(await _context.Positions.ToListAsync()) :
        //                Problem("Entity set 'CNNCDbContext.Positions'  is null.");
        //}
        [Authorize]
        public async Task<ActionResult> Index(string sort)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            ViewBag.DateSortParm = sort == "Date" ? "date_desc" : "Date";
            var positions = await _context.Positions.ToListAsync();
            if (sort == "name_desc")
            {
                positions = positions.OrderByDescending(d => d.Name).ToList();
            }
            else
            {
                positions = positions.OrderBy(d => d.Name).ToList();
            }
            return View(positions);
        }

        [Authorize]
        public async Task<IActionResult> Print()
        {
            if (positions == null)
            {
                return View(await _context.Positions.OrderBy(x => x.Name).ToListAsync());
            }
            return View(positions);
        }

        // GET: Positions/Details/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
