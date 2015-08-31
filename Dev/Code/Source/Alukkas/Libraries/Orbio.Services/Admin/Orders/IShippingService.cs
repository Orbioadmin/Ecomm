using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public partial interface IShippingService
    {
        /// <summary>
        /// Get all shipping methods
        /// </summary>
        List<ShippingMethod> GetAllShippingMethods();

        /// <summary>
        /// Select Shipping Ifo
        /// </summary>
        Shipment SelectShippingInfoByOrderId(int id);

        /// <summary>
        /// insert Shipping Ifo
        /// </summary>
        void InsertShippingInfo(Shipment shipment);

        /// <summary>
        /// Update Shipping Ifo
        /// </summary>
        void UpdateShippingInfo(Shipment shipment);
    }
}
