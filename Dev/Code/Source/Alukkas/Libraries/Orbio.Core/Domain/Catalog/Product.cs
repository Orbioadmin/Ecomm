using System.Runtime.Serialization;
using Orbio.Core.Domain.Catalog.Abstract;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class Product : IPriceComponent
    {
        /// <summary>
        /// Gets or Sets the id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short description
        /// </summary>
        [DataMember]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Gets or sets the full description
        /// </summary>
        [DataMember]
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        [DataMember]
        public decimal OldPrice { get; set; }

        /// <summary>
        /// Gets or sets the view path
        /// </summary>
        [DataMember]
        public string ViewPath { get; set; }

        /// <summary>
        /// Gets or sets the currency code
        /// </summary>
        [DataMember]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// gets or sets the relative url of product image
        /// </summary>
        [DataMember]
        public string ImageRelativeUrl { get; set; }        

        /// <summary>
        /// gets or sets SeName
        /// </summary>
        [DataMember]
        public string SeName { get; set; }

        /// <summary>
        /// Gets or sets GoldWeight
        /// </summary>
        [DataMember]
        public decimal GoldWeight { get; set; }

        /// <summary>
        /// Gets or sets ProductUnit
        /// </summary>
        [DataMember]
        public decimal ProductUnit { get; set; }

        /// <summary>
        /// Gets or sets PriceUnit
        /// </summary>
        [DataMember]
        public int PriceUnit { get; set; }

        /// <summary>
        /// Gets or sets MarketUnitPrice
        /// </summary>
        [DataMember]
        public decimal MarketUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets ProductPriceDetail
        /// </summary>
        [DataMember]
        public ProductPriceDetail ProductPriceDetail { get; set; }

    }
}
