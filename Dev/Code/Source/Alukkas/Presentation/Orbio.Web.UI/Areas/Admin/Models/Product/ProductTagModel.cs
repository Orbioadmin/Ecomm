using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Product
{
    public class ProductTagModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductCount { get; set; }
    }
}