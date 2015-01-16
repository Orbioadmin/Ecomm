using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public class BaseCatalogModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeName { get; set; }
        public string ViewPath { get; set; }

    }
}