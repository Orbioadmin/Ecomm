﻿using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Customers
{
    [DataContract]
    public partial class Customer 
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// Gets or sets the email
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// gets or sets shoppping cart count
        /// </summary>
        [DataMember]
        public Cart Cart { get; set; }
    }
}
