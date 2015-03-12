using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductReview
    {
        /// <summary>
        /// get or set review title
        /// </summary>
        [DataMember]
        public string ReviewTitle { get; set; }
        
        /// <summary>
        /// get or sets review text
        /// </summary>

        [DataMember]
        public string ReviewText { get; set; }

        /// <summary>
        /// get or sets rating
        /// </summary>
        [DataMember]
        public int Rating { get; set; }

        /// <summary>
        /// get or sets Customer Name
        /// </summary>
        [DataMember]
        public string Customer { get; set; }

        /// <summary>
        /// get or sets starcount
        /// </summary>
        [DataMember]
        public int starcount{get;set;}

        [DataMember]
        public List<ProductReview> ProductReviews { get; set; }
    }
}
