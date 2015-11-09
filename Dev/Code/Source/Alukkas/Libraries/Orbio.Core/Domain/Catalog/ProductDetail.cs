using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductDetail : Product
    {
        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the entity is ship enabled
        /// </summary>
        public bool IsShipEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is free shipping
        /// </summary>
        public bool IsFreeShipping { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to display stock availability
        /// </summary>
        public bool DisplayStockAvailability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display stock quantity
        /// </summary>
        public bool DisplayStockQuantity { get; set; }
        /// <summary>
        /// Gets or sets a value indicating how to manage inventory
        /// </summary>
        public int ManageInventoryMethodId { get; set; }
        /// <summary>
        /// Gets or sets the value indicating how to manage inventory
        /// </summary>
        public ManageInventoryMethod ManageInventoryMethod
        {
            get
            {
                return (ManageInventoryMethod)this.ManageInventoryMethodId;
            }
            set
            {
                this.ManageInventoryMethodId = (int)value;
            }
        }
        /// <summary>
        /// Gets or sets a value of used category template viewpath
        /// </summary>
        [DataMember]
        public string ProductViewPath { get; set; }

        /// <summary>
        /// gets or sets categories for breadcrumbs
        /// </summary>
        [DataMember]
        public List<Category> BreadCrumbs { get; set; }

        /// <summary>
        /// gets or sets categories for DeliveredIn
        /// </summary>
        [DataMember]
        public string DeliveredIn { get; set; }
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
        public List<ProductAttributeVariant> ProductAttributeVariants { get; set; }

        /// <summary>
        /// Gets or sets OrderMinimumQuantity
        /// </summary>
        [DataMember]
        public int OrderMinimumQuantity { get; set; }

        /// <summary>
        /// Gets or sets OrderMaximumQuantity
        /// </summary>
        [DataMember]
        public int OrderMaximumQuantity { get; set; }

        /// <summary>
        /// Gets or sets AllowedQuantities
        /// </summary>
        [DataMember]
        public string AllowedQuantities { get; set; }

        /// <summary>
        /// Gets or sets IsGift
        /// </summary>
        public bool IsGift { get; set; }

        /// <summary>
        /// Gets or sets Gift Charge
        /// </summary>
        public decimal GiftCharge { get; set; }


        /// <summary>
        /// Gets or sets IsGift
        /// </summary>
        public bool IsGiftWrapping { get; set; }

        /// <summary>
        /// Gets or sets MetaDescription
        /// </summary>
        public string MetaDescription{ get; set; }

        /// <summary>
        /// Gets or sets MetaKeywords
        /// </summary>
        public string MetaKeywords{ get; set; }

        /// <summary>
        /// Gets or sets MetaTitle
        /// </summary>
        public string MetaTitle{ get; set; }
    }
}
