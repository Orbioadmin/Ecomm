using Orbio.Core;
using Orbio.Core.Domain.Admin.Customers;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Admin.Customers;
using Orbio.Services.Admin.Orders;
using Orbio.Services.Common;
using Orbio.Web.UI.Areas.Admin.Models.Customers;
using Orbio.Web.UI.Areas.Admin.Models.Orders;
using Orbio.Web.UI.Controllers;
using Orbio.Web.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

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

        [AdminAuthorizeAttribute]
        public ActionResult CurrentCarts()
        {
            return View();
        }

        public ActionResult CartCustomer(int? page)
        {
            ShoppingCartType cartType = ShoppingCartType.ShoppingCart;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var model = new ShoppingCartModel(_shoppingCartService.GetShoppingCartAllCustomer(cartType, _storeContext.CurrentStore.Id),pageNumber,pageSize);
            return PartialView("_CartCustomer",model.customers);
        }

        [AdminAuthorizeAttribute]
        public ActionResult CurrentWishLists()
        {
            return View();
        }

        public ActionResult WishlistCustomer(int? page)
        {
            ShoppingCartType cartType = ShoppingCartType.Wishlist;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var model = new ShoppingCartModel(_shoppingCartService.GetShoppingCartAllCustomer(cartType, _storeContext.CurrentStore.Id), pageNumber, pageSize);
            return PartialView("_WishListCustomer", model.customers);
        }
    }
}