
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

        public CustomerModel(Orbio.Core.Data.Customer customer)
        {
            this.Id = customer.Id;
            this.Name = (!string.IsNullOrEmpty(customer.FirstName) && !string.IsNullOrEmpty(customer.LastName)) ? customer.FirstName + " " + customer.LastName : "Guest";
            this.Email = customer.Email;
            this.CompanyName = customer.CompanyName;
            this.IPAddress = customer.LastIpAddress;
            //this.Location=customer.lo
            this.Active = customer.Active;
            this.CreatedOn = customer.CreatedOnUtc;
            this.LastActivity = customer.LastActivityDateUtc;
            if (customer.CustomerRoles != null)
            {
                this.CustomerRoles = (from C in customer.CustomerRoles
                                      select new CustomerRoleModel(C)).ToList();
            }
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string IPAddress { get; set; }

        public string Location { get; set; }

        public string LastVisitedPage { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastActivity { get; set; } 

        public List<CustomerRoleModel> CustomerRoles { get; set; }

        public CartModel shoppingCart { get; set; }
    }
}