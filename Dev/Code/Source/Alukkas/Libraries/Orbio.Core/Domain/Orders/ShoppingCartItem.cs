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
    /// Represents a Cart Item
    /// </summary>
    [DataContract]
    public class ShoppingCartItem : ProductDetail
    {
        /// <summary>
        /// Gets or sets the Cart identifier
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// Gets or sets the shopping cart type identifier
        /// </summary>
        public int ShoppingCartTypeId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product variant attributes
        /// </summary>
        public string AttributesXml { get; set; }

        /// <summary>
        /// Gets or sets the price enter by a customer
        /// </summary>
        public decimal CustomerEnteredPrice { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }

        ///// <summary>
        ///// Gets or sets the Item count
        ///// </summary>
        //public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the Total Price
        /// </summary>
       // public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the Total Price
        /// </summary>
        public bool IsRemove { get; set; }

    }
}
