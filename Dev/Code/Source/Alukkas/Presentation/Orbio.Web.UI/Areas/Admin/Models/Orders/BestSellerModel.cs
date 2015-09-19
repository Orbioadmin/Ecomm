using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public class BestSellerModel
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}