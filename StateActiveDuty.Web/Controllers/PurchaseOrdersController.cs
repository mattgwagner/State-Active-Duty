﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StateActiveDuty.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StateActiveDuty.Web.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly Database db;

        public PurchaseOrdersController(Database db)
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

            return View(await Get(id));
        }

        public async Task<IActionResult> New()
        {
            return await Edit();
        }

        public async Task<IActionResult> Edit(int? id = null)
        {
            ViewBag.Units =
                await db
                .Units
                .OrderBy(unit => unit.Name)
                .Select(unit => new SelectListItem
                {
                    Text = unit.Name,
                    Value = $"{unit.Id}"
                })
                .ToListAsync();

            var model = new PurchaseOrder { };

            if (id > 0)
            {
                model = await Get(id.Value);
            }

            return View("Edit", model);
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
            var order = await Get(model.Id);

            // Update the order events as appropriate

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id)
        {
            var order = await Get(id);

            order.Events.Add(new PurchaseOrder.OrderEvent
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
            // var order = await db.PurchaseOrders.FindAsync(id);

            var order = new PurchaseOrder
            {
                Unit = new Unit
                {
                    Name = "C BTRY",
                    POC = new PointOfContact
                    {
                        Name = "Me",
                        PhoneNumber = "My #",
                        Role = "Boss"
                    },
                    CommandOrTaskForce = "53 IBCT",
                    Phone = "123456789"
                },
                Vendor = new PurchaseOrder.OrderVendor
                {
                    Name = "Vendor Name",
                    BusinessPhone = "987654321",
                    FedID = "999999999",
                    PhysicalAddress = new PurchaseOrder.Address
                    {
                        City = "Lakeland",
                        Line1 = "321 Fake Street",
                        State = "TX",
                        ZipCode = "54321"
                    },
                    RemitToAddress = new PurchaseOrder.Address
                    {
                        City = "Tampa",
                        Line1 = "123 Happy Street",
                        State = "FL",
                        ZipCode = "12345"
                    },
                    POC = new PointOfContact
                    {
                        Name = "Mr Mgr",
                        Role = "Grcery Manager"
                    }
                }
            };

            var filename = $"FLNG_49D_{order.Unit?.Name}_{order.Vendor?.Name}_{order.Date:yyyyMMdd}.pdf";

            return File(order.Generate_FLNG_49D(), "application/pdf", filename);
        }

        private async Task<PurchaseOrder> Get(int id)
        {
            return
                await db
                .PurchaseOrders
                .Include(order => order.Unit)
                .ThenInclude(unit => unit.POC)
                .Include(order => order.Events)
                .Where(order => order.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}