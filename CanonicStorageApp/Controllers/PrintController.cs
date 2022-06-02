using CNNCStorageDB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CanonicStorageApp.Controllers
{
    public class PrintController : Controller
    {
        CNNCDbContext _context = new CNNCDbContext();
        [Authorize]
        public async Task<IActionResult> Departments()
        {
            return _context.Departments != null ?
                            View(await _context.Departments.ToListAsync()) :
                            Problem("Entity set 'CNNCDbContext.Departments'  is null.");
        }

        [Authorize]
        public async Task<IActionResult> Positions()
        {
            return _context.Positions != null ?
                        View(await _context.Positions.ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Positions'  is null.");
        }
        [Authorize]
        public async Task<IActionResult> Workers()
        {
            return _context.Workers != null ?
                        View(await _context.Workers.Include(x => x.Position).ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Workers'  is null.");
        }
        [Authorize]
        public async Task<IActionResult> Projects()
        {
            return _context.Projects != null ?
                        View(await _context.Projects.Include(x => x.Workers).ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Projects'  is null.");
        }
        [Authorize]
        public async Task<IActionResult> Clients()
        {
            return _context.Clients != null ?
                        View(await _context.Clients.ToListAsync()) :
                        Problem("Entity set 'CNNCDbContext.Clients'  is null.");
        }
    }
}
