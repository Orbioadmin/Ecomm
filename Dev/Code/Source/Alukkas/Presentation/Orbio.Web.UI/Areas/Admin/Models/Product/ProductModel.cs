using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Product
{
    public class ProductModel
    {
        public ProductModel()
        {
            AvailableDeliveryDates = new List<SelectListItem>();
            AvailableWarehouses = new List<SelectListItem>();
            AvailableTaxCategories = new List<SelectListItem>();
        }
        public ProductModel(Orbio.Core.Data.Product products)
        {
            Id = products.Id;
            Name = products.Name;
            ImageUrl = GetImageRelativeUrl(products);
            Sku = products.Sku;
            Price = products.Price;
            StockQuantity = products.StockQuantity;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        [AllowHtml]
        public string FullDescription { get; set; }
        public string AdminComment { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public bool AllowCustomerReviews { get; set; }
        public string ProductTags { get; set; }
        public int ApprovedRatingSum { get; set; }
        public int NotApprovedRatingSum { get; set; }
        public int ApprovedTotalReviews { get; set; }
        public int NotApprovedTotalReviews { get; set; }
        public bool SubjectToAcl { get; set; }
        public bool LimitedToStores { get; set; }
        public string Sku { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public bool IsShipEnabled { get; set; }
        public bool IsFreeShipping { get; set; }
        public decimal AdditionalShippingCharge { get; set; }
        public int DeliveryDateId { get; set; }
        public int WarehouseId { get; set; }
        public bool IsTaxExempt { get; set; }
        public int TaxCategoryId { get; set; }
        public int ManageInventoryMethodId { get; set; }
        public int StockQuantity { get; set; }
        public bool DisplayStockAvailability { get; set; }
        public bool DisplayStockQuantity { get; set; }
        public int MinStockQuantity { get; set; }
        public int LowStockActivityId { get; set; }
        public int NotifyAdminForQuantityBelow { get; set; }
        public int BackorderModeId { get; set; }
        public bool AllowBackInStockSubscriptions { get; set; }
        public int OrderMinimumQuantity { get; set; }
        public int OrderMaximumQuantity { get; set; }
        public string AllowedQuantities { get; set; }
        public bool CallForPrice { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal ProductCost { get; set; }
        public Nullable<decimal> SpecialPrice { get; set; }
        public Nullable<System.DateTime> SpecialPriceStartDateTimeUtc { get; set; }
        public Nullable<System.DateTime> SpecialPriceEndDateTimeUtc { get; set; }
        public bool HasDiscountsApplied { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public Nullable<System.DateTime> AvailableStartDateTimeUtc { get; set; }
        public Nullable<System.DateTime> AvailableEndDateTimeUtc { get; set; }
        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public Nullable<decimal> ProductUnit { get; set; }
        public string ImageUrl { get; set; }

        public List<SelectListItem> AvailableDeliveryDates { get; set; }
        public List<SelectListItem> AvailableWarehouses { get; set; }
        public List<SelectListItem> AvailableTaxCategories { get; set; }

        public string GetImageRelativeUrl(Orbio.Core.Data.Product products)
        {
            var baseUrl = ConfigurationManager.AppSettings["ImageServerBaseUrl"];
            string url = (from pic in products.Product_Picture_Mapping.Select(p => p.Picture)
                          select pic.RelativeUrl).FirstOrDefault();
            return baseUrl + url;
        }
    }
}