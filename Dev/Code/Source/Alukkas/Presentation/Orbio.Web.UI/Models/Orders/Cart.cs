using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public class Cart
    {
         public Cart()
        {
            this.ShoppingCartItems = new List<ShoppingCartItemModel>();
        }
         public Cart(List<ShoppingCartItem> cartItems)
             : this()
         {
             if (cartItems != null && cartItems.Count > 0)
             {
                 this.ShoppingCartItems = (from p in cartItems
                                    select new ShoppingCartItemModel(p)).ToList();
             }
         }

         public List<ShoppingCartItemModel> ShoppingCartItems { get; private set; }
    }
}