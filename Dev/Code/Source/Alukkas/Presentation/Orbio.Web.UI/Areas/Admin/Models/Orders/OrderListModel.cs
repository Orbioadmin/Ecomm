using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public partial class OrderListModel
    {
        public OrderListModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
            AvailableShippingStatuses = new List<SelectListItem>();
        }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CustomerEmail { get; set; }

        public int OrderStatusId { get; set; }

        public int PaymentStatusId { get; set; }

        public int ShippingStatusId { get; set; }

        public string OrderGuid { get; set; }

        public int? GoDirectlyToNumber { get; set; }

        public bool IsLoggedInAsVendor { get; set; }


        public IList<SelectListItem> AvailableOrderStatuses { get; set; }
        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }
        public IList<SelectListItem> AvailableShippingStatuses { get; set; }

    }
}