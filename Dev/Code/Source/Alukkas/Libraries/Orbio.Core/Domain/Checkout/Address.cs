using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Checkout
{
    public partial class Address 
    {
        public Address()
        {
        }

        /// <summary>
        /// Gets or sets the Firstname
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Lastname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the State
        /// </summary>
        public string StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// Gets or sets the Phoneno
        /// </summary>
        public string PhoneNumber { get; set; }

         /// <summary>
        /// Gets or sets the Postal code
        /// </summary>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the Billing Address
        /// </summary>
        public int? BillingAddress_Id { get; set; }

        /// <summary>
        /// Gets or sets the Shipping Address
        /// </summary>
        public int? ShippingAddress_Id { get; set; }

        /// <summary>
        /// Gets or sets the State
        /// </summary>
        public string States { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country{ get; set; }

    }
}
