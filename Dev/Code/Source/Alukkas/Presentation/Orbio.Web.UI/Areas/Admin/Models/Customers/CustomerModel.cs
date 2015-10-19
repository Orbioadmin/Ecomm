
using Orbio.Core.Domain.Admin.Customers;
using Orbio.Core.Domain.Checkout;
using Orbio.Web.UI.Areas.Admin.Models.Discount;
using Orbio.Web.UI.Areas.Admin.Models.Orders;
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

        public CustomerModel(Orbio.Core.Data.Customer customer)
        {
            this.Id = customer.Id;
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.Email = customer.Email;
            this.Active = customer.Active;
            this.Gender = customer.Gender;
            this.DOB = customer.DOB;
            this.AdminComment = customer.AdminComment;
            this.IsTaxExempt = customer.IsTaxExempt;
            this.CreatedOn = customer.CreatedOnUtc;
            this.LastActivity = customer.LastActivityDateUtc;
            if (customer.CustomerRoles != null)
            {
                this.CustomerRoles = (from C in customer.CustomerRoles
                                      select new CustomerRoleModel(C)).ToList();
            }

            //this.CompanyName = customer.CompanyName;
            //this.IPAddress = customer.LastIpAddress;
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string DOB { get; set; }

        public string AdminComment { get; set; }

        public bool IsTaxExempt { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastActivity { get; set; }

        public List<int> Roles { get; set; }

        public List<int> Discounts { get; set; }

        public List<DiscountModel> DiscountList { get; set; }

        public List<CustomerRoleModel> CustomerRoles { get; set; }

        public List<OrderModel> Order { get; set; }

        public CartModel shoppingCart { get; set; }

        //shipping info
        public Address ShippingAddress { get; set; }

        //billing info

        public Address BillingAddress { get; set; }
    }
}