using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Discounts;
using Orbio.Services.Orders;
using Orbio.Services.Taxes;
using System;
using System.Collections.Generic;

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
            this.GoldWeight = product.GoldWeight;
            this.ProductUnit = product.ProductUnit;
            this.ProductPrice.Price = product.CalculatePrice();// product.Price.ToString("#,##0.00");
            this.ProductPrice.OldPrice = product.OldPrice.ToString("#,##0.00");
            this.ComponentDetails = product.GetComponentDetails();
            this.Discounts = product.Discounts == null ? new List<Discount>() : product.Discounts;
        
            this.TaxCategoryId = product.TaxCategoryId;
            var taxCalculator = EngineContext.Current.Resolve<ITaxCalculationService>();
            this.ProductPrice.TaxAmount = taxCalculator.CalculateTax(Convert.ToDecimal(this.ProductPrice.Price), this.TaxCategoryId, EngineContext.Current.Resolve<IWorkContext>().CurrentCustomer);
            if (this.ComponentDetails == null)
            {
                this.ComponentDetails = new Dictionary<string, string>();
            }

            this.ComponentDetails.Add("Taxes", this.ProductPrice.TaxAmount.ToString("#,##0.00"));

        }

      

        
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
       
        public string ImageRelativeUrl { get; set; }
        public string CurrencyCode { get; set; }
        public decimal GoldWeight { get; set; }
        public decimal ProductUnit { get; set; }
        public Dictionary<string, string> ComponentDetails { get; set; }
        //price
        public ProductPriceModel ProductPrice { get; set; }

        public int TaxCategoryId { get; set; }

        public List<Discount> Discounts { get; set; }

        public decimal DiscountAmount
        {
            get
            {
                if (this.Discounts != null && this.Discounts.Count > 0)
                {
                    var priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
                    return priceCalculationService.GetDiscountAmount(this.Discounts, Convert.ToDecimal(this.ProductPrice.Price));
                }

                return decimal.Zero;
            }
        }
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
            public decimal Price {get;set;}
            public decimal Discount { get; set; }
            public bool DisableBuyButton { get; set; }
            public bool DisableWishlistButton { get; set; }
            public decimal TaxAmount { get; set; }

            public bool AvailableForPreOrder { get; set; }
            public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

            public bool ForceRedirectionAfterAddingToCart { get; set; }
        }

		#endregion

     
    }
}