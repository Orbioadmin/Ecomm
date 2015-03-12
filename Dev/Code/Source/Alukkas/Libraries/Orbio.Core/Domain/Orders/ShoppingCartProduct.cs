using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    /// <summary>
    /// Represents a Cart product details
    /// </summary>
    [DataContract]
    public class ShoppingCartProduct : Product
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

        [DataMember]
        public List<ProductPicture> ProductPictures { get; set; }

        /// <summary>
        /// Gets or sets Total Price
        /// </summary>
        [DataMember]
        public decimal Totalprice { get; set; }

        /// <summary>
        /// Gets or sets Cart Id
        /// </summary>
        [DataMember]
        public int CartId { get; set; }

        /// <summary>
        /// Gets or sets Cart quantity
        /// </summary>
        [DataMember]
        public string CartQuantity { get; set; }

        /// <summary>
        /// Gets or sets AllowedQuantities
        /// </summary>
        [DataMember]
        public string AllowedQuantities { get; set; }

        [DataMember]
        public int OrderMinimumQuantity { get; set; }

        /// <summary>
        /// Gets or sets OrderMaximumQuantity
        /// </summary>
        [DataMember]
        public int OrderMaximumQuantity { get; set; }

    }
}
