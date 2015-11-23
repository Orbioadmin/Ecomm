using Orbio.Web.UI.Areas.Admin.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.ACL
{
    public class AccessControlDetailModel
    {
        public List<CustomerRoleModel> Roles { get; set; }

        public List<AccessControlListModel> ACL { get; set; }
    }
}