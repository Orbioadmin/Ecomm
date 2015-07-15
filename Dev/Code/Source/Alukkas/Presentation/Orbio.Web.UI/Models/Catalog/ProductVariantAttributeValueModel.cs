using Orbio.Core.Domain.Catalog.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public partial class ProductVariantAttributeValueModel : IProductAttributeValue
    {
        public ProductVariantAttributeValueModel()
        {
        }

       
        
        public int Id { get; set; }
        public string Name { get; set; }

        public string ColorSquaresRgb { get; set; }

        public decimal PriceAdjustment { get; set; }

        public decimal PriceAdjustmentValue { get; set; }

        public bool IsPreSelected { get; set; }

        public int DisplayOrder { get; set; }

        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string FullSizePictureUrl { get; set; }

        decimal IProductAttributeValue.PriceAdjustment
        {
            get { return this.PriceAdjustment; }
        }

        string IProductAttributeValue.AttributeValue
        {
            get { return this.Name; }
        }
    }
}