using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Checkout
{
    [DataContract]
    public class Address 
    {
        /// <summary>
        /// Gets or sets the Firstname
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Lastname
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        [DataMember]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        [DataMember]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>
        [DataMember]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the State
        /// </summary>
        [DataMember]
        public string StateProvinceId { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        [DataMember]
        public string CountryId { get; set; }

        /// <summary>
        /// Gets or sets the Phoneno
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

         /// <summary>
        /// Gets or sets the Postal code
        /// </summary>
        [DataMember]
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the Billing Address
        /// </summary>
        [DataMember]
        public int? BillingAddress_Id { get; set; }

        /// <summary>
        /// Gets or sets the Shipping Address
        /// </summary>
        [DataMember]
        public int? ShippingAddress_Id { get; set; }

        /// <summary>
        /// Gets or sets the State
        /// </summary>
        [DataMember]
        public string States { get; set; }

        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        [DataMember]
        public string Country{ get; set; }

    }
}
