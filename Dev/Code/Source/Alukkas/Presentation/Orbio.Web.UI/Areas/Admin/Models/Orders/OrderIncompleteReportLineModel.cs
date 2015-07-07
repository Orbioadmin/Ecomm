using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public partial class OrderIncompleteReportLineModel
    {
        public string Item { get; set; }

        public string Total { get; set; }

        public int Count { get; set; }
    }
}