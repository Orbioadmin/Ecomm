using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Customers
{
    public class NewletterSubscribersModel
    {
        public List<CustomerModel> Subscribers { get; set; }

        public string Search { get; set; }
    }
}