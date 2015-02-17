using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Stores
{
    /// <summary>
    /// Represents a store
    /// </summary>
    public partial class Store  
    {
        /// <summary>
        /// Gets or sets the store id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the store name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the store URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is enabled
        /// </summary>
        public bool SslEnabled { get; set; }

        /// <summary>
        /// Gets or sets the store secure URL (HTTPS)
        /// </summary>
        public string SecureUrl { get; set; }

        /// <summary>
        /// Gets or sets the comma separated list of possible HTTP_HOST values
        /// </summary>
        public string Hosts { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }


    }
}
