using Orbio.Core.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Payments;
using Orbio.Services.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public class OrderProcessingService:IOrderProcessingService
    {
        #region Fields

        private readonly OrbioAdminContext context = new OrbioAdminContext();
        private readonly IOrderService _orderService;
        private readonly IMessageService _messageService;

        #endregion


        /// instantiates OrderProcessing service type
        /// </summary>
        /// <param name="context">db context</param>
        public OrderProcessingService(IOrderService orderService,IMessageService messageService)
        {
            this._orderService = orderService;
            this._messageService = messageService;
        }

        #region Methods

        /// <summary>
        /// Checks order status
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Validated order</returns>
        public virtual void CheckOrderStatus(Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (order.PaymentStatus == PaymentStatus.Paid && !order.PaidDateUtc.HasValue)
            {
                //ensure that paid date is set
                order.PaidDateUtc = DateTime.UtcNow;
                _orderService.UpdateOrder(order);
            }

            if (order.OrderStatus == OrderStatus.Pending)
            {
                if (order.PaymentStatus == PaymentStatus.Authorized ||
                    order.PaymentStatus == PaymentStatus.Paid)
                {
                    SetOrderStatus(order, OrderStatus.Processing, false);
                }
            }

            if (order.OrderStatus == OrderStatus.Pending)
            {
                if (order.ShippingStatus == ShippingStatus.PartiallyShipped ||
                    order.ShippingStatus == ShippingStatus.Shipped ||
                    order.ShippingStatus == ShippingStatus.Delivered)
                {
                    SetOrderStatus(order, OrderStatus.Processing, false);
                }
            }

            if (order.OrderStatus != OrderStatus.Cancelled &&
                order.OrderStatus != OrderStatus.Complete)
            {
                if (order.PaymentStatus == PaymentStatus.Paid)
                {
                    if (order.ShippingStatus == ShippingStatus.ShippingNotRequired || order.ShippingStatus == ShippingStatus.Delivered)
                    {
                        SetOrderStatus(order, OrderStatus.Complete, true);
                    }
                }
            }
        }

        /// <summary>
        /// Sets an order status
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="os">New order status</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        protected virtual void SetOrderStatus(Orbio.Core.Domain.Orders.Order order, OrderStatus os, bool notifyCustomer)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            OrderStatus prevOrderStatus = order.OrderStatus;
            if (prevOrderStatus == os)
                return;

            //set and save new order status
            order.OrderStatusId = (int)os;
            _orderService.UpdateOrder(order);

            //order notes, notifications
            order.OrderNotes.Clear();
            order.OrderNotes.Add(new OrderNote()
            {
                Note = string.Format("Order status has been changed to {0}", os.ToString()),
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrderNotes(order);


            if (prevOrderStatus != OrderStatus.Complete &&
                os == OrderStatus.Complete
                && notifyCustomer)
            {
                //notification
                var orderCompletedAttachmentFilePath = "";
                var orderCompletedAttachmentFileName = "";
                int orderCompletedCustomerNotificationQueuedEmailId = _messageService
                    .SendOrderCompletedCustomerNotification(order, order.CustomerLanguageId, orderCompletedAttachmentFilePath,
                    orderCompletedAttachmentFileName);
                if (orderCompletedCustomerNotificationQueuedEmailId > 0)
                {
                    _orderService.UpdateOrder(order);
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = string.Format("\"Order completed\" email (to customer) has been queued. Queued email identifier: {0}.", orderCompletedCustomerNotificationQueuedEmailId),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrderNotes(order);
                }
            }

            if (prevOrderStatus != OrderStatus.Cancelled &&
                os == OrderStatus.Cancelled
                && notifyCustomer)
            {
                //notification
                int orderCancelledCustomerNotificationQueuedEmailId = _messageService.SendOrderCancelledCustomerNotification(order, order.CustomerLanguageId);
                if (orderCancelledCustomerNotificationQueuedEmailId > 0)
                {
                    _orderService.UpdateOrder(order);
                    order.OrderNotes.Clear();
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = string.Format("\"Order cancelled\" email (to customer) has been queued. Queued email identifier: {0}.", orderCancelledCustomerNotificationQueuedEmailId),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrderNotes(order);
                }
            }
        }

        /// <summary>
        /// Cancels order
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        public virtual void CancelOrder(Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //Cancel order
            SetOrderStatus(order, OrderStatus.Cancelled, true);
            _orderService.UpdateOrder(order);
            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been cancelled",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrderNotes(order);

            //cancel recurring payments
            var recurringPayments = _orderService.SearchRecurringPayments(0, order.OrderId, null, 0, int.MaxValue);
            //foreach (var rp in recurringPayments)
           // {
                //use errors?
                //var errors = CancelRecurringPayment(rp);
            //}

            //Adjust inventory
            //foreach (var orderItem in order.OrderItems)
                //_productService.AdjustInventory(orderItem.Product, false, orderItem.Quantity, orderItem.AttributesXml);

            //_eventPublisher.PublishOrderCancelled(order);

        }

        /// <summary>
        /// Marks order as paid
        /// </summary>
        /// <param name="order">Order</param>
        public virtual void MarkOrderAsPaid(Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");


            order.PaymentStatusId = (int)PaymentStatus.Paid;
            order.PaidDateUtc = DateTime.UtcNow;
            _orderService.UpdateOrder(order);
            //add a note
            order.OrderNotes.Clear();
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been marked as paid",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrderNotes(order);
            

            CheckOrderStatus(order);

            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                //raise event
                //_eventPublisher.PublishOrderPaid(order);

                //order paid email notification
               // _messageService.SendOrderPaidStoreOwnerNotification(order, _localizationSettings.DefaultAdminLanguageId);
            }
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="order">The order</param>
        public virtual void DeleteOrder(Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (order.OrderStatus != OrderStatus.Cancelled)
            {
                //cancel recurring payments
                var recurringPayments = _orderService.SearchRecurringPayments(0, order.OrderId, null, 0, int.MaxValue);
                foreach (var rp in recurringPayments)
                {
                    //use errors?
                    //var errors = CancelRecurringPayment(rp);
                }

                //Adjust inventory
                //foreach (var orderItem in order.OrderItems)
                //    _productService.AdjustInventory(orderItem.Product, false, orderItem.Quantity, orderItem.AttributesXml);
            }

            //now delete an order
            _orderService.DeleteOrder(order);

            //add a note
            order.OrderNotes.Clear();
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been deleted",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrderNotes(order);
        }


        #endregion
    }
}
