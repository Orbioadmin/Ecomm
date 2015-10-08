using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipping
{
    public interface IShippingMethodService
    {
        List<ShippingMethod> GetAllShippingMethods();

        void AddOrUpdate(int id, string name, string description, int displayOrder);

        void Delete(int id);
    }
}
