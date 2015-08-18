using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Directory
{
    [DataContract]
    public partial class Country
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether billing is allowed to this country
        /// </summary>
        [DataMember]
        public bool AllowsBilling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether shipping is allowed to this country
        /// </summary>
        [DataMember]
        public bool AllowsShipping { get; set; }

        /// <summary>
        /// Gets or sets the two letter ISO code
        /// </summary>
        [DataMember]
        public string TwoLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the three letter ISO code
        /// </summary>
        [DataMember]
        public string ThreeLetterIsoCode { get; set; }

        /// <summary>
        /// Gets or sets the numeric ISO code
        /// </summary>
        [DataMember]
        public int NumericIsoCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers in this country must be charged EU VAT
        /// </summary>
        [DataMember]
        public bool SubjectToVat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        [DataMember]
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [DataMember]
        public int DisplayOrder { get; set; }
    }
}
