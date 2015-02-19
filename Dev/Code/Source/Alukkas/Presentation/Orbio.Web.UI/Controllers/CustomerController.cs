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
            var infomodel = customerinfo(model, curcustomer);
            ViewBag.ReturnUrl = returnUrl;
            return View(infomodel);
        }

        public CustomerModel customerinfo(CustomerModel model, Customer Customer)
        {
            if (ModelState.IsValid)
            {
                var loginResult = customerService.Customerdetails("Update",Customer.Id,model.FirstName,model.LastName,model.Gender,model.DOB,model.Email,model.Mobile);
            }
            Customer.FirstName = model.FirstName;
            Customer.LastName = model.LastName;
            Customer.Gender = model.Gender;
            Customer.DOB = model.DOB;
            Customer.Email = model.Email;
            Customer.MobileNo = model.Mobile;
            return model;
        }

    }
}