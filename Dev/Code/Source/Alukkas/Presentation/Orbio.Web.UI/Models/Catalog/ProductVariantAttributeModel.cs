using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Catalog.Abstract;

namespace Orbio.Web.UI.Models.Catalog
{
    public partial class ProductVariantAttributeModel : IProductAttribute
    {
        public ProductVariantAttributeModel()
        {
            AllowedFileExtensions = new List<string>();
            Values = new List<ProductVariantAttributeValueModel>();
        }

       

        public int Id { get; set; }

        public int ProductAttributeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TextPrompt { get; set; }

        public string SizeGuideUrl { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Selected value for textboxes
        /// </summary>
        public string TextValue { get; set; }
        /// <summary>
        /// Selected day value for datepicker
        /// </summary>
        public int? SelectedDay { get; set; }
        /// <summary>
        /// Selected month value for datepicker
        /// </summary>
        public int? SelectedMonth { get; set; }
        /// <summary>
        /// Selected year value for datepicker
        /// </summary>
        public int? SelectedYear { get; set; }
        /// <summary>
        /// Allowed file extensions for customer uploaded files
        /// </summary>
        public IList<string> AllowedFileExtensions { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<ProductVariantAttributeValueModel> Values { get; set; }




        string IProductAttribute.AttributeName
        {
            get { return this.TextPrompt; }
        }

        List<IProductAttributeValue> IProductAttribute.ProductAttributeValues
        {
            get { return this.Values.ToList<IProductAttributeValue>(); }
        }
    }
}