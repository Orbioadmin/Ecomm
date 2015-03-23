using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Customer
{
    public class PasswordRecoveryConfirmModel
    {
        [Required(ErrorMessage = "New Password Required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }

        public string Result { get; set; }
    }
}