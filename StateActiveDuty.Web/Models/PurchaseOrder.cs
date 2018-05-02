using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Today;

        public virtual Unit Unit { get; set; }

        public OrderVendor Vendor { get; set; } = new OrderVendor { };

        public OrderStatus Status => Events.OrderByDescending(@event => @event.Timestamp).Select(@event => @event.Status).FirstOrDefault();

        public OrderPriority Priority { get; set; } = OrderPriority.Routine;

        public ICollection<OrderEvent> Events { get; set; }

        public class OrderEvent
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            // Events

            // Who submitted it & when
            // Who approved it & when

            public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

            public OrderStatus Status { get; set; } = OrderStatus.Created;

            public String Username { get; set; }

            public String Comments { get; set; }
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

            // Completed / Paid?

            Rejected = byte.MaxValue
        }

        [Owned]
        public class OrderVendor
        {
            public String Name { get; set; }

            public String PointOfContact { get; set; }

            public String Phone { get; set; }

            public String PointOfContactRole { get; set; }

            public Address PhysicalAddress { get; set; } = new Address { };
        }

        [Owned]
        public class Address
        {
            public String Line1 { get; set; }

            public String City { get; set; }

            public String State { get; set; }

            public String ZipCode { get; set; }
        }

        public virtual byte[] Generate_FLNG_49D()
        {
            const String prefix = "form1[0]";

            using (var stream = typeof(Program).GetTypeInfo().Assembly.GetManifestResourceStream("StateActiveDuty.Web.Models.FLNG_FORM 49D_SAD_Vendor_Request_Form.pdf"))
            using (var output = new MemoryStream())
            {
                var reader = new PdfReader(stream);
                var stamper = new PdfStamper(reader, output);

                var form = stamper.AcroFields;

                // Update the form fields as appropriate

                //form.SetField($"{prefix}.Page1[0].Name[0]", model.Name);
                //form.SetField($"{prefix}.Page1[0].Name_Title_Counselor[0]", model.Counselor);
                //form.SetField($"{prefix}.Page1[0].Key_Points_Disscussion[0]", model.KeyPointsOfDiscussion);
                //form.SetField($"{prefix}.Page1[0].Date_Counseling[0]", model.Date.ToString("yyyy-MM-dd"));
                //form.SetField($"{prefix}.Page1[0].Rank_Grade[0]", model.Rank.DisplayName());
                //form.SetField($"{prefix}.Page2[0].Leader_Responsibilities[0]", model.LeadersResponsibilities);
                //form.SetField($"{prefix}.Page1[0].Purpose_Counseling[0]", model.Purpose);
                //form.SetField($"{prefix}.Page1[0].Organization[0]", model.Organization);
                //form.SetField($"{prefix}.Page2[0].Plan_Action[0]", model.PlanOfAction);
                //form.SetField($"{prefix}.Page2[0].Assessment[0]", model.Assessment);

                stamper.Close();

                return output.ToArray();
            }
        }
    }
}