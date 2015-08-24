using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Orbio.Services.Orders
{
    /// <summary>
    /// ShoppingCart service interface
    /// </summary>
    public partial interface IShoppingCartService
    {
        /// <summary>
        /// Add shopping cart item
        /// </summary>
        /// <param name="action">Action</param>
        void AddCartItem(string action, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, string attributexml, int Quantity, ShoppingCartStatus shoppingCartStatus);

        /// <summary>
        /// Add wishlist item
        /// </summary>
        /// <param name="action">Action</param>
        string AddWishlistItem(string action, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, string attributexml, int Quantity, ShoppingCartStatus shoppingCartStatus);
        
        /// <summary>
        /// Migrate shopping cart item
        /// </summary>
        /// <param name="action">Action</param>
        void MigrateShoppingCart(string action, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, string attributexml, int Quantity);

         /// <summary>
        /// Get shopping cart items
        /// </summary>
        /// <param name="action">Action</param>
        Cart GetCartItems(string action, int id, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, int Quantity, int storeId);

        /// <summary>
        /// Update and delete shopping cart item
        /// </summary>
        /// <param name="cartItems">cartItems</param>
        void ModifyCartItem(List<ShoppingCartItem> cartItems);

        /// <summary>
        /// Update wishlist items
        /// </summary>
        /// <param name="action">Action</param>
        string UpdateWishListItems(string action, int id, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, int quantity, ShoppingCartStatus shoppingCartStatus);
    }
}
