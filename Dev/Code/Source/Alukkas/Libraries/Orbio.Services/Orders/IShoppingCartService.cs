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
        void AddCartItem(string action, int ShoppingCartTypeId, int CustomerId, int ProductId, string attributexml, int Quantity);

         /// <summary>
        /// Get shopping cart items
        /// </summary>
        /// <param name="action">Action</param>
        ShoppingCartItems GetCartItems(string action, int id, int ShoppingCartTypeId, int CustomerId, int ProductId, int Quantity);

        /// <summary>
        /// Update and delete shopping cart item
        /// </summary>
        /// <param name="action">Action</param>
        void ModifyCartItem(string cartitems);
    }
}
