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
            // Nothing to do here yet

            return RedirectToAction(nameof(Index), "PurchaseOrders");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}