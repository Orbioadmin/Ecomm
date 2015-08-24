using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Checkout;
using Orbio.Core.Domain.Orders;

namespace Orbio.Services.Checkout
{
   public partial interface ICheckoutService
    {
        /// <summary>
        /// get customer registered address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="billorship"></param>
        /// <returns></returns>
       Address GetCustomerAddress(string email, string billorShip, ShoppingCartStatus shoppingCartStatus, ShoppingCartType shoppingCartType);

        /// <summary>
        /// update customer address
        /// </summary>
        /// 
        void UpdateCustomerAddress(string email, bool sameAddress, string billFirstName, string billLastName, string billPhone, string billAddress,
                string billCity, string billPincode, string billState, string billCountry, string shipFirstName, string shipLastName,
                string shipPhone, string shipAddress, string shipCity, string shipPincode, string shipState, string shipCountry, ShoppingCartStatus shoppingCartStatus, ShoppingCartType shoppingCartType);
    }
}
