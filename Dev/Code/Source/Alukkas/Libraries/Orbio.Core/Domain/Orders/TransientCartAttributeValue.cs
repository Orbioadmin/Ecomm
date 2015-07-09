using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    public class TransientCartAttributeValue
    {
        public decimal PriceAdjustment { get; set; }
        public string AttributeValue { get; set; }
    }
}
