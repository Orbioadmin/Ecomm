using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Orders;
using Orbio.Web.UI.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Controllers
{
    public class CartBaseController : Controller
    {
        protected readonly IShoppingCartService shoppingCartService;
        protected readonly IWorkContext workContext;
        protected readonly IStoreContext storeContext;

        public CartBaseController(IShoppingCartService shoppingCartService, IWorkContext workContext, IStoreContext storeContext)
        {
            this.shoppingCartService = shoppingCartService;
            this.workContext = workContext;
            this.storeContext = storeContext;
        }

        protected CartModel PrepareShoppingCartItemModel()
        {
            
            var curCustomer = workContext.CurrentCustomer;
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, curCustomer.Id, 0, 0, storeContext.CurrentStore.Id));

            // decimal subtotal = priceCalculationService.GetCartSubTotal(model,false);

            //foreach (var totalprice in model.ShoppingCartItems)
            //{
            //    subtotal = subtotal + Convert.ToDouble(totalprice.TotalPrice);
            //}
            // ViewBag.subtotal = subtotal.ToString("#,##0.00");
            //ViewBag.DiscountAmount = priceCalculationService.GetAllDiscountAmount(model).ToString("#,##0.00");
            // ViewBag.CartTotal = priceCalculationService.GetCartSubTotal(model, true).ToString("#,##0.00");
            var currency = (from r in model.ShoppingCartItems.AsEnumerable()
                            select r.CurrencyCode).Take(1).ToList();
            ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";

            return model;
        }

        protected CartModel PrepareShoppingCartItemModel(int customerId)
        {
           
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, customerId, 0, 0, storeContext.CurrentStore.Id));

            // decimal subtotal = priceCalculationService.GetCartSubTotal(model,false);

            //foreach (var totalprice in model.ShoppingCartItems)
            //{
            //    subtotal = subtotal + Convert.ToDouble(totalprice.TotalPrice);
            //}
            // ViewBag.subtotal = subtotal.ToString("#,##0.00");
            //ViewBag.DiscountAmount = priceCalculationService.GetAllDiscountAmount(model).ToString("#,##0.00");
            // ViewBag.CartTotal = priceCalculationService.GetCartSubTotal(model, true).ToString("#,##0.00");
            var currency = (from r in model.ShoppingCartItems.AsEnumerable()
                            select r.CurrencyCode).Take(1).ToList();
            ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";

            return model;
        }
    }
}