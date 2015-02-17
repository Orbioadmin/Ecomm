using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductDetail : Product
    {
        /// <summary>
        /// gets all Product Specification Attributes
        /// </summary>
        [DataMember]
        public List<SpecificationFilterModel> SpecificationFilters { get; set; }

        /// <summary>
        /// gets all Product Picture urls
        /// </summary>
        [DataMember]
        public List<ProductPicture> ProductPictures { get; set; }

        /// <summary>
        /// gets all Product Attributes
        /// </summary>
        [DataMember]
        public List<ProductAttribute> ProductAttributes { get; set; }

        /// <summary>
        /// gets all ProductvarientAttributevalues
        /// </summary>
        [DataMember]
        public List<ProductVarientAttributeValue> ProductVarientAttributeValues { get; set; }

    }
}
