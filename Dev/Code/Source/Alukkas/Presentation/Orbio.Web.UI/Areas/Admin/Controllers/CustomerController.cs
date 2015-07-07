using Orbio.Services.Admin.Customers;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
       #region Fields

        private readonly ICustomerReportService _customerReportService;

        #endregion

        #region Ctor

        public CustomerController(ICustomerReportService customerReportService)
        {
            this._customerReportService = customerReportService;
        }

        #endregion
        // GET: Admin/Customer
        public ActionResult Index()
        {
            return View();
        }

        protected IList<RegisteredCustomerReportLineModel> GetReportRegisteredCustomersModel()
        {
            var report = new List<RegisteredCustomerReportLineModel>();
            report.Add(new RegisteredCustomerReportLineModel()
            {
                Period = "In the last 7 days",
                Customers = _customerReportService.GetRegisteredCustomersReport(7)
            });

            report.Add(new RegisteredCustomerReportLineModel()
            {
                Period = "In the last 14 days",
                Customers = _customerReportService.GetRegisteredCustomersReport(14)
            });
            report.Add(new RegisteredCustomerReportLineModel()
            {
                Period = "In the last month",
                Customers = _customerReportService.GetRegisteredCustomersReport(30)
            });
            report.Add(new RegisteredCustomerReportLineModel()
            {
                Period = "In the last year",
                Customers = _customerReportService.GetRegisteredCustomersReport(365)
            });

            return report;
        }

        [ChildActionOnly]
        public ActionResult ReportRegisteredCustomers()
        {
            var model = GetReportRegisteredCustomersModel();
            return PartialView(model);
        }

    }
}