using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class Search
    {
        [DataMember]
        public string CategoryId { get; set; }

        /// <summary>
        /// gets or sets total count 
        /// </summary>
        [DataMember]
        public string Totalcount { get; set; }

        /// <summary>
        /// Gets or sets products
        /// </summary>
        [DataMember]
        public List<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets a value of used category template viewpath
        /// </summary>
        [DataMember]
        public string TemplateViewPath { get; set; }
    }
}
