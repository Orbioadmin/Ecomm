using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public partial class OrderAverageReportLineSummaryModel
    {
        [Display(Name = "Admin.SalesReport.Average.OrderStatus")]
        public string OrderStatus { get; set; }

        [Display(Name = "Admin.SalesReport.Average.SumTodayOrders")]
        public string SumTodayOrders { get; set; }

        [Display(Name = "Admin.SalesReport.Average.SumThisWeekOrders")]
        public string SumThisWeekOrders { get; set; }

        [Display(Name = "Admin.SalesReport.Average.SumThisMonthOrders")]
        public string SumThisMonthOrders { get; set; }

        [Display(Name = "Admin.SalesReport.Average.SumThisYearOrders")]
        public string SumThisYearOrders { get; set; }

        [Display(Name = "Admin.SalesReport.Average.SumAllTimeOrders")]
        public string SumAllTimeOrders { get; set; }
    }
}