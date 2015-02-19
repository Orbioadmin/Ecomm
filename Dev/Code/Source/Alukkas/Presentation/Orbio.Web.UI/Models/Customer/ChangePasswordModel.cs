using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Customer
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Old Password Required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords does not match")]
        public string ConfirmNewPassword { get; set; }

        public string Result { get; set; }

    }
}