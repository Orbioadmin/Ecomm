using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Attributes
{
    public class ProductAttributeModel
    {
        public ProductAttributeModel()
        {
        }

        public ProductAttributeModel(ProductAttribute p)
        {
            Id = p.Id;
            Name = p.Name;
            Description = p.Description;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }
    }
}