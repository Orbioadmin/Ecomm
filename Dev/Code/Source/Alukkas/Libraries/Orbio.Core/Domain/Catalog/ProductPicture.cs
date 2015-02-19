using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductPicture
    {
        
        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        [DataMember]
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the picture Url
        /// </summary>
        [DataMember]
        public string RelativeUrl { get; set; }

        /// <summary>
        /// Gets or sets the picture mime type
        /// </summary>
        [DataMember]
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the SEO friednly filename of the picture
        /// </summary>
        [DataMember]
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the picture is new
        /// </summary>
        [DataMember]
        public bool IsNew { get; set; }
    }
}
