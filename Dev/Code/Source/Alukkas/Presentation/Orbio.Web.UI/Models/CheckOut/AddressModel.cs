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

        public AddressModel(Orbio.Core.Domain.Checkout.Address Address)
            :this()
        {
            this.BillFirstName = Address.FirstName;
            this.BillLastName = Address.LastName;
            this.BillPhone = Address.PhoneNumber;
            this.BillAddress = Address.Address1 + Address.Address2;
            this.BillCity = Address.City;
            this.BillPincode = Address.ZipPostalCode;
            this.BillState = Address.States;
            this.BillCountry = Address.Country;
            this.ShipFirstName = Address.FirstName;
            this.ShipLastName = Address.LastName;
            this.ShipPhone = Address.PhoneNumber;
            this.ShipAddress = Address.Address1 + Address.Address2;
            this.ShipCity = Address.City;
            this.ShipPincode = Address.ZipPostalCode;
            this.ShipState = Address.States;
            this.ShipCountry = Address.Country;
            this.SameAddress = checked(true);
        }
        public AddressModel(Orbio.Core.Domain.Checkout.Address BillAddress, Orbio.Core.Domain.Checkout.Address ShipAddress)
            : this()
        {
            this.BillFirstName = BillAddress.FirstName;
            this.BillLastName = BillAddress.LastName;
            this.BillPhone = BillAddress.PhoneNumber;
            this.BillAddress = BillAddress.Address1 + BillAddress.Address2;
            this.BillCity = BillAddress.City;
            this.BillPincode = BillAddress.ZipPostalCode;
            this.BillState = BillAddress.States;
            this.BillCountry = BillAddress.Country;
            this.ShipFirstName = ShipAddress.FirstName;
            this.ShipLastName = ShipAddress.LastName;
            this.ShipPhone = ShipAddress.PhoneNumber;
            this.ShipAddress = ShipAddress.Address1 + ShipAddress.Address2;
            this.ShipCity = ShipAddress.City;
            this.ShipPincode = ShipAddress.ZipPostalCode;
            this.ShipState = ShipAddress.States;
            this.ShipCountry = ShipAddress.Country;
            this.SameAddress = checked(false);
        }
         public AddressModel(Orbio.Core.Domain.Checkout.Address Address,string name)
            :this()
         {
             if(name=="Billing")
             {
                 this.BillFirstName = Address.FirstName;
                 this.BillLastName = Address.LastName;
                 this.BillPhone = Address.PhoneNumber;
                 this.BillAddress = Address.Address1  + Address.Address2;
                 this.BillCity = Address.City;
                 this.BillPincode = Address.ZipPostalCode;
                 this.BillState = Address.States;
                 this.BillCountry = Address.Country;
             }
             else
             {
                 this.ShipFirstName = Address.FirstName;
                 this.ShipLastName = Address.LastName;
                 this.ShipPhone = Address.PhoneNumber;
                 this.ShipAddress = Address.Address1 + "," + Address.Address2;
                 this.ShipCity = Address.City;
                 this.ShipPincode = Address.ZipPostalCode;
                 this.ShipState = Address.States;
                 this.ShipCountry = Address.Country;
             }
         }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string BillFirstName { get; set; }

        [Required]
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
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string ShipFirstName { get; set; }

        [Required]
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