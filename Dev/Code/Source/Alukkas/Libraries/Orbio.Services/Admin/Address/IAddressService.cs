using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Data;

namespace Orbio.Services.Admin.Address
{
    public partial interface IAddressService
    {
        /// <summary>
        /// Get address details by id
        /// </summary>
        ///<param name="id">address identifier</param>
        Orbio.Core.Data.Address GetAddressDetailsById(int addressId);

        /// <summary>
        /// Update address details
        /// </summary>
        void UpdateAddress(int id, string firstName, string lastName, string address, string phone, string city, string postalCode,int stateProvinceId, int countryId);
    }
}
