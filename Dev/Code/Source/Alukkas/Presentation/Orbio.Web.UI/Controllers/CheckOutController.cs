using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Services.Checkout;
using Orbio.Services.Common;
using Orbio.Services.Orders;
using Orbio.Web.UI.Filters;
using Orbio.Web.UI.Models.CheckOut;
using System.Web.Mvc;

namespace Orbio.Web.UI.Controllers
{
    public class CheckOutController : CartBaseController
    {
        //private readonly IShoppingCartService shoppingCartService;
        
        private readonly ICheckoutService checkoutService;

        private readonly IPriceCalculationService priceCalculationService;
        //
        // GET: /CheckOut/
        public CheckOutController(ICheckoutService checkoutService, IShoppingCartService shoppingCartService, IPriceCalculationService priceCalculationService,
            IWorkContext workContext, IStoreContext storeContext)
            : base(shoppingCartService, workContext, storeContext, EngineContext.Current.Resolve<IGenericAttributeService>())
        {
            this.checkoutService = checkoutService;
           // this.shoppingCartService = shoppingCartService;
            this.priceCalculationService = priceCalculationService;
        }

        [LoginRequired]
        public ActionResult Index()
        {
            //var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            //var curcustomer = workContext.CurrentCustomer;
            //ShoppingCartType carttype = ShoppingCartType.ShoppingCart;
            PrepareShoppingCartItemModel();
            var address = new AddressModel();
            return View(address);

        }

         [LoginRequired]
        public ActionResult Address()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var customer = workContext.CurrentCustomer;
            //ShoppingCartType carttype = ShoppingCartType.ShoppingCart;
            PrepareShoppingCartItemModel(customer.Id);
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
                    return View("~/Views/CheckOut/Index.cshtml", address);
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

        //private void PrepareShoppingCartItemModel(int customerId, ShoppingCartType cartType)
        //{
        //    var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType,0, customerId, 0, 0));
           
        //    var currency = (from r in model.ShoppingCartItems.AsEnumerable()
        //                    select r.CurrencyCode).Take(1).ToList();
        //    ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";
        //}
    }
}