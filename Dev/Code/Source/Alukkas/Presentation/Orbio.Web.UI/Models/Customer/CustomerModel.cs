using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Customer
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            // TODO: Complete member initialization
        }

        public CustomerModel(Orbio.Core.Domain.Customers.Customer Customer)
            : this()
        {
            this.FirstName = Customer.FirstName;
            this.LastName = Customer.LastName;
            this.Gender = Customer.Gender;
            this.DOB = Customer.DOB;
            this.Email = Customer.Email;
            this.Mobile = Customer.MobileNo;

        }

        [Required(ErrorMessage = "First Name Required")]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Gender Required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth Required")]
        [Display(Name = "Date of Birth")]
        public string DOB { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage="Mobile Number Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered mobile number is not valid.")]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords does not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmNewPassword { get; set; }

        public string Result { get; set; }
    }
}