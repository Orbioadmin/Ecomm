using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public class ProductDetailModel : ProductOverViewModel
    {
        public ProductDetailModel()
        {
            this.ProductVariantAttributes = new List<ProductVariantAttributeModel>();
            this.SpecificationAttributes = new List<SpecificationAttribute>();
        }
        public List<ProductVariantAttributeModel> ProductVariantAttributes { get; private set; }

        public List<SpecificationAttribute> SpecificationAttributes { get; private set; }
    }
}