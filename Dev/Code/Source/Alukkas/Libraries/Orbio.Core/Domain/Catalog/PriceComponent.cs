using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class PriceComponent
    {
        /// <summary>
        /// gets or set the name of price component
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// gets or set the   price of component
        /// </summary>
        [DataMember]
        public decimal? Price { get; set; }

        /// <summary>
        /// gets or set the percentage of price component
        /// </summary>
        [DataMember]
        public decimal? Percentage { get; set; }

        /// <summary>
        /// gets or set the price per item the component
        /// </summary>
        [DataMember]
        public decimal? ItemPrice { get; set; }
    }
}
