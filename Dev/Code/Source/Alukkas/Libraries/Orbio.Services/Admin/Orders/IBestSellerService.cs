using Orbio.Core.Domain.Admin.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public interface IBestSellerService
    {
        BestSellers GetAllDetailsForSearch();

        List<BestSellers> GetAllSellerDetails(DateTime? StartDate, DateTime? EndDate, int OrderStatusId, int PaymentStatusId, int Category, int Manufacturer);
    }
}
