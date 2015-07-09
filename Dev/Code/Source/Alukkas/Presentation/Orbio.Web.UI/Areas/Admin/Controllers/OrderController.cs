using Orbio.Core.Domain.Admin.Orders;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Services.Admin;
using Orbio.Services.Admin.Orders;
using Orbio.Services.Orders;
using Orbio.Services.Payments;
using Orbio.Web.UI.Areas.Admin.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        #region Fields

        private readonly IOrderReportService _orderReportService;

        #endregion

        #region Ctor

        public OrderController(IOrderReportService orderReportService)
        {
            this._orderReportService = orderReportService;
        }

        #endregion

        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }

        protected virtual IList<OrderAverageReportLineSummaryModel> GetOrderAverageReportModel()
        {
            var report = new List<OrderAverageReportLineSummary>();
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Pending));
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Processing));
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Complete));
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Cancelled));
            var model = report.Select(x =>
            {
                return new OrderAverageReportLineSummaryModel()
                {
                    OrderStatus = x.OrderStatus.ToString(),
                    CountTodayOrders = x.CountTodayOrders,
                    SumTodayOrders = x.SumTodayOrders.ToString("#,##0.00"), 
                    CountThisWeekOrders = x.CountThisWeekOrders,
                    SumThisWeekOrders = x.SumThisWeekOrders.ToString("#,##0.00"),
                    CountThisMonthOrders = x.CountThisMonthOrders,
                    SumThisMonthOrders = x.SumThisMonthOrders.ToString("#,##0.00"),
                    CountThisYearOrders = x.CountThisYearOrders,
                    SumThisYearOrders = x.SumThisYearOrders.ToString("#,##0.00"),
                    CountAllTimeOrders = x.CountAllTimeOrders,
                    SumAllTimeOrders = x.SumAllTimeOrders.ToString("#,##0.00"),
                };
            }).ToList();

            return model;
        }

        [ChildActionOnly]
        public ActionResult OrderAverageReport()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
            //    return Content("");

            var model = GetOrderAverageReportModel();
            return PartialView(model);
       }

        protected virtual IList<OrderIncompleteReportLineModel> GetOrderIncompleteReportModel()
        {
            var model = new List<OrderIncompleteReportLineModel>();
            //not paid
            var psPending = _orderReportService.GetOrderAverageReportLine(0, 0, null, PaymentStatus.Pending, null, null, null, null, true);
            model.Add(new OrderIncompleteReportLineModel()
            {
                Item = "Total unpaid order (pending payment status)",//_localizationService.GetResource("Admin.SalesReport.Incomplete.TotalUnpaidOrders"),
                Count = psPending.CountOrders,
                Total = psPending.SumOrders.ToString("#,##0.00")
            });
            //not shipped
            var ssPending = _orderReportService.GetOrderAverageReportLine(0, 0, null, null, ShippingStatus.NotYetShipped, null, null, null, true);
            model.Add(new OrderIncompleteReportLineModel()
            {
                Item = "Total not yet shipped orders",//_localizationService.GetResource("Admin.SalesReport.Incomplete.TotalNotShippedOrders"),
                Count = ssPending.CountOrders,
                Total = ssPending.SumOrders.ToString("#,##0.00")
            });
            //pending
            var osPending = _orderReportService.GetOrderAverageReportLine(0, 0, OrderStatus.Pending, null, null, null, null, null, true);
            model.Add(new OrderIncompleteReportLineModel()
            {
                Item = "Total incomplete orders (pending payment status)",//_localizationService.GetResource("Admin.SalesReport.Incomplete.TotalIncompleteOrders"),
                Count = osPending.CountOrders,
                Total = osPending.SumOrders.ToString("#,##0.00")
            });
            return model;
        }

        [ChildActionOnly]
        public ActionResult OrderIncompleteReport()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
            //    return Content("");

            var model = GetOrderIncompleteReportModel();
            return PartialView(model);
        }
    }
}