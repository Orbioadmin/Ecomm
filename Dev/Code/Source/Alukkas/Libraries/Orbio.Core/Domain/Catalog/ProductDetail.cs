﻿using System.Collections.Generic;
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
        public List<ProductAttribute> ProductAttributes { get; set; }

        /// <summary>
        /// gets all ProductvarientAttributevalues
        /// </summary>
        [DataMember]
        public List<ProductVarientAttributeValue> ProductVarientAttributeValues { get; set; }

    }
}
