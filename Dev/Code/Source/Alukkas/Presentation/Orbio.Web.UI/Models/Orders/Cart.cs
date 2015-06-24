using System.Collections.Generic;
using System.Linq;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Discounts;

namespace Orbio.Web.UI.Models.Orders
{
    public class CartModel : ICart
    {
         public CartModel()
        {
            this.ShoppingCartItems = new List<ShoppingCartItemModel>();
            this.Discounts = new List<Discount>();
        }
         public CartModel(Cart cart)
             : this()
         {
             if (cart.ShoppingCartItems != null && cart.ShoppingCartItems.Count > 0)
             {
                 this.ShoppingCartItems = (from p in cart.ShoppingCartItems
                                    select new ShoppingCartItemModel(p)).ToList();
             }

             if (cart.Discounts != null && cart.Discounts.Count > 0)
             {
                 this.Discounts = cart.Discounts;
             }
         }

         public List<ShoppingCartItemModel> ShoppingCartItems { get; private set; }

         public List<Discount> Discounts { get; set; }

         IEnumerable<IShoppingCartItem> ICart.ShoppingCartItems
         {
             get
             {
                 return this.ShoppingCartItems;

             }
         }


         IEnumerable<IDiscount> ICart.Discounts
         {
             get { return this.Discounts;  }
         }
    }
}