using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Shipments
{
    public class ShipmentModel
    {
        public int Id { get; set; }
      
        public int OrderId { get; set; }
       
        public decimal? TotalWeight { get; set; }
       
        public string TrackingNumber { get; set; }

        public DateTime? ShippedDateUtc { get; set; }

        public DateTime? DeliveryDateUtc { get; set; }

        public string ShipmentMethod { get; set; }

        public List<ShipmentItemModel> ShipmentItems { get; set; }
    }
}