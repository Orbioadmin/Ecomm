using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Customers
{
    public class CustomerRoleModel
    {
        private ICollection<CustomerRole> collection;

        public CustomerRoleModel()
        {
        }

        public CustomerRoleModel(CustomerRole cust)
        {
            Id = cust.Id;
            Name = cust.Name;
            SystemName = cust.SystemName;
            FreeShipping = cust.FreeShipping;
            TaxExempt = cust.TaxExempt;
            Active = cust.Active;
            IsSystemRole = cust.IsSystemRole;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string SystemName { get; set; }

        public bool FreeShipping { get; set; }

        public bool TaxExempt { get; set; }

        public bool Active { get; set; }

        public bool IsSystemRole { get; set; }
    }
}