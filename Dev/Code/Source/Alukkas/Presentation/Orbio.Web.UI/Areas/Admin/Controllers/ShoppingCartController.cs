using Orbio.Core;
using Orbio.Core.Domain.Admin.Customers;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Admin.Customers;
using Orbio.Services.Admin.Orders;
using Orbio.Services.Common;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using Orbio.Web.UI.Areas.Admin.Models.Orders;
using Orbio.Web.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ShoppingCartController:Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;
        public ShoppingCartController(IShoppingCartService _shoppingCartService, IStoreContext storeContext)
        {
            this._shoppingCartService = _shoppingCartService;
            this._storeContext = storeContext;
        }

        public ActionResult CurrentCarts()
        {
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            var model = new ShoppingCartModel(_shoppingCartService.GetShoppingCartAllCustomer(cartType, _storeContext.CurrentStore.Id));

            return View(model);
        }

        public ActionResult CurrentWishLists()
        {
            ShoppingCartType cartType = ShoppingCartType.Wishlist;
            var model = new ShoppingCartModel(_shoppingCartService.GetShoppingCartAllCustomer(cartType, _storeContext.CurrentStore.Id));

            return View(model);
        }
    }
}