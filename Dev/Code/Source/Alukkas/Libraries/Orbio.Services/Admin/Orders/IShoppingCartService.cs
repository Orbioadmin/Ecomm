using Orbio.Core.Domain.Admin.Orders;
using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
   public partial interface IShoppingCartService
    {
        /// <summary>
        /// get all current shopping cart customer
        /// </summary>
        /// <returns></returns>
       ShoppingCart GetShoppingCartAllCustomer(ShoppingCartType shoppingCartType, int storeId);
    }
}
