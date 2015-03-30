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

        private readonly IShoppingCartService ShoppingCartService;

        public ShoppingCartController(IShoppingCartService ShoppingCartService)
        {

            this.ShoppingCartService = ShoppingCartService;
        }

        public ActionResult Cart()
        {
            if (Request.RawUrl.ToString() != "/")
            {
                string Currenturl = Request.Url.Scheme + "://" + Request.Url.Authority + "/cart";
                if (Request.UrlReferrer != null)
                {
                    if (Request.UrlReferrer.ToString() != Currenturl)
                    {
                        HttpCookie myCookie = new HttpCookie("Returnurl");
                        DateTime now = DateTime.Now;
                        myCookie.Value = Request.UrlReferrer.ToString();

                        myCookie.Expires = now.AddHours(2);

                        Response.Cookies.Add(myCookie);
                    }
                }
            }
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = PrepareShoppingCartItemModel(curcustomer.Id, cartType);
            double subtotal = 0.00;
            foreach (var totalprice in model.CartDetail)
            {
                subtotal = subtotal + Convert.ToDouble(totalprice.TotalPrice);
            }
            ViewBag.subtotal = subtotal.ToString("#,##0.00");
            var currency = (from r in model.CartDetail.AsEnumerable()
                            select r.CurrencyCode).Take(1).ToList();
            ViewBag.Currencycode = (currency.Count > 0) ? currency[0] : "Rs";
            return View(model);
        }
        [HttpPost]
        public ActionResult Cart(ShoppingCartItemModel detailModel)
        {
            List<ShoppingCartItem> cartUpdateItems = new List<ShoppingCartItem>();
            cartUpdateItems = (from r in detailModel.items.AsEnumerable()
                               select new ShoppingCartItem { CartId=r.CartId,Quantity=Convert.ToInt32(r.SelectedQuantity),IsRemove=r.IsRemove}).ToList();
            ShoppingCartService.ModifyCartItem(cartUpdateItems);
            return RedirectToRoute("ShoppingCart");
        }
        public ActionResult CartItem()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = PrepareShoppingCartItemModel(curcustomer.Id, cartType);
            return PartialView("CartItems",model);
        }

        public ActionResult Continueshopping()
        {
            string returnurl = "";
            HttpCookie myCookie = new HttpCookie("Returnurl");
            myCookie = Request.Cookies["Returnurl"];
            if (myCookie != null)
            {
                returnurl = myCookie.Value;
            }
            else
            { returnurl = ""; }
             string previousurl ="";
             if (!string.IsNullOrEmpty(returnurl))
                 previousurl = returnurl;
             else
                 previousurl = Request.Url.Scheme+"://"+Request.Url.Authority;

             Request.Cookies.Remove("Returnurl");
            return Redirect(previousurl);
        }

        private ShoppingCartItemsModel PrepareShoppingCartItemModel(int customerId, ShoppingCartType cartType)
        {
            var model = new ShoppingCartItemsModel(ShoppingCartService.GetCartItems("select", 0, cartType, customerId, 0, 0));

            return model;
        }
    }
}
