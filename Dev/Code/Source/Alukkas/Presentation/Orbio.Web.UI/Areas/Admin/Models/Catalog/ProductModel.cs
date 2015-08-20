using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsFeaturedProduct { get; set; }

        public int DisplayOrder { get; set; }
    }
}