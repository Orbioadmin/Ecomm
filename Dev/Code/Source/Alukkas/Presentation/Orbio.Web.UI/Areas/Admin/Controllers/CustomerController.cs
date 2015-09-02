using Orbio.Core.Data;
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
        private readonly ICustomerRoleService customerRoleService;

        #endregion

        #region Ctor

        public CustomerController(ICustomerReportService customerReportService, ICustomerRoleService customerRoleService)
        {
            this._customerReportService = customerReportService;
            this.customerRoleService = customerRoleService;
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

        #region CustomerRole

        public ActionResult ListCustomerRole()
        {
            var result = customerRoleService.GetAllCustomerRole();
            var model = (from CR in result
                         select new CustomerRoleModel(CR)).ToList();

            return View("ListCustomerRole",model);
        }

        public ActionResult AddCustomerRole()
        {
            var model = new CustomerRoleModel();
            return View("AddOrEditCustomerRole", model);
        }

        public ActionResult EditCustomerRole(int Id)
        {
            var result = customerRoleService.GetCustomerRoleById(Id);
            var model = new CustomerRoleModel(result);
            return View("AddOrEditCustomerRole", model);
        }

        public ActionResult DeleteCustomerRole(int Id)
        {
            int result = customerRoleService.DeleteCustomerRole(Id);
            return RedirectToAction("ListCustomerRole");
        }

        public ActionResult AddOrEditCustomerRole(CustomerRoleModel model)
        {
            var customerRole = new CustomerRole
                                {
                                    Id=model.Id,
                                    Name=model.Name,
                                    SystemName=model.SystemName,
                                    FreeShipping=model.FreeShipping,
                                    TaxExempt=model.TaxExempt,
                                    Active=model.Active,
                                };
            int result = customerRoleService.AddOrUpdateCustomerRole(customerRole);
            return RedirectToAction("ListCustomerRole");
        }

        #endregion

    }
}