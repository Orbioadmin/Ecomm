using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    /// <summary>
    /// Represents a shoping cart type
    /// </summary>
    public enum ShoppingCartType
    {
        /// <summary>
        /// Shopping cart
        /// </summary>
        ShoppingCart = 1,
        /// <summary>
        /// Wishlist
        /// </summary>
        Wishlist = 2,
    }
}
