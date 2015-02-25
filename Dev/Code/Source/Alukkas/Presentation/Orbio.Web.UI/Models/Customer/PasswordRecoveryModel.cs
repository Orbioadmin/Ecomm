using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Customer
{
    public class PasswordRecoveryModel
    {
        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Result { get; set; }
    }
}