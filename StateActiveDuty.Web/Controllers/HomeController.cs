using Microsoft.AspNetCore.Mvc;
using StateActiveDuty.Web.Models;

namespace StateActiveDuty.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database db;

        public HomeController(Database db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
