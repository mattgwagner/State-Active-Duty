using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StateActiveDuty.Web.Models
{
    public class Unit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Name { get; set; } = "C BTRY 2-116TH FA";

        public String Phone { get; set; } = "3527321213";

        public String CommandOrTaskForce { get; set; } = "53 IBCT";

        public PointOfContact POC { get; set; } = new PointOfContact { };
    }
}