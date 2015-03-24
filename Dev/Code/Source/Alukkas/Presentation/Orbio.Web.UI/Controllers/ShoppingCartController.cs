using Nop.Core.Infrastructure;
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

namespace Orbio.Web.UI.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IShoppingCartService shoppingcartservice;

        public ShoppingCartController(IShoppingCartService shoppingcartservice)
        {

            this.shoppingcartservice = shoppingcartservice;
        }

        public ActionResult Cart()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            ShoppingCartType carttype = ShoppingCartType.ShoppingCart;
            var model = PrepareShoppingCartItemModel(curcustomer.Id, Convert.ToInt32(carttype));
            double subtotal = 0.00;
            foreach (var totalprice in model.CartDetail)
            {
                subtotal = subtotal + Convert.ToDouble(totalprice.Totalprice);
            }
            ViewBag.subtotal = subtotal.ToString("0.00");
            var currency = (from r in model.CartDetail.AsEnumerable()
                            select r.CurrencyCode).Take(1).ToList();
            ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";
            return View(model);
        }
        [HttpPost]
        public ActionResult Cart(ShoppingCartItemModel detailmodel)
        {

            string xml = Serializer.GenericDataContractSerializer(detailmodel.items);
            shoppingcartservice.ModifyCartItem(xml);
            return RedirectToRoute(new { seName = "cart"});
        }
        public ActionResult CartItem()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            ShoppingCartType carttype = ShoppingCartType.ShoppingCart;
            var model = PrepareShoppingCartItemModel(curcustomer.Id, Convert.ToInt32(carttype));
            return PartialView("CartItems",model);
        }
        private ShoppingCartItemsModel PrepareShoppingCartItemModel(int customerid, int carttype)
        {
            var model = new ShoppingCartItemsModel(shoppingcartservice.GetCartItems("select",0, carttype, customerid, 0, 0));

            return model;
        }
    }
}
