using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Shipments
{
    public class ShipmentListModel
    {
        public ShipmentListModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [AllowHtml]
        public string TrackingNumber { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }
      
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableStates { get; set; }
       
        public int StateProvinceId { get; set; }

        [AllowHtml]
        public string City { get; set; }
  
    }
}