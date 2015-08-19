using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Address
{
    public partial class AddressService:IAddressService
    {
        #region Fields
        private readonly OrbioAdminContext context = new OrbioAdminContext();
        #endregion

        public AddressService()
        {
        }

        #region Methods

        /// <summary>
        /// Get address details by id
        /// </summary>
        ///<param name="id">address identifier</param>
        public Orbio.Core.Data.Address GetAddressDetailsById(int addressId)
        {
            var address = context.Addresses.AsQueryable().Where(o => o.Id == addressId);

            return address.FirstOrDefault();
        }


        public void UpdateAddress(int id, string firstName, string lastName, string address, string phone, string city, string postalCode, int stateProvinceId, int countryId)
        {
            Orbio.Core.Data.Address addressDetail = context.Addresses.First(a=>a.Id == id);
             addressDetail.FirstName = firstName;
             addressDetail.LastName = lastName;
             addressDetail.Address1 = address;
             addressDetail.PhoneNumber = phone;
             addressDetail.City = city;
             addressDetail.ZipPostalCode = postalCode;
             addressDetail.CountryId = countryId;
             addressDetail.StateProvinceId = stateProvinceId;
            this.context.SaveChanges();
        }
        #endregion

    }
}
