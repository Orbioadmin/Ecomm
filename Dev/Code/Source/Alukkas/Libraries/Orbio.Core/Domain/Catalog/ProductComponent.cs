using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductComponent
    {
        /// <summary>
        /// gets or set the name of product component
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// gets or set the Weight of product component
        /// </summary>
        [DataMember]
        public decimal Weight { get; set; }

        /// <summary>
        /// gets or set the UnitPrice of product component
        /// </summary>
        [DataMember]
        public decimal UnitPrice { get; set; }
    }
}
