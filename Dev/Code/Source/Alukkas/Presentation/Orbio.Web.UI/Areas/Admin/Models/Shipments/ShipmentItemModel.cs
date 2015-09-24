using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Shipments
{
    public class ShipmentItemModel
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public string AttributeInfo { get; set; }

        //weight of one item (product)
        public decimal? ItemWeight { get; set; }
        public string ItemDimensions { get; set; }
        public int QuantityToAdd { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityInThisShipment { get; set; }
        public int QuantityInAllShipments { get; set; }

        public int ShipmentId { get; set; }
    }
}