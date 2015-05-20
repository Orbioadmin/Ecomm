﻿using Orbio.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    [DataContract]
    public class OrderDetails
    {
        public List<Order> OrderedProductItems { get; set; }
    }
}
