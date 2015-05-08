using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Email
{
    //Represents an email
    public partial class EmailDetail
    {
        /// <summary>
        /// Gets or sets the fromaddress
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the ToAddress
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the smtp credentials
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the FromName
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the ToName
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Body
        /// </summary>
        public string Body { get; set; }
    }
}
