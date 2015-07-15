using System.Collections.Generic;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Payments;

namespace Orbio.Services.Orders
{
    public partial interface IOrderService
    {
        List<OrderSummary> GetOrderDetails(int curCustomerId);
        string PlaceOrder(ProcessOrderRequest processOrderRequest);
    }
}
