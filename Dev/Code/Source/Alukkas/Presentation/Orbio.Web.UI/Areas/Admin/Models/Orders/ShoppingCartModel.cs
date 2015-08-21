using Orbio.Core.Domain.Admin.Orders;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
            this.customers = new List<CustomerModel>();
        }

        public ShoppingCartModel(ShoppingCart shoppingCart):this()
        {
            if (shoppingCart.Customers != null && shoppingCart.Customers.Count > 0)
            {
                this.customers = (from p in shoppingCart.Customers
                                  select new CustomerModel(p)).ToList();
            }
        }

        public List<CustomerModel> customers { get; set; }
    }
}