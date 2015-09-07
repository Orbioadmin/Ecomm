using Orbio.Core.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Web.Framework;
using Orbio.Core.Payments;
using Orbio.Services.Admin.Customers;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using Orbio.Web.UI.Areas.Admin.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orbio.Services.Helpers;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
       #region Fields

        private readonly ICustomerReportService _customerReportService;
        private readonly ICustomerRoleService customerRoleService;
        private readonly ICustomerService customerService;
        private readonly IOnlineCustomerService onlineService;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Ctor

        public CustomerController(ICustomerReportService customerReportService, ICustomerRoleService customerRoleService,
            ICustomerService customerService, IOnlineCustomerService onlineService, IDateTimeHelper _dateTimeHelper)
        {
            this._customerReportService = customerReportService;
            this.customerRoleService = customerRoleService;
            this.customerService = customerService;
            this.onlineService = onlineService;
            this._dateTimeHelper = _dateTimeHelper;
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

        #region Customers

        public ActionResult ListCustomer()
        {
            var result = customerRoleService.GetAllCustomerRole();
            var model = new CustomerListModel();
            model.CustomerRoles=  (from CR in result
                                select new CustomerRoleModel(CR)).ToList();
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult CustomerList(CustomerListModel model)
        {
            var result = customerService.GetAllCustomer(model.FirstName, model.LastName, model.Email, model.Roles);
            var customer = (from cust in result
                            select new CustomerModel(cust)).ToList();
            return PartialView(customer);
        }

        public ActionResult OnlineCustomers()
        {
            var result = onlineService.GetOnlineCustomers();
            var model = (from cust in result
                         select new CustomerModel
                         {
                             Id=cust.Id,
                             Email=(!string.IsNullOrEmpty(cust.Email)?cust.Email:"Guest"),
                             IPAddress=cust.LastIpAddress,
                             LastActivity=cust.LastActivityDateUtc.ToLocalTime(),
                         }).ToList();

            return View("ListOnlineCustomers",model);
        }

        public ActionResult CustomerReport()
        {
            return View();
        }

        public ActionResult SearchReport()
        {
            //order statuses
            var model = new OrderListModel();
            model.AvailableOrderStatuses = OrderStatus.Pending.ToSelectList(false).ToList();
            model.AvailableOrderStatuses.Insert(0, new SelectListItem() { Text = "Order Status", Value = "0" });

            //payment statuses
            model.AvailablePaymentStatuses = PaymentStatus.Pending.ToSelectList(false).ToList();
            model.AvailablePaymentStatuses.Insert(0, new SelectListItem() { Text = "Payment Status", Value = "0" });

            //shipping statuses
            model.AvailableShippingStatuses = ShippingStatus.NotYetShipped.ToSelectList(false).ToList();
            model.AvailableShippingStatuses.Insert(0, new SelectListItem() { Text = "Shipping Status", Value = "0" });
            return PartialView("CustomerReportSearch", model);
        }

        [ChildActionOnly]
        public ActionResult TopCustomers(OrderListModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                          : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            PaymentStatus? paymentStatus = model.PaymentStatusId > 0 ? (PaymentStatus?)(model.PaymentStatusId) : null;
            ShippingStatus? shippingStatus = model.ShippingStatusId > 0 ? (ShippingStatus?)(model.ShippingStatusId) : null;

            var result = _customerReportService.SearchCustomerReport(startDateValue, endDateValue, orderStatus,
                paymentStatus, shippingStatus);
            var topCustomers = (from cr in result
                         select new RegisteredCustomerReportLineModel
                         {
                             Customers = cr.CustomerId,
                             Email = cr.Email,
                             OrderCount = cr.OrderCount,
                             OrderTotal = cr.OrderTotal,
                         }).ToList();

            return PartialView(topCustomers);
        }

        #endregion
    }
}