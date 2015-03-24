using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Customer
{
    public class EmailFriendModel
    {
        public EmailFriendModel()
        {

        }

        [Required (ErrorMessage="Email Required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Entered Email format is not valid.")]
        [Display(Name = "Friend's Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Captcha Code Required")]
        [DataType(DataType.Text)]
        [Display(Name = "Enter the code")]
        public string Captcha { get; set; }

        public string CaptchaCode { get; set;}

        [DataType(DataType.MultilineText)]
        [Display(Name = "Personal Message")]
        public string Message { get; set; }

        public string SeName { get; set; }

        public string Name { get; set; }

        public string url { get; set; }
    }
}