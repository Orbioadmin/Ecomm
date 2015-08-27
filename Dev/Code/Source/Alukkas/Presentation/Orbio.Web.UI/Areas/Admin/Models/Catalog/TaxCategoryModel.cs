using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class TaxCategoryModel
    {
        public TaxCategoryModel()
        { 
        }
        public TaxCategoryModel(TaxCategory tax)
        {
            Id = tax.Id;
            Name = tax.Name;
            DisplayOrder = tax.DisplayOrder;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}