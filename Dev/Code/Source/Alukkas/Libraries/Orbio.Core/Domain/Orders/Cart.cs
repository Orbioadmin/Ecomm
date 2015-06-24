using Orbio.Core.Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    /// <summary>Cart
    /// represents 
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// gets or sets the order level discounts
        /// </summary>
        public List<Discount> Discounts { get; set; }

        /// <summary>
        /// gets or sets list of shoppingcart item
        /// </summary>
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
