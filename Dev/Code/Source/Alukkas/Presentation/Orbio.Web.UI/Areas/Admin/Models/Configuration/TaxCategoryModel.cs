using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Configuration
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
            TaxRate = (tax.TaxRates != null) ? (from rate in tax.TaxRates
                                                where rate.TaxCategoryId == tax.Id
                                                select rate.Percentage).FirstOrDefault() : 0;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public decimal TaxRate { get; set; }
    }
}