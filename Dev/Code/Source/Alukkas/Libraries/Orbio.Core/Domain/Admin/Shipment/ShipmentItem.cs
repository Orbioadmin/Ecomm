using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Shipment
{
    public class ShipmentItem
    {
        /// <summary>
        /// Gets or sets the shipment identifier
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// Gets or sets the order item identifier
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets the shipment
        /// </summary>
        public virtual Shipment Shipment { get; set; }
    }
}
