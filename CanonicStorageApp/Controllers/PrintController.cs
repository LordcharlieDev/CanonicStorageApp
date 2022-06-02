using CNNCStorageDB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CanonicStorageApp.Controllers
{
    public class PrintController : Controller
    {
        CNNCDbContext _context = new CNNCDbContext();
        [Authorize]
        public IActionResult Departments()
        {
            return View();
        }
        [Authorize]
        public IActionResult Positions()
        {
            return View();
        }
        [Authorize]
        public IActionResult Workers()
        {
            return View();
        }
        [Authorize]
        public IActionResult Projects()
        {
            return View();
        }
        [Authorize]
        public IActionResult Clients()
        {
            return View();
        }
    }
}
