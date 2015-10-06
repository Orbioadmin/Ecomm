using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipping
{
    public interface IDeliveryDateService
    {
        List<DeliveryDate> GetAllDeliveryDate();

        int AddOrUpdate(int id, string name, int displayOrder);

        void Delete(int id);
    }
}
