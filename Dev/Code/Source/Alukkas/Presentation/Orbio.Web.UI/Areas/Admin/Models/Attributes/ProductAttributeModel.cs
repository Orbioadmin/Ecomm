using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class ProductAttributeModel
    {
        public ProductAttributeModel()
        {
        }

        public ProductAttributeModel(ProductAttribute p)
        {
            Name = p.Name;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}