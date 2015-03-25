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
using Orbio.Services.Messages;
using Orbio.Web.UI.Models.Catalog;
using Orbio.Services.Catalog;
using Orbio.Core.Domain.Catalog;
using System.Text;
using Orbio.Core.Domain.Orders;
using Orbio.Web.UI.Models.Orders;
using Orbio.Services.Orders;
using System.Threading.Tasks;

namespace Orbio.Web.UI.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerService customerService;
        private readonly IMessageService MessageService;
        private readonly IProductService productService;
        private readonly IShoppingCartService shoppingcartservice;

        public CustomerController(ICustomerService customerService, IMessageService MessageService , IProductService productService
            , IShoppingCartService shoppingcartservice)
        {
            this.customerService = customerService;
            this.MessageService = MessageService;
            this.productService = productService;
            this.shoppingcartservice = shoppingcartservice;
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
        //[ValidateAntiForgeryToken]
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

                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Gender = model.Gender;
                customer.DOB = model.DOB;
                customer.Email = model.Email;
                customer.MobileNo = model.Mobile;
            }
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
        //[ValidateAntiForgeryToken]
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


        public ActionResult PasswordRecoveryConfirm(string token, string email, string returnUrl)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            var model = new CustomerModel(curcustomer);
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.token = token;
            ViewBag.email = email;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult PasswordRecoveryConfirm(string token, string email, PasswordRecoveryConfirmModel model)
        {
            var curcustomer = new Customer();

            if (ModelState.IsValid)
            {
                var changePasswordRequest = new ChangePasswordRequest(email,
                    true, PasswordFormat.Hashed, model.NewPassword);

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
                    var loginResult = customerService.GetCustomerDetailsByEmail(email, ref curcustomer);
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
                                            model1.NewPassword = "";
                                            model1.ConfirmNewPassword = "";
                                            return View(model1);
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
            return View(acmodel);
        }

        public ActionResult PasswordRecovery()
        {
            var model = new PasswordRecoveryModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult PasswordRecovery(PasswordRecoveryModel model)
        {
            var customer = new Customer();
            if (ModelState.IsValid)
            {

                var loginResult = customerService.GetCustomerDetailsByEmail(model.Email, ref customer);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            MessageService.SendCustomerPasswordRecoveryMessage(customer);
                            model.Result = "Email with instructions has been sent to you.";
                            return View(model);
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

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [LoginRequired]
        public ActionResult EmailFriend(EmailFriendModel model,string SeName, string Name)
        {
            ModelState.Clear();
            var uri = Request.Url;
            string host = uri.GetLeftPart(UriPartial.Authority);
            string code = GenerateCaptcha();
            model.SeName = SeName;
            model.Name = Name;
            model.CaptchaCode = code;
            model.url = host + "/" + SeName+"?p=pt";
            return View(model);
        }

        
        [HttpPost]
        public ActionResult EmailFriend(EmailFriendModel model, string SeName, string Name, string Url, string CaptchaCode)
        {
            if (ModelState.IsValid)
            {
                if (CaptchaCode == model.Captcha)
                {
                    var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                    if (workContext.CurrentCustomer.IsRegistered)
                    {
                        var customer = workContext.CurrentCustomer;
                        var product = new ProductDetail();
                        product = productService.GetProductsDetailsBySlug(SeName);
                        int mailresult = MessageService.SendCustomerEmailFrendMessage(customer, product, model.Email, model.Message, Name, Url);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only registered customers can sent email");
                    }
                    return RedirectToRoute("Category", new { p = "pt", seName = SeName });
                }
                else
                {
                    model.Captcha = "";
                    model.CaptchaCode = CaptchaCode;
                    ModelState.AddModelError("", "Captcha code does not match");
                    return View(model);
                }
            }
            else
            {
                string code = GenerateCaptcha();
                model.CaptchaCode = code;
                return View(model);
            }
          
        }

        public string GenerateCaptcha()
        {
            Random random = new Random();
            string combination = "0123456789";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 4; i++)
            captcha.Append(combination[random.Next(combination.Length)]);
            string result = captcha.ToString();
            return result;
        }

        public ActionResult WishList()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            ShoppingCartType carttype = ShoppingCartType.Wishlist;
            var model = PrepareShoppingCartItemModel(curcustomer.Id, Convert.ToInt32(carttype));
            return View("WishList", model);
        }
        private ShoppingCartItemsModel PrepareShoppingCartItemModel(int customerid, int carttype)
        {
                var model = new ShoppingCartItemsModel(shoppingcartservice.GetCartItems("select", 0, carttype, customerid, 0, 0));
                return model;
        }
        [HttpGet]
        public async Task<ActionResult> UpdateWishList(int Id, string value)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            ShoppingCartType carttype = ShoppingCartType.Wishlist;
            DeleteOrUpdateWishList(Id, Convert.ToInt32(carttype), value);
            var model = PrepareShoppingCartItemModel(curcustomer.Id, Convert.ToInt32(carttype));
            return PartialView("WishListSummary", model.CartDetail);
        }

        private void DeleteOrUpdateWishList(int Id, int carttype, string value)
        {
            shoppingcartservice.GetCartItems(value, Id, carttype, 0, 0, 0);
        }
    }
}