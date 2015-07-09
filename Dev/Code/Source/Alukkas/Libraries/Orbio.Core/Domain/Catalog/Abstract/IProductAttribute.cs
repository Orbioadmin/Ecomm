using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog.Abstract
{
    public interface IProductAttribute
    {      
        string AttributeName { get; }
        List<IProductAttributeValue> ProductAttributeValues { get; } 
    }
}
