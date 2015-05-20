using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public class OrderDetailsModel
    {
          public OrderDetailsModel()
        {
            this.OrderedProductDetail = new List<OrderModel>();
        }
          public OrderDetailsModel(OrderDetails orderDetails)
             : this()
         {
             if (orderDetails != null)
             {
                 if (orderDetails.OrderedProductItems != null && orderDetails.OrderedProductItems.Count > 0)
                 {
                     this.OrderedProductDetail = (from p in orderDetails.OrderedProductItems
                                                  select new OrderModel(p)).ToList();
                 }
             }
         }

         public List<OrderModel> OrderedProductDetail { get; private set; }
    }
}