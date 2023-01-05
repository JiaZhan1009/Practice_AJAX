using AJAX.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace AJAX.Controllers
{
    public class AjaxController : Controller
    {
        //相依性注入
        private readonly NorthwindContext _context;
        public AjaxController(NorthwindContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Greet(string Name)
        {
            Thread.Sleep(2000);
            return $"Hello, {Name}!";
        }

        // 這個方法也想叫 Greet, 但是上面有一個同名方法, 所以取名叫PostGreet, 在給 ActionName = Greet
        [HttpPost, ActionName("Greet")]
        public string PostGreet(string Name)
        {
            return $"Hello, {Name}!";
        }

        [HttpPost]
        public string CheckName(string CompanyName)
        {
            bool Exists = _context.Customers.Any(e => e.CompanyName == CompanyName);
            return Exists ? "true" : "false";
        }
    }
}
