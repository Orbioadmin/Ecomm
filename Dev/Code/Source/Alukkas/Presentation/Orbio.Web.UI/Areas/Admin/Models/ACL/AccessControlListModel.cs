using Orbio.Core.Data;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.ACL
{
    public class AccessControlListModel
    {
        public AccessControlListModel(PermissionRecord p)
        {
            Id = p.Id;
            PermissionName = p.SystemName;
            if (p.CustomerRoles != null)
            {
                SelectedCustomerRoles = (from r in p.CustomerRoles
                                         select new CustomerRoleModel()
                                         {
                                             Id = r.Id,
                                             Name = r.Name,
                                         }).ToList();
            }
        }

        public int Id { get; set; }

        public string PermissionName { get; set; }

        public List<CustomerRoleModel> SelectedCustomerRoles { get; set; }

        public List<int> SelectedRoles { get; set; }
    }
}