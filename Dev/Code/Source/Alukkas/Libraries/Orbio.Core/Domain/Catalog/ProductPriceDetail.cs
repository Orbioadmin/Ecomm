using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductPriceDetail
    {
        /// <summary>
        /// gets or set the PriceComponents
        /// </summary>
        [DataMember]
        public List<PriceComponent> PriceComponents { get; set; }

        /// <summary>
        /// gets or set the ProductComponents
        /// </summary>
        [DataMember]
        public List<ProductComponent> ProductComponents { get; set; }
    }
}
