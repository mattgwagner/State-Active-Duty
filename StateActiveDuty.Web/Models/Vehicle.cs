using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StateActiveDuty.Web.Models
{
    public class Vehicle
    {
        public Status VehicleStatus { get; set; } = Status.FMC;

        public virtual Unit Unit { get; set; }

        // Chalk Order?

        // LIN?

        public String Bumper { get; set; }

        public String Nomenclature { get; set; }

        public String Registration { get; set; }

        public String Serial { get; set; }

        // Fuel Card? Towbar? Water Buffalo?

        // Fuel Level?

        public enum Status : byte
        {
            Unknown = byte.MinValue,

            FMC = 0,

            NMC = byte.MaxValue
        }
    }
}
