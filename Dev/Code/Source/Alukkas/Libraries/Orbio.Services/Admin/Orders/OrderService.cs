using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Payments;
using Orbio.Core.Utility;
using Orbio.Services.Helpers;
using Orbio.Services.Orders;
using Orbio.Services.Payments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public class OrderService:IOrderService
    {
        #region Fields

        private readonly IDbContext dbContext;
        private readonly OrbioAdminContext context = new OrbioAdminContext(); 
        private readonly IDateTimeHelper _dateTimeHelper;
        #endregion

        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public OrderService(IDateTimeHelper dateTimeHelper, IDbContext dbContext)
        {
            this.dbContext = dbContext;
            //this.context = context;
            this._dateTimeHelper = dateTimeHelper;
        }

        #region Methods

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
        public virtual List<Orbio.Core.Domain.Orders.Order> SearchOrders(int customerId = 0, DateTime? createdFromUtc = null, DateTime? createdToUtc = null,
             OrderStatus? os = null, PaymentStatus? ps = null, ShippingStatus? ss = null,
             string billingEmail = null, int? orderNumber = 0)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;

            //var query = context.usp_Get_AdminOrderDetails(orderStatusId, paymentStatusId, shippingStatusId, customerId, createdFromUtc, createdToUtc, billingEmail, orderNumber).ToList();

            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_Get_AdminOrderDetails",
            new SqlParameter() { ParameterName = "@orderStatusId", Value = orderStatusId, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@paymentStatusId", Value = paymentStatusId, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@shippingStatusId", Value = shippingStatusId, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 },
           new SqlParameter() { ParameterName = "@createdFromUtc", Value = createdFromUtc, DbType = System.Data.DbType.DateTime },
           new SqlParameter() { ParameterName = "@createdToUtc", Value = createdToUtc, DbType = System.Data.DbType.DateTime },
           new SqlParameter() { ParameterName = "@billingEmail", Value = billingEmail, DbType = System.Data.DbType.String },
           new SqlParameter() { ParameterName = "@orderNo", Value = orderNumber, DbType = System.Data.DbType.Int32 }
       ).FirstOrDefault();

            if (result != null && result.XmlResult != null)
            {
                var orders = Serializer.GenericDeSerializer<List<Orbio.Core.Domain.Orders.Order>>(result.XmlResult);
                return orders;
            }

            return new List<Orbio.Core.Domain.Orders.Order>();

        }


        /// <summary>
        /// Get order by Id
        /// </summary>
        /// <param name="orderId">Order identifier; null to load order</param>
        /// <returns>Order</returns>
        public Orbio.Core.Domain.Orders.Order GetOrderById(int orderId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@Id", Value = orderId, DbType = System.Data.DbType.Int32 });
            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_OrbioAdmin_OrderDetailsById", sqlParamList.ToArray()).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var order = Serializer.GenericDeSerializer<Orbio.Core.Domain.Orders.Order>(result.XmlResult);
                return order;
            }

            return new Orbio.Core.Domain.Orders.Order();
        }

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual bool UpdateOrder(Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            bool result = UpdateOrderDetail(order);

            return result;
            //event notification
            //_eventPublisher.EntityUpdated(order);
        }

        /// <summary>
        /// Delete an order item
        /// </summary>
        /// <param name="orderItem">The order item</param>
        public virtual void DeleteOrderItem(Orbio.Core.Domain.Orders.OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException("orderItem");

            dbContext.ExecuteFunction<OrderNote>("usp_Delete_OrderItem",
                new SqlParameter() { ParameterName = "@id", Value = orderItem.Id, DbType = System.Data.DbType.Int32 });
        }


        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void DeleteOrder(Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");
            order.Deleted = true;
            UpdateOrder(order);
        }

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
        public virtual List<RecurringPayment> SearchRecurringPayments(int customerId, int initialOrderId, OrderStatus? initialOrderStatus,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            int? initialOrderStatusId = null;
            if (initialOrderStatus.HasValue)
                initialOrderStatusId = (int)initialOrderStatus.Value;

            var query1 = from rp in context.RecurringPayments.AsQueryable()
                         join c in context.Customers.AsQueryable() on rp.Order.CustomerId equals c.Id
                         where
                         (!rp.Deleted) &&
                         (showHidden || !rp.Order.Deleted) &&
                         (showHidden || !c.Deleted) &&
                         (showHidden || rp.IsActive) &&
                         (customerId == 0 || rp.Order.CustomerId == customerId) &&
                         (initialOrderId == 0 || rp.Order.Id == initialOrderId) &&
                         (!initialOrderStatusId.HasValue || initialOrderStatusId.Value == 0 || rp.Order.OrderStatusId == initialOrderStatusId.Value)
                         select rp.Id;

            var query2 = from rp in context.RecurringPayments.AsQueryable()
                         where query1.Contains(rp.Id)
                         orderby rp.StartDateUtc, rp.Id
                         select rp;

            //var recurringPayments = new List<RecurringPayment>(query2, pageIndex, pageSize);
            var recurringPayments = new List<RecurringPayment>(query2);
            return recurringPayments;
        }

        private bool UpdateOrderDetail(Orbio.Core.Domain.Orders.Order order)
        {
            var orderXml = Serializer.GenericSerializer<Orbio.Core.Domain.Orders.Order>(order);
            var sqlParams = new SqlParameter[] {new SqlParameter{ ParameterName="@orderXml", 
            Value=orderXml, DbType= System.Data.DbType.Xml}, new SqlParameter{ ParameterName="@updateSucess", 
            Value=0, DbType= System.Data.DbType.Boolean, Direction= System.Data.ParameterDirection.Output} };
            var result1 = dbContext.ExecuteSqlCommand("EXEC [usp_Order_UpdateOrder] @orderXml, @updateSucess OUTPUT", false, null, sqlParams);
            return Convert.ToBoolean(sqlParams[1].Value);
        }


        public void UpdateOrderNotes(Orbio.Core.Domain.Orders.Order order)
        {
            var orderNoteXml = Serializer.GenericSerializer(order.OrderNotes);
            dbContext.ExecuteFunction<OrderNote>("usp_Update_OrderNote",
               new SqlParameter() { ParameterName = "@orderId", Value = order.OrderId, DbType = System.Data.DbType.Int32 },
               new SqlParameter() { ParameterName = "@note", Value = orderNoteXml, DbType = System.Data.DbType.Xml });
        }

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="orderNote">The order note</param>
        public virtual void DeleteOrderNote(OrderNote orderNote)
        {
            if (orderNote == null)
                throw new ArgumentNullException("orderNote");

            dbContext.ExecuteFunction<OrderNote>("usp_OrderNote_DeleteOrderNote",
               new SqlParameter() { ParameterName = "@id", Value = orderNote.Id, DbType = System.Data.DbType.Int32 });

        }

        #endregion
    }
}
