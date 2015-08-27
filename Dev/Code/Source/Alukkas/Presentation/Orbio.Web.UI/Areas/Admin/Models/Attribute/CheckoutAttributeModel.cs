using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
using Orbio.Web.UI.Areas.Admin.Models.Catalog;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class CheckoutAttributeModel
    {
        public CheckoutAttributeModel()
        {
            TaxCategory = new List<TaxCategoryModel>();
            CheckoutAttributeValue = new List<CheckoutAttributeValueModel>();
        }

        public CheckoutAttributeModel(CheckoutAttribute checkout)
        {
            Id = checkout.Id;
            Name = checkout.Name;
            TextPrompt = checkout.TextPrompt;
            IsRequired = checkout.IsRequired;
            ShippableProduct = checkout.ShippableProductRequired;
            IsTaxExempt = checkout.IsTaxExempt;
            TaxCategoryId = checkout.TaxCategoryId;
            DisplayOrder = checkout.DisplayOrder;
            ControlTypeId = checkout.AttributeControlTypeId;
            ControlType = Enum.Parse(typeof(AttributeControlType), checkout.AttributeControlTypeId.ToString()).ToString();

            CheckoutAttributeValue = (from C in checkout.CheckoutAttributeValues
                                      select new CheckoutAttributeValueModel(C)).ToList();
        }

        public CheckoutAttributeModel(List<TaxCategory> tax)
        {
            TaxCategory = (from T in tax select new TaxCategoryModel(T)).ToList();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string TextPrompt { get; set; }

        public bool IsRequired { get; set; }

        public bool ShippableProduct { get; set; }

        public bool IsTaxExempt { get; set; }

        public int TaxCategoryId { get; set; }

        public int DisplayOrder { get; set; }

        public string ControlType { get; set; }

        public int ControlTypeId { get; set; }


        public List<CheckoutAttributeValueModel> CheckoutAttributeValue { get; set; }

        public List<TaxCategoryModel> TaxCategory { get; set; }
    }
}