using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductAttribute
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [DataMember]
        public string Description { get; set; }


    }
}
