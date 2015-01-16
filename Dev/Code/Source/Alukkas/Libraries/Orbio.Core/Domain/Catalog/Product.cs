using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class Product
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

    }
}
