using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StateActiveDuty.Web.Models
{
    public class PurchaseOrder
    {
        // TODO Move Operation to a different scope for grouping?

        // TODO How to auto-generate the justification text?

        // TODO How to repeat using vendors

        // TODO Store uploaded receipts

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;

        public String Operation { get; set; } = "17-20 HURRICANE IRMA";

        public PurchaseCategory Category { get; set; } = PurchaseCategory.Meals;

        public MealCategory Meal { get; set; } = MealCategory.NA;

        public int UnitId { get; set; }

        public virtual Unit Unit { get; set; }

        public OrderVendor Vendor { get; set; } = new OrderVendor { };

        public OrderStatus Status =>
            Events
            .OrderByDescending(@event => @event.Timestamp)
            .Select(@event => @event.Status)
            .FirstOrDefault();

        public String Justification { get; set; } = "Hurricane Irma SAD lunch meal for 24 soldiers. Meal will be fed at Ocala for 1 day (9SEP17)";

        public OrderPriority Priority { get; set; } = OrderPriority.Routine;

        public virtual ICollection<OrderEvent> Events { get; set; }

        public class OrderEvent
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

            public OrderStatus Status { get; set; } = OrderStatus.Created;

            public String Username { get; set; }

            public String Comments { get; set; }
        }

        public enum PurchaseCategory : byte
        {
            Meals,
            Lodging,
            OfficeSupplies,
            Fuel,
            Laundry,
            RentalVehicles,
            RepairParts,
            EquipmentRental,
            Other
        }

        public enum MealCategory : byte
        {
            NA,
            Breakfast,
            Lunch,
            Dinner
        }

        public enum OrderPriority : byte
        {
            Routine = 0,
            Priority = 1,
            Emergency = 2
        }

        public enum OrderStatus : byte
        {
            Created = 0,
            Submitted_to_S4 = 1,
            Sent_to_SQM = 2,
            Requires_Update = 3,
            Approved = 10,
            Rejected = byte.MaxValue
            // Completed / Paid?
        }

        public class OrderVendor
        {
            public String Name { get; set; }

            public String FedID { get; set; }

            public String BusinessPhone { get; set; }

            public PointOfContact POC { get; set; } = new PointOfContact { };

            public Address PhysicalAddress { get; set; } = new Address { };

            public Address RemitToAddress { get; set; } = new Address { };
        }

        public class Address
        {
            public String Line1 { get; set; }

            public String City { get; set; }

            public String State { get; set; } = "FL";

            public String ZipCode { get; set; }
        }

        public virtual byte[] Generate_FLNG_49D()
        {
            using (var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("StateActiveDuty.Web.Models.FLNG_FORM 49D_SAD_Vendor_Request_Form.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                // Update the form fields as appropriate

                form.SetField($"operation", Operation);

                form.SetField($"Justification", Justification);

                form.SetField($"RequestingUnit", Unit.Name);
                form.SetField($"CommandTF", Unit.CommandOrTaskForce);
                form.SetField($"UnitPhone", Unit.Phone);
                form.SetField($"UnitPOCCell", Unit.POC.PhoneNumber);
                form.SetField($"UnitPOC", Unit.POC.Name);

                form.SetField($"BusPOC", Vendor.POC.Name);
                form.SetField($"Position", Vendor.POC.Role);

                form.SetField($"VendorName", Vendor.Name);
                form.SetField($"BusPhone", Vendor.BusinessPhone);
                form.SetField($"FedID", Vendor.FedID);

                form.SetField($"PhysAdd", Vendor.PhysicalAddress.Line1);
                form.SetField($"City", Vendor.PhysicalAddress.City);
                form.SetField($"State", Vendor.PhysicalAddress.State);
                form.SetField($"Zip", Vendor.PhysicalAddress.ZipCode);

                form.SetField($"RemitTo", Vendor.RemitToAddress.Line1);
                form.SetField($"City2", Vendor.RemitToAddress.City);
                form.SetField($"State2", Vendor.RemitToAddress.State);
                form.SetField($"Zip2", Vendor.RemitToAddress.ZipCode);

                form.SetField($"Other", "");

                //foreach(DictionaryEntry field in form.Fields)
                //{
                //    var states = form.GetAppearanceStates($"{field.Key}");
                //}

                Func<PurchaseCategory, String> To_String = (category) => Category == category ? "Yes" : "Off";

                form.SetField("Check Box9", To_String(PurchaseCategory.Meals)); // Meals
                form.SetField("Check Box10", To_String(PurchaseCategory.Lodging)); // Lodging
                form.SetField("Check Box11", To_String(PurchaseCategory.OfficeSupplies)); // Office Supplies
                form.SetField("Check Box12", To_String(PurchaseCategory.Fuel)); // Fuel
                form.SetField("Check Box13", To_String(PurchaseCategory.Laundry)); // Laundry
                form.SetField("Check Box14", To_String(PurchaseCategory.RentalVehicles)); // Rental Vehicles
                form.SetField("Check Box15", To_String(PurchaseCategory.RepairParts)); // Repair Parts
                form.SetField("Check Box16", To_String(PurchaseCategory.EquipmentRental)); // Equipment Rental
                form.SetField("Check Box17", To_String(PurchaseCategory.Other)); // Other (Explain)

                form.SetField("Check Box18", String.IsNullOrWhiteSpace(Justification) ? "Off" : "Yes"); // Purchase of Equipment (Justification)

                stamper.Close();

                return output.ToArray();
            }
        }
    }
}