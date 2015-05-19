using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Orders
{
    public partial interface IOrderService
    {
        OrderDetails GetOrderDetails(int curCustomerId);
    }
}
