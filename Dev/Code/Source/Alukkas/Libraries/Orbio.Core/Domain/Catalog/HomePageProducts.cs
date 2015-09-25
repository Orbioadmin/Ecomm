using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class HomePageProducts
    {
        /// <summary>
        /// Gets or sets products
        /// </summary>
        [DataMember]
        public List<ProductDetail> ProductDetails { get; set; }
    }
}
