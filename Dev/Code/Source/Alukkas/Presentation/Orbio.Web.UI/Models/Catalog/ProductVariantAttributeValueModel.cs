using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public partial class ProductVariantAttributeValueModel 
    {
        public string Name { get; set; }

        public string ColorSquaresRgb { get; set; }

        public string PriceAdjustment { get; set; }

        public decimal PriceAdjustmentValue { get; set; }

        public bool IsPreSelected { get; set; }

        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string FullSizePictureUrl { get; set; }
    }
}