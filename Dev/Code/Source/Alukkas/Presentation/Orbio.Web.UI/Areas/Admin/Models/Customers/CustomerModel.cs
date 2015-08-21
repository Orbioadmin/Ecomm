
using Orbio.Core.Domain.Admin.Customers;
using Orbio.Web.UI.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Customers
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            // TODO: Complete member initialization
        }

        public CustomerModel(Customer customer)
            : this()
        {
            this.Id = customer.Id;
            this.Email = customer.Email;
            this.shoppingCart = new CartModel(customer.Cart);
        }
        public int Id { get; set; }

        public string Email { get; set; }

        public CartModel shoppingCart { get; set; }
    }
}