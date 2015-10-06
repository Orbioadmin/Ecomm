using Orbio.Core.Domain.Checkout;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Area.Admin.Models.CheckOut
{
    public class AddressModel
    {
        public AddressModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        public AddressModel(Orbio.Core.Data.Address address)
        {
            Id = address.Id;
            Country = (address.Country != null) ? address.Country.Name : null;
            State = (address.StateProvince != null) ? address.StateProvince.Name : null;
            City = address.City;
            Address = address.Address1 + address.Address2;
            Pincode = address.ZipPostalCode;
            CountryId = address.CountryId.GetValueOrDefault();
            StateProvinceId = address.StateProvinceId.GetValueOrDefault();
        }

        //Get address id
        public int Id { get; set; }

        public int OrderId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid First Name")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid Last Name")]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid City Name")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Pincode")]
        public string Pincode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid State Name")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("/^[a-z ,.'-]+$/i", ErrorMessage = "Invalid Country Name")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public int CountryId { get; set; }

        public int StateProvinceId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
       
    }
}