using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateActiveDuty.Web.Models
{
    public class PurchaseOrder
    {
        public DateTime Date { get; set; } = DateTime.Today;

        public virtual Unit Unit { get; set; }

        public virtual Vendor Vendor { get; set; }

        public Status OrderStatus { get; set; } = Status.Created;

        // Who submitted it & when

        // Who approved it & when

        public Priority OrderPriority { get; set; } = Priority.Routine;

        public enum Priority : byte
        {
            Routine = 0,

            Priority = 1,

            Emergency = 2
        }

        public enum Status : byte
        {
            Created = 0,

            Submitted_to_S4 = 1,

            Sent_to_SQM = 2,

            Requires_Update = 3,

            Approved = 10,

            // Completed / Paid?

            Rejected = byte.MaxValue
        }
    }

    public class Vendor
    {
        public String Name { get; set; }

        public String PointOfContact { get; set; }

        public String Phone { get; set; }

        public String PointOfContactRole { get; set; }

        public Address PhysicalAddress { get; set; } = new Address { };
    }

    public class Address
    {
        public String Line1 { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String ZipCode { get; set; }
    }

}
