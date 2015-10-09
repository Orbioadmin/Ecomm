using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipping
{
    public interface IWarehouseService
    {
       List<Warehouse> GetAllWarehouseDetails();

       Warehouse Edit(int id);

       int AddOrUpdate(int id, string name, int addressId, int countryId, int stateProvinceId, string city, string address, string pincode);

       int Delete(int id);
    }
}
