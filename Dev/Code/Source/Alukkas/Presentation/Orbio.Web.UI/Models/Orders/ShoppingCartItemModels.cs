using Orbio.Core.Domain.Orders;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public class ShoppingCartItemModels : ProductOverViewModel
    {
        public ShoppingCartItemModels()
        {
            this.Products = new List<ShoppingCartProductDetailModel>();    
        }
        public ShoppingCartItemModels(ShoppingCartItem cartitem)
            : this()
        {
            //this.Itemcount = cartitem.Itemcount;
            //if (cartitem.ShoppingCartProducts != null && cartitem.ShoppingCartProducts.Count > 0)
            //{
            //    this.Products = (from p in cartitem.ShoppingCartProducts
            //                     select new ShoppingCartProductDetailModel(p)).ToList();
            //}
        }
        public int Itemcount { get; set; }
        public List<ShoppingCartProductDetailModel> Products { get; set; }
    }
}