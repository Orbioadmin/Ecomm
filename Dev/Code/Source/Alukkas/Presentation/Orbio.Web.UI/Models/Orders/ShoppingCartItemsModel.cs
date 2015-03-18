using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public class ShoppingCartItemsModel
    {
         public ShoppingCartItemsModel()
        {
        }
         public ShoppingCartItemsModel(ShoppingCartItems productDetail)
             : this()
         {
             if (productDetail.ShoppingCartProductItems != null && productDetail.ShoppingCartProductItems.Count > 0)
             {
                 this.CartDetail = (from p in productDetail.ShoppingCartProductItems
                                    select new ShoppingCartItemModel(p)).ToList();
             }
         }

         public List<ShoppingCartItemModel> CartDetail { get; private set; }
    }
}