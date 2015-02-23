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
        private readonly CustomerSettings _customerSettings;

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
                customerService.GetCustomerDetails("Update", customer.Id, model.FirstName, model.LastName, model.Gender, model.DOB, model.Email, model.Mobile);
            }
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Gender = model.Gender;
            customer.DOB = model.DOB;
            customer.Email = model.Email;
            customer.MobileNo = model.Mobile;
            return model;
        }

        [LoginRequiredAttribute]
        public ActionResult ChangePassword(string returnUrl)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            var model = new CustomerModel(curcustomer);
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Customer/MyAccount.cshtml", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model, string returnUrl)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;

            if (ModelState.IsValid)
            {
                var changePasswordRequest = new ChangePasswordRequest(curcustomer.Email,
                    true, PasswordFormat.Hashed, model.NewPassword, model.OldPassword);

                if (changePasswordRequest == null)
                    throw new ArgumentNullException("request");

                if (String.IsNullOrWhiteSpace(changePasswordRequest.Email))
                {
                    ModelState.AddModelError("", "Email Is Not Provided");
                }
                else if (String.IsNullOrWhiteSpace(changePasswordRequest.NewPassword))
                {
                    ModelState.AddModelError("", "Password Is Not Provided");
                }
                else
                {
                    var loginResult = customerService.GetCustomerDetailsByEmail(changePasswordRequest.Email, changePasswordRequest.OldPassword);
                    switch (loginResult)
                    {
                        case CustomerLoginResults.Successful:
                            {

                                var changePasswordResult = customerService.ChangePassword(curcustomer.Id, changePasswordRequest.NewPassword, (int)PasswordFormat.Hashed);
                                switch (changePasswordResult)
                                {
                                    case ChangePasswordResult.Successful:
                                        {
                                            var model1 = new CustomerModel(curcustomer);
                                            model1.Result = "Your Password has been successfully changed";
                                            ViewBag.ReturnUrl = returnUrl;
                                            return View("~/Views/Customer/MyAccount.cshtml", model1);
                                            //return RedirectToLocal(returnUrl);
                                        }
                                }
                                break;
                            }
                        case CustomerLoginResults.CustomerNotExist:
                            ModelState.AddModelError("", "No customer account found");
                            break;
                        case CustomerLoginResults.Deleted:
                            ModelState.AddModelError("", "Customer is deleted");
                            break;
                        case CustomerLoginResults.NotActive:
                            ModelState.AddModelError("", "Account is not active");
                            break;
                        case CustomerLoginResults.NotRegistered:
                            ModelState.AddModelError("", "Account is not registered");
                            break;
                        case CustomerLoginResults.WrongPassword:
                        default:
                            ModelState.AddModelError("", "Old Password Doesnt Match");
                            break;
                    }
                }

            }
            var acmodel = new CustomerModel(curcustomer);
            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Customer/MyAccount.cshtml", acmodel);
        }
    }
}