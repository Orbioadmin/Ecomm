using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipments
{
    public class ShipmentService : IShipmentService
    {
        public List<Country> GetAllCountries()
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Countries.Include("StateProvinces").Where(m => m.Published).OrderBy(m => m.Name).ToList();
                return result;
            }
        }

        public List<Shipment> GetAllShipmentDetails(DateTime? startDateValue, DateTime? endDateValue, string trackingNumber, int countryId, int stateProvinceId)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = context.Shipments
                    .Include("Order.OrderItems.Product")
                    .Include("ShipmentItems")
                    .Include("Order.Address.StateProvince")
                    .Include("Order.Address.Country")
                    .Where(s => s.Order != null && !s.Order.Deleted);
                if (!String.IsNullOrEmpty(trackingNumber))
                    query = query.Where(s => s.TrackingNumber.Contains(trackingNumber));
                if (countryId > 0)
                    query = query.Where(s => s.Order.Address.CountryId == countryId);
                if (stateProvinceId > 0)
                    query = query.Where(s => s.Order.Address.StateProvinceId == stateProvinceId);
                if (startDateValue.HasValue)
                    query = query.Where(s => startDateValue.Value <= s.CreatedOnUtc);
                if (endDateValue.HasValue)
                    query = query.Where(s => endDateValue.Value >= s.CreatedOnUtc);

                var queryOrderItem=(from c in context.OrderItems
                                    select c.Id);

                query = from s in query
                        where queryOrderItem.Intersect(s.ShipmentItems.Select(si => si.OrderItemId)).Any()
                        select s;

                query = query.OrderByDescending(s => s.CreatedOnUtc);

                return query.ToList();
            }
        }

        public int DeleteShipment(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var query = context.Shipments.Where(m => m.Id == Id).FirstOrDefault();
                    if (query != null)
                    {
                        context.Shipments.Remove(query);
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }
    }
}