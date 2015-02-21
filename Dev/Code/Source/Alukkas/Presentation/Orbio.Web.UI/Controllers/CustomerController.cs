using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Orbio.Web.UI.Filters;
using Orbio.Web.UI.Models.Home;
using Orbio.Web.UI.Models.Customer;
using Orbio.Services.Customers;
using Orbio.Core.Domain.Customers;

namespace Orbio.Web.UI.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [LoginRequiredAttribute]
        public ActionResult MyAccount(string returnUrl)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            var model = new CustomerModel(curcustomer);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult MyAccount(CustomerModel model, string returnUrl)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            var infomodel = Customerinfo(model, curcustomer);
            ViewBag.ReturnUrl = returnUrl;
            return View(infomodel);
        }

        public CustomerModel Customerinfo(CustomerModel model, Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerService.getcustomerdetails("Update", customer.Id, model.FirstName, model.LastName, model.Gender, model.DOB, model.Email, model.Mobile);
            }
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Gender = model.Gender;
            customer.DOB = model.DOB;
            customer.Email = model.Email;
            customer.MobileNo = model.Mobile;
            return model;
        }

    }
}