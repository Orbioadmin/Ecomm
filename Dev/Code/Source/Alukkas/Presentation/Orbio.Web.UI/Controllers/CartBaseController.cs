using Nop.Core.Infrastructure;
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
        private readonly IShoppingCartService shoppingCartService;

        public CartBaseController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        protected CartModel PrepareShoppingCartItemModel()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, curCustomer.Id, 0, 0));

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
            var model = new CartModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, customerId, 0, 0));

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