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
using Orbio.Core;
using PagedList;
using System.Configuration;

namespace Orbio.Web.UI.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerService customerService;
        private readonly IMessageService messageService;
        private readonly IProductService productService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IOrderService orderService;
        private readonly IStoreContext storeContext;

        public CustomerController(ICustomerService customerService, IMessageService messageService, IProductService productService
            , IShoppingCartService shoppingCartService, IOrderService orderService, IStoreContext storeContext)
        {
            this.customerService = customerService;
            this.messageService = messageService;
            this.productService = productService;
            this.shoppingCartService = shoppingCartService;
            this.orderService = orderService;
            this.storeContext = storeContext;
        }

        [LoginRequiredAttribute]
        public ActionResult MyAccount(string returnUrl)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var model = new CustomerModel(curCustomer);
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult MyAccount(CustomerModel model)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var infomodel = Customerinfo(model, curCustomer);
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
        public ActionResult ChangePassword()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var model = new CustomerModel(curCustomer);
            return View("~/Views/Customer/MyAccount.cshtml", model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            if (ModelState.IsValid)
            {
                var changePasswordRequest = new ChangePasswordRequest(curCustomer.Email,
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

                                var changePasswordResult = customerService.ChangePassword(curCustomer.Id, changePasswordRequest.NewPassword, (int)PasswordFormat.Hashed);
                                switch (changePasswordResult)
                                {
                                    case ChangePasswordResult.Successful:
                                        {
                                            var model1 = new CustomerModel(curCustomer);
                                            model1.Result = "Your Password has been successfully changed";
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
            var acmodel = new CustomerModel(curCustomer);
            return View("~/Views/Customer/MyAccount.cshtml", acmodel);
        }


        public ActionResult PasswordRecoveryConfirm(string token, string email)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var model = new CustomerModel(curCustomer);
            ViewBag.token = token;
            ViewBag.email = email;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult PasswordRecoveryConfirm(string token, string email, PasswordRecoveryConfirmModel model)
        {
            var curCustomer = new Customer();

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
                    var loginResult = customerService.GetCustomerDetailsByEmail(email, ref curCustomer);
                    switch (loginResult)
                    {
                        case CustomerLoginResults.Successful:
                            {

                                var changePasswordResult = customerService.ChangePassword(curCustomer.Id, changePasswordRequest.NewPassword, (int)PasswordFormat.Hashed);
                                switch (changePasswordResult)
                                {
                                    case ChangePasswordResult.Successful:
                                        {
                                            var model1 = new CustomerModel(curCustomer);
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
            var acmodel = new CustomerModel(curCustomer);
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
                            messageService.SendCustomerPasswordRecoveryMessage(customer);
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
        public ActionResult EmailFriend(EmailFriendModel model, string seName, string name)
        {
            ModelState.Clear();
            var uri = Request.Url;
            string host = uri.GetLeftPart(UriPartial.Authority);
            string code = GenerateCaptcha();
            model.SeName = seName;
            model.Name = name;
            model.CaptchaCode = code;
            model.url = host + "/" + seName + "?p=pt";
            return View(model);
        }


        [HttpPost]
        public ActionResult EmailFriend(EmailFriendModel model, string seName, string name, string url, string captchaCode)
        {
            if (ModelState.IsValid)
            {
                if (captchaCode == model.Captcha)
                {
                    var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                    if (workContext.CurrentCustomer.IsRegistered)
                    {
                        var customer = workContext.CurrentCustomer;
                        var product = new ProductDetail();
                        product = productService.GetProductsDetailsBySlug(seName);
                        int mailresult = messageService.SendCustomerEmailFrendMessage(customer, product, model.Email, model.Message, name, url);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only registered customers can sent email");
                    }
                    return RedirectToRoute("Category", new { p = "pt", seName = seName });
                }
                else
                {
                    model.Captcha = "";
                    model.CaptchaCode = captchaCode;
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
            return View("WishList");
        }

        public ActionResult WishListSummary(int? page)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            ShoppingCartType cartType = ShoppingCartType.Wishlist;
            var model = PrepareShoppingCartItemModel(curCustomer.Id, cartType);
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["CatelogProductsPageSize"]);
            return PartialView(model.ShoppingCartItems.ToPagedList(pageNumber,pageSize));
        }

        private CartModel PrepareShoppingCartItemModel(int customerId, ShoppingCartType cartType)
        {
            var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, customerId, 0, 0, storeContext.CurrentStore.Id));
            return model;
        }

        public ActionResult Order()
        {
            return View("Order");
        }

        public ActionResult OrderDetails(int? page)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var model = new OrderDetailsModel(orderService.GetOrderDetails(curCustomer.Id));
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["CatelogProductsPageSize"]);
            return PartialView(model.OrderedProductDetail.ToPagedList(pageNumber, pageSize));
        }
        
        [HttpGet]
        public ActionResult UpdateWishList(int itemId, string value)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;

            if (value == "addtocart")
            {
                string result = shoppingCartService.UpdateWishListItems(value, itemId, ShoppingCartType.ShoppingCart, 0, 0, 0, 0,ShoppingCartStatus.Cart);
                if (result == "ShoppingCart")
                    return RedirectToRoute("ShoppingCart");
                else
                    return RedirectToRoute("Category", new { p = "pt", seName = result });
            }
            else
            {
                shoppingCartService.UpdateWishListItems(value, itemId, ShoppingCartType.Wishlist, 0, 0, 0, 0,ShoppingCartStatus.WishList);
                return RedirectToAction("MyAccount", "Customer", new { wish = "#mywishlist" });
            }
        }

        /*Add product to wishlist from list page*/
        [HttpPost]
        public ActionResult AddItemToWishList(int id)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            string result = shoppingCartService.UpdateWishListItems("addWishList", 0, ShoppingCartType.Wishlist, 0, curCustomer.Id, id, 1,ShoppingCartStatus.WishList);
            if (result == "updated" || result == "inserted")
            {
                return Json("Success");
            }
            else
                return Json(result);
        }
    }
}