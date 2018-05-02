using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StateActiveDuty.Web.Models
{
    public class PurchaseOrderEditModel
    {
        public PurchaseOrder Order { get; set; } = new PurchaseOrder { };

        public IEnumerable<SelectListItem> Units { get; set; }

        public IEnumerable<SelectListItem>
    }
}