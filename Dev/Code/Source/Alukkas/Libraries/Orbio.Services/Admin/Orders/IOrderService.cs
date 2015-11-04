using Orbio.Core.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Payments;
using Orbio.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public partial interface IOrderService
    {
        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="customerId">Customer identifier; null to load all orders</param>
        /// <param name="productId">Product identifier which was purchased in an order; 0 to load all orders</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="os">Order status; null to load all orders</param>
        /// <param name="ps">Order payment status; null to load all orders</param>
        /// <param name="ss">Order shippment status; null to load all orders</param>
        /// <param name="billingEmail">Billing email. Leave empty to load all records.</param>
        /// <param name="orderGuid">Search by order GUID (Global unique identifier) or part of GUID. Leave empty to load all records.</param>
        /// <returns>Order collection</returns>
        List<Orbio.Core.Domain.Orders.Order> SearchOrders(int customerId = 0, DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
            OrderStatus? os = null, PaymentStatus? ps = null, ShippingStatus? ss = null,
            string billingEmail = null, int? oderNumber = 0);

       /// <summary>
       /// Get order by Id
       /// </summary>
       /// <param name="orderId">Order identifier; null to load order</param>
       /// <returns>Order</returns>
       Orbio.Core.Domain.Orders.Order GetOrderById(int orderId);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        bool UpdateOrder(Orbio.Core.Domain.Orders.Order order);

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="order">The order</param>
        void UpdateOrderNotes(Orbio.Core.Domain.Orders.Order order);

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        void DeleteOrderItem(Orbio.Core.Domain.Orders.OrderItem orderItem);

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        void DeleteOrder(Orbio.Core.Domain.Orders.Order order);

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        void Delete(int orderId);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="customerId">The customer identifier; 0 to load all records</param>
        /// <param name="initialOrderId">The initial order identifier; 0 to load all records</param>
        /// <param name="initialOrderStatus">Initial order status identifier; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recurring payment collection</returns>
        List<RecurringPayment> SearchRecurringPayments(int customerId, int initialOrderId, OrderStatus? initialOrderStatus,
            int pageIndex, int pageSize, bool showHidden = false);

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="orderNote">The order note</param>
        void DeleteOrderNote(OrderNote orderNote);
    }
}
