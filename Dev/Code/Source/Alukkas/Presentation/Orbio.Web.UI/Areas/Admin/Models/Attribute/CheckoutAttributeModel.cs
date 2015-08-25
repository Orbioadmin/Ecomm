using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class CheckoutAttributeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TextPrompt { get; set; }

        public bool IsRequired { get; set; }

        public bool ShippableProduct { get; set; }

        public bool IsTaxExempt { get; set; }

        public int TaxCategoryId { get; set; }

        public int TaxCategory { get; set; }

        public int DisplayOrder { get; set; }

        public string ControlType { get; set; }

        public List<CheckoutAttributeValueModel> CheckoutAttributeValue { get; set; }
    }
}