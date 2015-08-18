using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public partial interface IOrderProcessingService
    {
        /// <summary>
        /// Cancels order
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        void CancelOrder(Orbio.Core.Domain.Orders.Order order);

        /// <summary>
        /// Marks order as paid
        /// </summary>
        /// <param name="order">Order</param>
        void MarkOrderAsPaid(Orbio.Core.Domain.Orders.Order order);

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        void DeleteOrder(Orbio.Core.Domain.Orders.Order order);
    }
}
