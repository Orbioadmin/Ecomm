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
            this.CartDetail = new List<ShoppingCartItemModel>();
        }
         public ShoppingCartItemsModel(List<ShoppingCartItem> cartItems)
             : this()
         {
             if (cartItems != null && cartItems.Count > 0)
             {
                 this.CartDetail = (from p in cartItems
                                    select new ShoppingCartItemModel(p)).ToList();
             }
         }

         public List<ShoppingCartItemModel> CartDetail { get; private set; }
    }
}