using CanonicStorageApp.Models;
using CNNCStorageDB.Data;
using CNNCStorageDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CanonicStorageApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CNNCDbContext _context = new CNNCDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            User user = new User();
            user.IsAdmin = await _context.Users.Where(u => u.Login == User.Identity.Name).Select(x => x.IsAdmin).FirstOrDefaultAsync();
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}