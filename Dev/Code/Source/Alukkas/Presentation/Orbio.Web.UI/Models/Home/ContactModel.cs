using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Home
{
    public class ContactModel
    {
        /// <summary>
        /// gets or sets customer name
        /// </summary>
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        /// <summary>
        /// gets or sets email id
        /// </summary>
         [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }

        /// <summary>
        /// gets or sets message
        /// </summary>
         [Required(ErrorMessage = "Message Required")]
         public string Message { get; set; }
    }
}