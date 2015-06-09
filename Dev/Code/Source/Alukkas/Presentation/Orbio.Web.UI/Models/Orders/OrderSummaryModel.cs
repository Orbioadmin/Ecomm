using Orbio.Core.Domain.Orders;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public class OrderSummaryModel
    {
        public OrderSummaryModel()
        {
            this.items = new List<OrderSummaryModel>();
        }

        public OrderSummaryModel(OrderSummary orderDetail)
        {
            this.items = new List<OrderSummaryModel>();
            this.CustomerId = orderDetail.CustomerId;
            this.OrderNumber = orderDetail.OrderNumber;
            this.Quantity = orderDetail.Quantity;
            this.OrderStatus = orderDetail.OrderStatus;
            this.TotalPrice = orderDetail.TotalPrice;
            this.OrderDate = orderDetail.OrderDate;
        }

        public int OrderNumber { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; }

        public int CustomerId { get; set; }

        public List<OrderSummaryModel> items { get; set; }
    }
}