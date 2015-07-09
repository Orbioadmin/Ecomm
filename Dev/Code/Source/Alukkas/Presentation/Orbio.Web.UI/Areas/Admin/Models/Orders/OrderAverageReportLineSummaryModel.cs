using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public partial class OrderAverageReportLineSummaryModel
    {
        public string OrderStatus { get; set; }

        public int CountTodayOrders { get; set; }

        public string SumTodayOrders { get; set; }

        public int CountThisWeekOrders { get; set; }

        public string SumThisWeekOrders { get; set; }

        public int CountThisMonthOrders { get; set; }

        public string SumThisMonthOrders { get; set; }

        public int CountThisYearOrders { get; set; }

        public string SumThisYearOrders { get; set; }

        public int CountAllTimeOrders { get; set; }

        public string SumAllTimeOrders { get; set; }
    }
}