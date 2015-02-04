using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductDetail : Product
    {
        public List<SpecificationFilterModel> SpecificationFilters { get; set; }

    }
}
