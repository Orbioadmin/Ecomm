using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Web.UI.Models.Catalog
{
    public class ProductDetailModel : ProductOverViewModel
    {
        public ProductDetailModel()
        {
            this.ProductVariantAttributes = new List<ProductVariantAttributeModel>();
            this.SpecificationAttributes = new List<SpecificationAttribute>();
        }

        public ProductDetailModel(ProductDetail productDetail):base(productDetail)            
        {
           
       
            if (productDetail.BreadCrumbs != null && productDetail.BreadCrumbs.Count > 0)
            {
                this.BreadCrumbs = (from c in productDetail.BreadCrumbs
                                    select new CategoryModel { Name = c.Name, SeName = c.SeName }).ToList();
            }
            
            if (productDetail.SpecificationFilters != null && productDetail.SpecificationFilters.Count > 0)
            {
                var specFilterByspecAttribute = from sa in productDetail.SpecificationFilters
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

            if (productDetail.ProductPictures != null && productDetail.ProductPictures.Count > 0)
            {
                var baseUrl = ConfigurationManager.AppSettings["ImageServerBaseUrl"];
                this.ProductPictures = (from pp in productDetail.ProductPictures
                                        select new PictureModel
                                        {
                                            ImageUrl = pp.RelativeUrl != null ? baseUrl + GetThumbImageFileName(pp.RelativeUrl) : string.Empty,
                                            FullSizeImageUrl = pp.RelativeUrl != null ? baseUrl + pp.RelativeUrl : string.Empty,
                                            AlternateText = productDetail.Name,
                                            Title = productDetail.ShortDescription
                                        }).ToList();

                this.DefaultPicture = this.ProductPictures.First();
            }

            this.StockAvailability = productDetail.FormatStockMessage();
            this.IsFreeShipping = productDetail.IsFreeShipping;
            this.IsShipEnabled = productDetail.IsShipEnabled;
            this.DeliveredIn = productDetail.DeliveredIn;
            
        }
        public List<ProductVariantAttributeModel> ProductVariantAttributes { get; private set; }

        public List<SpecificationAttribute> SpecificationAttributes { get; private set; }

        public string DeliveredIn { get; set; }

        public IList<CategoryModel> BreadCrumbs { get; set; }

        public IList<PictureModel> ProductPictures { get; set; }

        public PictureModel DefaultPicture { get; set; }

        public string StockAvailability { get; set; }

        public bool IsShipEnabled { get; set; }

        public bool IsFreeShipping { get; set; }

        private static string GetThumbImageFileName(string imageUrl)
        {
            var fileName = imageUrl;
            if (imageUrl.IndexOf('?') > 0)
            {
                fileName = imageUrl.Substring(0, imageUrl.IndexOf('?'));
            }

            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1, fileName.Length - fileName.LastIndexOf('/') - 1);
            fileName = Path.GetFileNameWithoutExtension(fileName);

            return imageUrl.Replace(fileName, fileName + "_tb");
        }
    }
}