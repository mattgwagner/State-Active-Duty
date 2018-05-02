using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace StateActiveDuty.Web.Models
{
    public class PurchaseOrderEditModel
    {
        private readonly HtmlHelper html;

        public PurchaseOrderEditModel(HtmlHelper html)
        {
            this.html = html;
        }

        public PurchaseOrder Order { get; set; } = new PurchaseOrder { };

        public IEnumerable<SelectListItem> Units { get; set; }

        public IEnumerable<SelectListItem> Priorities => html.GetEnumSelectList<PurchaseOrder.OrderPriority>();

        public IEnumerable<SelectListItem> PurchaseCategories => html.GetEnumSelectList<PurchaseOrder.PurchaseCategory>();

        public IEnumerable<SelectListItem> MealCategories => html.GetEnumSelectList<PurchaseOrder.MealCategory>();
    }
}