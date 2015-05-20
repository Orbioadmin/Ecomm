using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductAttributeVariant
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ProductAttributeId
        /// </summary>
        [DataMember]
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// Gets or sets the TextPrompt
        /// </summary>
        [DataMember]
        public string TextPrompt { get; set; }

        /// <summary>
        /// Gets or sets the SizeGuideUrl
        /// </summary>
        [DataMember]
        public string SizeGuideUrl { get; set; }


        /// <summary>
        /// Gets or sets the IsRequired
        /// </summary>
        [DataMember]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the AttributeControlTypeId
        /// </summary>
        [DataMember]
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// Gets the attribute control type
        /// </summary>
        public AttributeControlType AttributeControlType
        {
            get
            {
                return (AttributeControlType)this.AttributeControlTypeId;
            }
            set
            {
                this.AttributeControlTypeId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the Product Variant Attribute Values
        /// </summary>
         [DataMember]
        public List<ProductVariantAttributeValue> ProductVariantAttributeValues { get; set; }

    }
}
