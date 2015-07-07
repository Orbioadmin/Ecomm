using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Customers
{
    public partial class RegisteredCustomerReportLineModel
    {
        public string Period { get; set; }

        public int Customers { get; set; }
    }
}