using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Common;
using Orbio.Services.Orders;
using Orbio.Web.UI.Filters;
using Orbio.Web.UI.Models.Orders;

namespace Orbio.Web.UI.Controllers
{
    public class ShoppingCartController : CartBaseController
    {

   
       
        private readonly IPriceCalculationService priceCalculationService;
       

        public ShoppingCartController(IShoppingCartService shoppingCartService, IStoreContext storeContext,
            IPriceCalculationService priceCalculationService, IGenericAttributeService genericAttributeService, IWorkContext workContext)
            : base(shoppingCartService, workContext, storeContext, genericAttributeService)
        {
                        
            this.priceCalculationService = priceCalculationService;
        }

        [ContinueShoppingAttribute]
        public ActionResult Cart()
        {
            var model = PrepareShoppingCartItemModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Cart(CartModel detailModel, FormCollection formCollection)
        {
            if (formCollection.AllKeys.Contains("removeCoupon"))
            {
                genericAttributeService.DeleteGenericAttribute(workContext.CurrentCustomer.Id, "Customer", SystemCustomerAttributeNames.DiscountCouponCode, string.Empty,
                    storeContext.CurrentStore.Id);
            }
            else
            {
                List<ShoppingCartItem> cartUpdateItems = new List<ShoppingCartItem>();
                if (!String.IsNullOrWhiteSpace(detailModel.AppliedCouponCode))
                {
                    genericAttributeService.SaveGenericAttributes(workContext.CurrentCustomer.Id, "Customer", SystemCustomerAttributeNames.DiscountCouponCode,
                            detailModel.AppliedCouponCode, storeContext.CurrentStore.Id);
                }
                cartUpdateItems = (from r in detailModel.ShoppingCartItems.AsEnumerable()
                                   select new ShoppingCartItem { CartId = r.CartId, Quantity = Convert.ToInt32(r.SelectedQuantity), IsRemove = r.IsRemove }).ToList();
                shoppingCartService.ModifyCartItem(cartUpdateItems);
            }
            
            return RedirectToRoute("ShoppingCart");
        }
        public ActionResult CartItem()
        {
            var model = new CartHeaderModel{ItemCount=shoppingCartService.GetCartItems("select", 0, ShoppingCartType.ShoppingCart,
                0, workContext.CurrentCustomer.Id, 0, 0, storeContext.CurrentStore.Id).ShoppingCartItems.Count};
            return PartialView("CartItems",model);
        }

        public ActionResult Continueshopping()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            string returnUrl = workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.LastContinueShoppingPage, storeContext.CurrentStore.Id);
            return RedirectToLocal(returnUrl);
        }

        //private CartModel PrepareShoppingCartItemModel()
        //{
        //    var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
        //    var curCustomer = workContext.CurrentCustomer;
        //    ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
        //    var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, curCustomer.Id, 0, 0));

        //   // decimal subtotal = priceCalculationService.GetCartSubTotal(model,false);
           
        //    //foreach (var totalprice in model.ShoppingCartItems)
        //    //{
        //    //    subtotal = subtotal + Convert.ToDouble(totalprice.TotalPrice);
        //    //}
        //   // ViewBag.subtotal = subtotal.ToString("#,##0.00");
        //    //ViewBag.DiscountAmount = priceCalculationService.GetAllDiscountAmount(model).ToString("#,##0.00");
        //   // ViewBag.CartTotal = priceCalculationService.GetCartSubTotal(model, true).ToString("#,##0.00");
        //    var currency = (from r in model.ShoppingCartItems.AsEnumerable()
        //                    select r.CurrencyCode).Take(1).ToList();
        //    ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";
            
        //    return model;
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [ChildActionOnly]
        public ActionResult CartSummary(bool? showCartSummary)
        {
            var model = PrepareShoppingCartItemModel();
            ViewBag.ShowCartSummary = true;
            if (showCartSummary.HasValue)
            {
                ViewBag.ShowCartSummary = showCartSummary.Value;
            }
            return PartialView(model);
        }


        /*Add product to cart from list page*/
        [HttpPost]
        public ActionResult AddItemToCart(int id)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            string result = shoppingCartService.UpdateWishListItems("addCartItem",0, ShoppingCartType.ShoppingCart, 0, curCustomer.Id, id, 1);
            if (result == "Updated" || result == "Inserted")
                {
                    return Json("Success");
                }
                else
                    return Json(result);
            }
        }

}
