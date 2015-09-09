using Orbio.Web.UI.Areas.Admin.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Orbio.Web.UI.Areas.Admin.Models.Product
{
    public class ProductListModel
    {
        public ProductListModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
        }
        public string SearchProductNameOrSku { get; set; }
        public int CategoryId { get; set; }
        public int ManufactureId { get; set; }
        public List<SelectListItem> AvailableCategories { get; set; }
        public List<SelectListItem> AvailableManufacturers { get; set; }

    }
}