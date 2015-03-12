using Nop.Core.Infrastructure;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Orders;
using Orbio.Web.UI.Models.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            foreach (var totalprice in model.Products)
            {
                subtotal = subtotal + Convert.ToDouble(totalprice.Totalprice);
            }
            ViewBag.subtotal = subtotal.ToString("0.00");
            return View(model);
        }
        [HttpPost]
        public ActionResult Cart(FormCollection formkey, ShoppingCartItemModels detailmodel)
        {
            var table = new DataTable();
            table.Columns.Add("CartId", typeof(string));
            table.Columns.Add("Remove", typeof(bool));
            table.Columns.Add("Quantity", typeof(string));
            foreach (var item in detailmodel.Products)
            {
                table.Rows.Add(item.CartId, item.IsRemove, item.SelectedQuantity);
            }
            shoppingcartservice.ModifyCartItem(table);
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
        private ShoppingCartItemModels PrepareShoppingCartItemModel(int customerid,int carttype)
        {
            var model = new ShoppingCartItemModels(shoppingcartservice.GetCartItems("select",carttype,customerid,0,0));

            return model;
        }
    }
}
