using Orbio.Core.Domain.Admin.Orders;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
        }

        public ShoppingCartModel(ShoppingCart shoppingCart,int pageNumber,int pageSize):this()
        {
            if (shoppingCart.Customers != null && shoppingCart.Customers.Count > 0)
            {
                this.customers = (from p in shoppingCart.Customers
                                  select new CustomerModel(p)).ToPagedList(pageNumber,pageSize);
            }
        }

        public IPagedList<CustomerModel> customers { get; set; }
    }
}