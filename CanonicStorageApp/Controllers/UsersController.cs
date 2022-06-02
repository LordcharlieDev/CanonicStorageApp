using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authorization;
using CanonicStorageApp.Models;

namespace CanonicStorageApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly CNNCDbContext _context;

        public UsersController(CNNCDbContext context)
        {
            _context = context;
        }

        [Authorize]
        private async Task<bool> IsAdmin()
        {
            var user = await _context.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefaultAsync();
            if (user.IsAdmin)
            {
                return true;
            }
            return false;
        }

        // GET: Users
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (await IsAdmin())
            {
                return _context.Users != null ?
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'CNNCDbContext.Users'  is null.");
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: Users/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            if (await IsAdmin())
            {
                return View(new RegisterViewModel());
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(RegisterViewModel registerViewModel)
        {
            if (await IsAdmin())
            {
                if (ModelState.IsValid)
                {
                    var user = await _context.Users.Where(x => x.Login == registerViewModel.Username).FirstOrDefaultAsync();
                    if (user == null)
                    {
                        User newUser = new User { Login = registerViewModel.Username, Password = registerViewModel.Password, IsAdmin = registerViewModel.IsAdmin };
                        _context.Add(newUser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "User already exists");
                    }

                }
                return View(registerViewModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (await IsAdmin())
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,IsAdmin")] User user)
        {
            if (await IsAdmin())
            {
                if (id != user.Id)
                {
                    return NotFound();
                }
                var u = await _context.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
                if (User.Identity.Name == u.Login)
                {
                    ModelState.AddModelError("", "Not access");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
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
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (await IsAdmin())
            {
                if (id == null || _context.Users == null)
                {
                    return NotFound();
                }
                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await IsAdmin())
            {
                var u = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (User.Identity.Name == u.Login)
                {
                    return Problem("Unable to delete a user who is authorized in the application", null, null, "Not access");
                }
                if (_context.Users == null)
                {
                    return Problem("Entity set 'CNNCDbContext.Users'  is null.");
                }
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
