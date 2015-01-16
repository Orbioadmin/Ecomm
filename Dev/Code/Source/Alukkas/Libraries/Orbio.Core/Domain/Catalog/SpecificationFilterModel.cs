using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class SpecificationFilterModel
    {
        /// <summary>
        /// gets or sets specification attribute id
        /// </summary>
        [DataMember]
        public int SpecificationAttributeId { get; set; }

        /// <summary>
        /// gets or sets specification attribute name
        /// </summary>
        [DataMember]
        public string SpecificationAttributeName { get; set; }

        /// <summary>
        /// gets or sets specification attribute option id
        /// </summary>
        [DataMember]
        public int SpecificationAttributeOptionId { get; set; }

        /// <summary>
        /// gets or sets specification attribute option name
        /// </summary>
        [DataMember]
        public string SpecificationAttributeOptionName { get; set; }
    }
}
