using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Orders;
using Orbio.Services.Utility;
using Orbio.Web.UI.Models.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Orbio.Services.Common;
using Orbio.Web.UI.Filters;

namespace Orbio.Web.UI.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IShoppingCartService shoppingCartService;
        private readonly IStoreContext storeContext;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IStoreContext storeContext)
        {

            this.shoppingCartService = shoppingCartService;
            this.storeContext = storeContext;
        }

        [ContinueShoppingAttribute]
        public ActionResult Cart()
        {
           
            var model = PrepareShoppingCartItemModel();

            return View(model);
        }
        [HttpPost]
        public ActionResult Cart(ShoppingCartItemModel detailModel)
        {
            List<ShoppingCartItem> cartUpdateItems = new List<ShoppingCartItem>();
            cartUpdateItems = (from r in detailModel.items.AsEnumerable()
                               select new ShoppingCartItem { CartId=r.CartId,Quantity=Convert.ToInt32(r.SelectedQuantity),IsRemove=r.IsRemove}).ToList();
            shoppingCartService.ModifyCartItem(cartUpdateItems);
            
            return RedirectToRoute("ShoppingCart");
        }
        public ActionResult CartItem()
        {
            var model = PrepareShoppingCartItemModel();
            return PartialView("CartItems",model);
        }

        public ActionResult Continueshopping()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            string returnUrl = workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.LastContinueShoppingPage, storeContext.CurrentStore.Id);
            return RedirectToLocal(returnUrl);
        }

        private ShoppingCartItemsModel PrepareShoppingCartItemModel()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = new ShoppingCartItemsModel(shoppingCartService.GetCartItems("select", 0, cartType, 0, curCustomer.Id, 0, 0));
            double subtotal = 0.00;
            foreach (var totalprice in model.CartDetail)
            {
                subtotal = subtotal + Convert.ToDouble(totalprice.TotalPrice);
            }
            ViewBag.subtotal = subtotal.ToString("#,##0.00");
            var currency = (from r in model.CartDetail.AsEnumerable()
                            select r.CurrencyCode).Take(1).ToList();
            ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";
            
            return model;
        }

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
        public ActionResult CartSummary()
        {
            var model = PrepareShoppingCartItemModel();
            return PartialView(model);
        }

    }
}
