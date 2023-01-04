using AJAX.Models;
using Microsoft.AspNetCore.Mvc;

namespace AJAX.Controllers
{
    public class AjaxController : Controller
    {
        private readonly NorthwindContext _context;
        public AjaxController(NorthwindContext context)
        {
            _context = context;
        }
        [HttpGet()]
        public string Greet(string Name)
        {
            Thread.Sleep(2000);
            return $"Hello, {Name}!";
        }
        [HttpPost(), ActionName("Greet")]
        public string PostGreet(string Name)
        {
            Thread.Sleep(2000);
            return $"Hello, {Name}!";
        }
        [HttpPost]
        public string CheckName(string CompanyName)
        {
            bool Exists = _context.Customers.Any(c => c.CompanyName == CompanyName);
            return Exists ? "true" : "false";
        }

        //public JsonResult Test()
        //{
        //    return Json(變數)
        //}
        public IActionResult Index()
        {
            return View();
        }
    }
}
