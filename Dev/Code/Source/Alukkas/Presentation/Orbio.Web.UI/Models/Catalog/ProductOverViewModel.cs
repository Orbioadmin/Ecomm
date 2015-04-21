using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Web.UI.Models.Catalog
{
    public class ProductOverViewModel : BaseCatalogModel
    {
        public ProductOverViewModel()
        {
            ProductPrice = new ProductPriceModel();
           // DefaultPictureModel = new PictureModel();
            //SpecificationAttributeModels = new List<ProductSpecificationModel>();
            //ReviewOverviewModel = new ProductReviewOverviewModel();
        }

        public ProductOverViewModel(Product product):this()
        {
            //TODO: throw custom exception and show not found page
            if (product == null)
            {
                throw new Exception("Page not found");
            }
            // TODO: Complete member initialization
            this.Id = product.Id;
            this.Name = product.Name;
            this.ShortDescription = product.ShortDescription;
            this.FullDescription = product.FullDescription;
            this.SeName = product.SeName;
            this.ViewPath = product.ViewPath;
            this.ImageRelativeUrl = product.ImageRelativeUrl;
            this.CurrencyCode = product.CurrencyCode;
            this.ProductPrice.Price = product.CalculatePrice();// product.Price.ToString("#,##0.00");
            this.ProductPrice.OldPrice = product.OldPrice.ToString("#,##0.00");
        }

      

        
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
       
        public string ImageRelativeUrl { get; set; }
        public string CurrencyCode { get; set; }
        //price
        public ProductPriceModel ProductPrice { get; set; }
        ////picture
        //public PictureModel DefaultPictureModel { get; set; }
        ////specification attributes
        //public IList<ProductSpecificationModel> SpecificationAttributeModels { get; set; }
        ////price
        //public ProductReviewOverviewModel ReviewOverviewModel { get; set; }
       
		#region Nested Classes

        public partial class ProductPriceModel
        {
            public string OldPrice { get; set; }
            public string Price {get;set;}
            public decimal Discount { get; set; }
            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }

            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            public bool ForceRedirectionAfterAddingToCart { get; set; }
        }

		#endregion

     
    }
}