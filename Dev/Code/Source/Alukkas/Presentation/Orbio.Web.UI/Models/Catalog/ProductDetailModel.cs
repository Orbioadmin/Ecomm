using Nop.Core.Domain.Catalog;
using Orbio.Core.Domain.Catalog;
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
        public ProductDetailModel(ProductDetail ProductDetail)
            : this()
        {
            this.Id = ProductDetail.Id;
            this.Name = ProductDetail.Name;
            this.ShortDescription = ProductDetail.ShortDescription;
            this.FullDescription = ProductDetail.FullDescription;
            this.SeName = ProductDetail.SeName;
            this.ViewPath = ProductDetail.ViewPath;
            this.ImageRelativeUrl = ProductDetail.ImageRelativeUrl;
            this.CurrencyCode = ProductDetail.CurrencyCode;
            this.ProductPrice.Price = ProductDetail.Price.ToString("0.00");
            if (ProductDetail.SpecificationFilters != null && ProductDetail.SpecificationFilters.Count > 0)
            {
                var specFilterByspecAttribute = from sa in ProductDetail.SpecificationFilters
                                                group sa by sa.SpecificationAttributeName;
                //var currentUrl = ControllerContext.RequestContext.HttpContext.Request.Url.AbsoluteUri;

                //var specs = selectedSpecs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); 
                this.SpecificationAttributes = (from sag in specFilterByspecAttribute
                             select new SpecificationAttribute
                             {
                                 Type = "Specification",
                                 Name = sag.Key,
                                 SpecificationAttributeOptions =
                                     new List<SpecificationAttributeOption>((from sao in sag
                                                                             select new SpecificationAttributeOption
                                                                             {
                                                                                 Id = sao.SpecificationAttributeOptionId,
                                                                                 Name = sao.SpecificationAttributeOptionName,
                                                                                 //FilterUrl = currentUrl,
                                                                                 //Selected = selectedSpecs != null && selectedSpecs.Length > 0 && selectedSpecs.Any(i => i == sao.SpecificationAttributeOptionId)
                                                                             }))
                             }).ToList();
            }
        }
        public List<ProductVariantAttributeModel> ProductVariantAttributes { get; private set; }

        public List<SpecificationAttribute> SpecificationAttributes { get; private set; }
    }
}