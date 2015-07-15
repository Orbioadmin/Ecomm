using Orbio.Core.Domain.Catalog.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    public class TransientCartAttribute  
    {
        
        public string AttributeName { get; set; }
        public List<TransientCartAttributeValue> AttributeValues { get; set; }

 
    }
}
