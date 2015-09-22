using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipments
{
    public interface IShipmentService
    {
        List<Country> GetAllCountries();

        List<Shipment> GetAllShipmentDetails(DateTime? startDateValue, DateTime? endDateValue, string TrackingNumber, int CountryId, int StateProvinceId);

        int DeleteShipment(int Id);
    }
}
