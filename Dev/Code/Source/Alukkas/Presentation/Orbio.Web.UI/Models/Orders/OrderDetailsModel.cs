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
            this.OrderedProductDetail = new List<OrderSummaryModel>();
        }
          public OrderDetailsModel(List<OrderSummary> orderDetails)
             : this()
         {
             if (orderDetails != null)
             {
                 if (orderDetails != null && orderDetails.Count > 0)
                 {
                     this.OrderedProductDetail = (from p in orderDetails
                                                  select new OrderSummaryModel(p)).ToList();
                 }
             }
         }

         public List<OrderSummaryModel> OrderedProductDetail { get; private set; }
    }
}