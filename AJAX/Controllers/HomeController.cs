using AJAX.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AJAX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CallGreet() 
        {
            return View();
        }

        public IActionResult jQueryCallGreet()
        {
            return View();
        }

        public IActionResult CallCheckName()
        {
            return View();
        }

        public IActionResult jQueryCallCheckName()
        {
            return View();
        }

        public IActionResult CallEmployeeServices()
        {
            return View();
        }
        public IActionResult VUECRUD()
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