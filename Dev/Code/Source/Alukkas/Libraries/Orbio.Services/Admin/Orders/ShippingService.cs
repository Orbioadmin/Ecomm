using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public partial class ShippingService:IShippingService
    {
        #region GetShippingMethods

        /// <summary>
        /// Get all shipping methods
        /// </summary>
        public List<ShippingMethod> GetAllShippingMethods()
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ShippingMethods.ToList();
                return result;
            }
        }

        /// <summary>
        /// Select Shipping Ifo
        /// </summary>
        public Shipment SelectShippingInfoByOrderId(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var shipping = context.Shipments.AsQueryable().Where(o => o.OrderId == id);

                return shipping.FirstOrDefault();
            }
        }

        /// <summary>
        /// insert Shipping Ifo
        /// </summary>
        public void InsertShippingInfo(Shipment shipment)
        {
            using (var context = new OrbioAdminContext())
            {

                var shipmentdata = context.Shipments.FirstOrDefault();
                shipmentdata.OrderId = shipment.OrderId;
                shipmentdata.TrackingNumber = shipment.TrackingNumber;
                shipmentdata.TotalWeight = shipment.TotalWeight;
                shipmentdata.ShippedDateUtc = shipment.ShippedDateUtc;
                shipmentdata.DeliveryDateUtc = shipment.DeliveryDateUtc;
                shipmentdata.CreatedOnUtc = DateTime.UtcNow;
                context.Shipments.Add(shipmentdata);
                context.SaveChanges();
                var id = shipmentdata.Id;
                foreach (var item in shipment.ShipmentItems)
                {
                    var shipmentItemdata = context.ShipmentItems.FirstOrDefault();
                    shipmentItemdata.OrderItemId = item.OrderItemId;
                    shipmentItemdata.Quantity = item.Quantity;
                    shipmentItemdata.ShipmentId = id;
                    context.ShipmentItems.Add(shipmentItemdata);
                }
                context.SaveChanges();

            }
        }

        /// <summary>
        /// Update Shipping Ifo
        /// </summary>
        public void UpdateShippingInfo(Shipment shipment)
        {
            using (var context = new OrbioAdminContext())
            {
                Orbio.Core.Data.Shipment shipmentdata = context.Shipments.First(a => a.Id == shipment.Id);
                shipmentdata.TrackingNumber = shipment.TrackingNumber;
                shipmentdata.TotalWeight = shipment.TotalWeight;
                shipmentdata.ShippedDateUtc = shipment.ShippedDateUtc;
                shipmentdata.DeliveryDateUtc = shipment.DeliveryDateUtc;
                context.SaveChanges();
            }
        }

        #endregion
    }
}
