using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public ICollection<PurchaseOrderEvent> Events { get; set; }

        public class PurchaseOrderEvent
        {
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

        [ComplexType]
        public class OrderVendor
        {
            public String Name { get; set; }

            public String PointOfContact { get; set; }

            public String Phone { get; set; }

            public String PointOfContactRole { get; set; }

            public Address PhysicalAddress { get; set; } = new Address { };
        }

        [ComplexType]
        public class Address
        {
            public String Line1 { get; set; }

            public String City { get; set; }

            public String State { get; set; }

            public String ZipCode { get; set; }
        }
    }
}