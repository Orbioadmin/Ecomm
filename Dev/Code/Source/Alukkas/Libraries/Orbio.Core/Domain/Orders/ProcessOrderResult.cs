using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Checkout;
using Orbio.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    [DataContract]
    public class ProcessOrderResult
    {
        /// <summary>
        /// Get or set order id
        /// </summary>
         [DataMember]
        public int OrderId { get; set; }

        /// <summary>
        /// Default billing address
        /// </summary>
        [DataMember]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Default shipping address
        /// </summary>
        [DataMember]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Get or set customer details
        /// </summary>
        [DataMember]
        public Customer Customer { get; set; }

        /// <summary>
        /// Get or set order items
        /// </summary>
        [DataMember]
        public List<Product> Products { get; set; }

    }
}
