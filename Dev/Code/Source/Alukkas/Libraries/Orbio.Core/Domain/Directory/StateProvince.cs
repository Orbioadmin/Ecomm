using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Directory
{
    [DataContract]
    public partial class StateProvince
    {
        /// <summary>
        /// Gets or sets the StateProvince identifier
        /// </summary>
        [DataMember]
        public int Id { get; set; }
 
        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        [DataMember]
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation
        /// </summary>
        [DataMember]
        public string Abbreviation { get; set; }

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
