using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Checkout;
using Orbio.Services.Common;
using Orbio.Services.Orders;
using Orbio.Services.Payments;
using Orbio.Web.UI.Filters;
using Orbio.Web.UI.Models.CheckOut;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Web.Routing;
using Orbio.Web.UI.Models.Orders;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Payments;
namespace Orbio.Web.UI.Controllers
{
    public class CheckOutController : CartBaseController
    {
        //private readonly IShoppingCartService shoppingCartService;
        
        private readonly ICheckoutService checkoutService;
        private readonly IOrderService orderService;
        private readonly IPriceCalculationService priceCalculationService;
        private readonly ITransientCartService transientCartService;
         //
        // GET: /CheckOut/
        public CheckOutController(ICheckoutService checkoutService, IShoppingCartService shoppingCartService, IPriceCalculationService priceCalculationService,
            IWorkContext workContext, IStoreContext storeContext, ITransientCartService transientCartService, IOrderService orderService)
            : base(shoppingCartService, workContext, storeContext, EngineContext.Current.Resolve<IGenericAttributeService>())
        {
            this.checkoutService = checkoutService;
           // this.shoppingCartService = shoppingCartService;
            this.priceCalculationService = priceCalculationService;
            this.transientCartService = transientCartService;
            this.orderService = orderService;
        }

        [LoginRequired]
        public ActionResult Index()
        {
            //var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            //var curcustomer = workContext.CurrentCustomer;
            //ShoppingCartType carttype = ShoppingCartType.ShoppingCart;
           // PrepareShoppingCartItemModel();
            //var address = new AddressModel();
            if (TempData.ContainsKey("ThankYou"))
            {
                ViewBag.ThankYou = TempData["ThankYou"];
                TempData.Remove("ThankYou");
            }
            else
            {
                ViewBag.ThankYou = false;
            }
            return View();

        }

         [LoginRequired]
        public ActionResult Address()
        {
            if (TempData.ContainsKey("ThankYou"))
            {
                ViewBag.ThankYou = TempData["ThankYou"];
                TempData.Remove("ThankYou");
            }
            else
            {
                ViewBag.ThankYou = false;
            }
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var customer = workContext.CurrentCustomer;
            //ShoppingCartType carttype = ShoppingCartType.ShoppingCart;
            //PrepareShoppingCartItemModel(customer.Id);
            if (customer.Email == null)
            {
                return RedirectToAction("MyAccount", "Login");
            }
            else
            {
                var customerBillAddress = checkoutService.GetCustomerAddress(customer.Email, "Billing");
                var customerShipAddress = checkoutService.GetCustomerAddress(customer.Email, "Shipping");
                if (customerBillAddress == null && customerShipAddress==null)
                {
                    return RedirectToAction("Index", "CheckOut");
                }
                else
                {
                    var address = new AddressModel();
                    if (customerBillAddress.BillingAddress_Id == customerBillAddress.ShippingAddress_Id)
                    {
                        address = new AddressModel(customerBillAddress);
                    }
                    else
                    {
                        if(customerShipAddress==null)
                        {
                            address = new AddressModel(customerBillAddress, "Billing");
                          
                        }
                        else if(customerBillAddress==null)
                        {
                           address = new AddressModel(customerBillAddress, "Shipping");
                        }
                        else
                        {
                            address = new AddressModel(customerBillAddress, customerShipAddress);
                        }
                    }
                    return PartialView("_address", address);
                }
            }
        }
       
        [HttpPost]
        public ActionResult CheckboxDetails(AddressModel model)
        {
           if(model.SameAddress)
           {
               model.BillFirstName = model.ShipFirstName;
               model.BillLastName = model.ShipLastName;
               model.BillPhone = model.ShipPhone;
               model.BillAddress = model.ShipAddress;
               model.BillCity = model.ShipCity;
               model.BillPincode = model.ShipPincode;
               model.BillState = model.ShipState;
               model.BillCountry = model.ShipCountry;
               return Json(new { model });
           }
           else
           {
               var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
               var customer = workContext.CurrentCustomer;
               var customerBillAddress = checkoutService.GetCustomerAddress(customer.Email, "Billing");
               if (customerBillAddress == null)
               {
                   model.BillFirstName = null;
                   model.BillLastName = null;
                   model.BillPhone = null;
                   model.BillAddress = null;
                   model.BillCity = null;
                   model.BillPincode = null;
                   model.BillState = null;
                   model.BillCountry = null;
               }
               else
               {
                   var address = new AddressModel(customerBillAddress, "Billing");
                   model.BillFirstName = address.BillFirstName;
                   model.BillLastName = address.BillLastName;
                   model.BillPhone = address.BillPhone;
                   model.BillAddress = address.BillAddress;
                   model.BillCity = address.BillCity;
                   model.BillPincode = address.BillPincode;
                   model.BillState = address.BillState;
                   model.BillCountry = address.BillCountry;
               }

               return Json(new { model });
           }
        }

        [HttpPost]
        [LoginRequired]
        public ActionResult UpdateCustomer(AddressModel model)
        {
            bool sameaddress=false;
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var customer = workContext.CurrentCustomer;
            if (model.SameAddress)
            {
                sameaddress = true;
            }
            checkoutService.UpdateCustomerAddress(customer.Email, sameaddress, model.BillFirstName, model.BillLastName, model.BillPhone, model.BillAddress,
            model.BillCity,model.BillPincode, model.BillState, model.BillCountry, model.ShipFirstName, model.ShipLastName,
            model.ShipPhone, model.ShipAddress, model.ShipCity, model.ShipPincode,model.ShipState, model.ShipCountry);

            return Json("Success");
        }

        

        public ActionResult SubmitOrder(int transientCartId)
        {
            var customer = workContext.CurrentCustomer;
            var cart = transientCartService.GetTransientCart(transientCartId, customer.Id);
            var processRequest = new ProcessOrderRequest()
            {
                StoreId = storeContext.CurrentStore.Id,
                CustomerId = workContext.CurrentCustomer.Id
            };
            processRequest.PaymentMethodSystemName = "Payments.CashOnDelivery";
            processRequest.NewPaymentStatus = PaymentStatus.Pending; //Hardcoding it for now assuming only COD
            processRequest.Cart = cart.ConvertToCartModel();
            //do payment and set Success = true
            processRequest.Success = true;             
            var result = orderService.PlaceOrder(processRequest);
            if (!TempData.ContainsKey("ThankYou"))
            {
                TempData.Add("ThankYou", true);
            }
            return RedirectToAction("Index", "CheckOut");
        }

        public ActionResult PayOrder()
        {
            
            var cartModel = PrepareShoppingCartItemModel();
            var transientCartId = transientCartService.UpdateTransientCart(0, workContext.CurrentCustomer.Id, new TransientCart
            {
                Discounts = cartModel.Discounts,
                ShoppingCartItems = (from sci in cartModel.ShoppingCartItems 
                                         select new TransientCartItem(sci)).ToList()
            });

            return RedirectToAction("SubmitOrder", "CheckOut", new RouteValueDictionary { { "transientCartId" , transientCartId} });
        }

        //private void PrepareShoppingCartItemModel(int customerId, ShoppingCartType cartType)
        //{
        //    var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType,0, customerId, 0, 0));
           
        //    var currency = (from r in model.ShoppingCartItems.AsEnumerable()
        //                    select r.CurrencyCode).Take(1).ToList();
        //    ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";
        //}
    }
}