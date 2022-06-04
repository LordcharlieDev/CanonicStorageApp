using CanonicStorageApp.Models;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CanonicStorageApp.Controllers
{
    public class AccountController : Controller
    {
        private CNNCDbContext _context;
        public AccountController(CNNCDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    if (user.IsAdmin)
                    {
                        await Authenticate(model.Username, "Administrator");
                    }
                    else
                    {
                        await Authenticate(model.Username);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect username or password");
            }
            return View(model);
        }
        /*[HttpGet]
        [Authorize]
        public async Task<IActionResult> Register()
        {
            if (await IsAdmin())
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Username);
                if (user == null)
                {
                    _context.Users.Add(new User { Login = model.Username, Password = model.Password, IsAdmin = model.IsAdmin });
                    await _context.SaveChangesAsync();

                    //await Authenticate(model.Username);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User already exists");
                }
            }
            return View(model);
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
        }*/

        private async Task Authenticate(string userName, string role = null)
        {
            // створюємо один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // створюємо об'єкт ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, role ?? ClaimsIdentity.DefaultRoleClaimType);
            // встановлення аутентифікаційних кукі
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
