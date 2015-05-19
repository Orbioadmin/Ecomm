using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    [DataContract]
    public class Order : ProductDetail
    {
        public int OrderNumber { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; }

        public int CustomerId { get; set; }

    }
}
