using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authorization;

namespace CanonicStorageApp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly CNNCDbContext _context;

        public ClientsController(CNNCDbContext context)
        {
            _context = context;
        }
        static private List<Client> clients = null;
        // GET: Clients
        [Authorize]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.Sort = sort;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "name_desc" : "";
            clients = await _context.Clients.ToListAsync();
            if (sort == "name_desc")
            {
                clients = clients.OrderByDescending(x => x.FullName).ToList();
            }
            else
            {
                clients = clients.OrderBy(x => x.FullName).ToList();
            }
            return View(clients);
        }

        [Authorize]
        public async Task<IActionResult> Print()
        {
            if (clients == null)
            {
                return View(await _context.Clients.OrderBy(x => x.FullName).ToListAsync());
            }
            return View(clients);
        }

        // GET: Clients/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.Include(x => x.Projects)
                                               .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,FullName,Address,Email,Phone")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                TempData["toastMsg"] = $"New client [{client.FullName}] created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Address,Email,Phone")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["toastMsg"] = $"Changed info about the client [{client.FullName}] was saved successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'CNNCDbContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            TempData["toastMsg"] = $"Client [{client?.FullName}] deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
