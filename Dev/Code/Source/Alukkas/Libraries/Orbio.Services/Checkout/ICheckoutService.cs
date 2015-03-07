using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Checkout;

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
        Address GetCustomerAddress(string email, string billorship);

        /// <summary>
        /// update customer address
        /// </summary>
        /// 
        void UpdateCustomerAddress(string email, bool sameaddress, string BillFirstName, string BillLastName, string BillPhone, string BillAddress,
                string BillCity, string BillPincode, string BillState, string BillCountry, string ShipFirstName, string ShipLastName,
                string ShipPhone, string ShipAddress, string ShipCity, string ShipPincode, string ShipState, string ShipCountry);
    }
}
