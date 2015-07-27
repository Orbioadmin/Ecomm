using Orbio.Core.Domain.Checkout;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.CheckOut
{
    public class AddressModel
    {
        public AddressModel()
        {

        }

        public AddressModel(Address address)
            :this()
        {
            this.BillFirstName = address.FirstName;
            this.BillLastName = address.LastName;
            this.BillPhone = address.PhoneNumber;
            this.BillAddress = address.Address1 + address.Address2;
            this.BillCity = address.City;
            this.BillPincode = address.ZipPostalCode;
            this.BillState = address.States;
            this.BillCountry = address.Country;
            this.ShipFirstName = address.FirstName;
            this.ShipLastName = address.LastName;
            this.ShipPhone = address.PhoneNumber;
            this.ShipAddress = address.Address1 + address.Address2;
            this.ShipCity = address.City;
            this.ShipPincode = address.ZipPostalCode;
            this.ShipState = address.States;
            this.ShipCountry = address.Country;
            this.SameAddress = checked(true);
        }
        public AddressModel(Address billAddress, Address shipAddress)
            : this()
        {
            this.BillFirstName = billAddress.FirstName;
            this.BillLastName = billAddress.LastName;
            this.BillPhone = billAddress.PhoneNumber;
            this.BillAddress = billAddress.Address1 + billAddress.Address2;
            this.BillCity = billAddress.City;
            this.BillPincode = billAddress.ZipPostalCode;
            this.BillState = billAddress.States;
            this.BillCountry = billAddress.Country;
            this.ShipFirstName = shipAddress.FirstName;
            this.ShipLastName = shipAddress.LastName;
            this.ShipPhone = shipAddress.PhoneNumber;
            this.ShipAddress = shipAddress.Address1 + shipAddress.Address2;
            this.ShipCity = shipAddress.City;
            this.ShipPincode = shipAddress.ZipPostalCode;
            this.ShipState = shipAddress.States;
            this.ShipCountry = shipAddress.Country;
            this.SameAddress = checked(false);
        }
         public AddressModel(Address address,string name)
            :this()
         {
             if(name=="Billing")
             {
                 this.BillFirstName = address.FirstName;
                 this.BillLastName = address.LastName;
                 this.BillPhone = address.PhoneNumber;
                 this.BillAddress = address.Address1  + address.Address2;
                 this.BillCity = address.City;
                 this.BillPincode = address.ZipPostalCode;
                 this.BillState = address.States;
                 this.BillCountry = address.Country;
             }
             else
             {
                 this.ShipFirstName = address.FirstName;
                 this.ShipLastName = address.LastName;
                 this.ShipPhone = address.PhoneNumber;
                 this.ShipAddress = address.Address1 + "," + address.Address2;
                 this.ShipCity = address.City;
                 this.ShipPincode = address.ZipPostalCode;
                 this.ShipState = address.States;
                 this.ShipCountry = address.Country;
             }
         }
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid First Name")]
        [Display(Name = "FirstName")]
        public string BillFirstName { get; set; }

        [Required]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid Last Name")]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string BillLastName { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone number is not valid.")]
        [Display(Name = "Phone")]
        public string BillPhone { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string BillAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string BillCity { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Pincode")]
        public string BillPincode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "State")]
        public string BillState { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Country")]
        public string BillCountry { get; set; }

        [Display(Name = "Billing Address same as shipping address")]
        public bool SameAddress { get; set; }

        [Required]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid First Name")]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string ShipFirstName { get; set; }

        [Required]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid Last Name")]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string ShipLastName { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone number is not valid.")]
        [Display(Name = "Phone")]
        public string ShipPhone { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string ShipAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string ShipCity { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Pincode")]
        public string ShipPincode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "State")]
        public string ShipState { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Country")]
        public string ShipCountry { get; set; }
    }
}