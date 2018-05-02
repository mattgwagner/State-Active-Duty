using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StateActiveDuty.Web.Models;
using System.Threading.Tasks;

namespace StateActiveDuty.Web.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly Database db;

        public PurchaseOrderController(Database db)
        {
            this.db = db;
        }

        // TODO Attach Documents/Receipts

        public async Task<IActionResult> Index()
        {
            // TODO Filter by Status, Unit or just use DataTables?

            return View(await db.PurchaseOrders.ToListAsync());
        }

        public async Task<IActionResult> Details(int id, string message = "")
        {
            ViewBag.Message = message;

            return View(await db.PurchaseOrders.FindAsync(id));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var model = new PurchaseOrder { };

            if (id > 0)
            {
                model = await db.PurchaseOrders.FindAsync(id);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PurchaseOrder model)
        {
            if (await db.PurchaseOrders.AnyAsync(order => order.Id == model.Id))
            {
                db.PurchaseOrders.Update(model);
            }
            else
            {
                await db.PurchaseOrders.AddAsync(model);
            }

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Update(dynamic model)
        {
            var order = await db.PurchaseOrders.FindAsync(model.Id);

            // Update the order events as appropriate

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id)
        {
            var order = await db.PurchaseOrders.FindAsync(id);

            order.Events.Add(new PurchaseOrder.PurchaseOrderEvent
            {
                Username = User.Identity.Name,
                Status = PurchaseOrder.OrderStatus.Submitted_to_S4
            });

            await db.SaveChangesAsync();

            // Send Email with PDF (?)

            return RedirectToAction(nameof(Details), new { id, message = "Your PO has been submitted successfully!" });
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFLNG49D(int id)
        {
            var order = await db.PurchaseOrders.FindAsync(id);

            var filename = $"FLNG_49D_{order.Unit}_{order.Vendor?.Name}_{order.Date:yyyyMMdd}.pdf";

            return File(order.Generate_FLNG_49D(), "application/pdf", filename);
        }
    }
}