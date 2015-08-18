using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order note
    /// </summary>
    public partial class OrderNote  
    {
        /// <summary>
        /// Gets or sets the order not identifier
        /// </summary>
        public int Id { get; set; }
 
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a customer can see a note
        /// </summary>
        public bool DisplayToCustomer { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order note creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets the order
        /// </summary>
        public virtual Order Order { get; set; }
    }
}
