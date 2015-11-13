using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            AvailableProductTags = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailableManufatures = new List<SelectListItem>();
            AvailableRelatedProducts = new List<SelectListItem>();
            AvailableSimilarProducts = new List<SelectListItem>();
            AvailableDiscounts = new List<SelectListItem>();
            Pictures = new List<Product_Picture_Mapping>();
            AddSpecificationAttributeModel = new AddProductSpecificationAttributeModel();
            AddVariantAttributeModel = new AddProductVariantAttributeModel();
            AddVariantAttributeValueModel = new AddProductVariantAttributeValueModel();
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
        public string SeName { get; set; }
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
        public decimal SpecialPrice { get; set; }
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
        public System.DateTime? CreatedOnUtc { get; set; }
        public System.DateTime? UpdatedOnUtc { get; set; }
        public decimal ProductUnit { get; set; }
        public string ImageUrl { get; set; }
        public bool IsGift { get; set; }
        public decimal GiftCharge { get; set; }

        public List<SelectListItem> AvailableDeliveryDates { get; set; }
        public List<SelectListItem> AvailableWarehouses { get; set; }
        public List<SelectListItem> AvailableTaxCategories { get; set; }
        public List<SelectListItem> AvailableProductTags { get; set; }
        public List<SelectListItem> AvailableCategories { get; set; }
        public List<SelectListItem> AvailableManufatures { get; set; }
        public List<SelectListItem> AvailableRelatedProducts { get; set; }
        public List<SelectListItem> AvailableSimilarProducts { get; set; }
        public List<SelectListItem> AvailableDiscounts { get; set; }
        public List<Product_Picture_Mapping> Pictures { get; set; }
        public List<Product_SpecificationAttribute_Mapping> ProductSpecification { get; set; }
        public List<Product_ProductAttribute_Mapping> ProductVariantAttribute { get; set; }
        public int[] SelectedProductTags { get; set; }
        public int[] SelectedCategories { get; set; }
        public int[] SelectedManufature { get; set; }
        public int[] SelectedRelatedProducts { get; set; }
        public int[] SelectedSimilarProducts { get; set; }
        public int[] SelectedDiscounts { get; set; }
        public ProductPictureModel PictureModel { get; set; }
        //add specification attribute model
        public AddProductSpecificationAttributeModel AddSpecificationAttributeModel { get; set; }
        //add product variant attribute model
        public AddProductVariantAttributeModel AddVariantAttributeModel { get; set; }
        //add product variant attribute value model
        public AddProductVariantAttributeValueModel AddVariantAttributeValueModel { get; set; }
        public List<ProductVariantAttributeValue> ProductVariantAttributeValue { get; set; }

        #region Nested classes
        public partial class ProductPictureModel 
        {
            public int ProductId { get; set; }

            public int PictureId { get; set; }

            public string PictureUrl { get; set; }

            public string ProductSizeGuideUrl { get; set; }

            public int DisplayOrder { get; set; }
        }
        public partial class AddProductSpecificationAttributeModel
        {
            public AddProductSpecificationAttributeModel()
            {
                AvailableAttributes = new List<SelectListItem>();
                AvailableOptions = new List<SelectListItem>();
            }
            public int SpecificationAttributeId { get; set; }

            public int SpecificationAttributeOptionId { get; set; }

            public string SubTitle { get; set; }

            public string CustomValue { get; set; }

            public bool AllowFiltering { get; set; }

            public bool ShowOnProductPage { get; set; }

            public int DisplayOrder { get; set; }

            public IList<SelectListItem> AvailableAttributes { get; set; }
            public IList<SelectListItem> AvailableOptions { get; set; }
        }
        public partial class AddProductVariantAttributeModel
        {
            public AddProductVariantAttributeModel()
            {
                AvailableProductAttributes = new List<SelectListItem>();
            }

            public int ProductAttributeId { get; set; }

            [AllowHtml]
            public string TextPrompt { get; set; }

            public string SizeGuideUrl { get; set; }

            public bool IsRequired { get; set; }

            public int AttributeControlTypeId { get; set; }

            public string AttributeControlType { get; set; }

            public IList<SelectListItem> AvailableProductAttributes { get; set; }

            public string ViewEditUrl { get; set; }
            public string ViewEditText { get; set; }
            public string ViewEditSizeGuide { get; set; }

        }
        public partial class AddProductVariantAttributeValueModel 
        {
            public int ProductVariantAttributeId { get; set; }

            public int AttributeValueTypeId { get; set; }

            public string Name { get; set; }

            public bool DisplayColorSquaresRgb { get; set; }

            public decimal PriceAdjustment { get; set; }

            public decimal WeightAdjustment { get; set; }

            public decimal Cost { get; set; }

            public bool IsPreSelected { get; set; }

            public int DisplayOrder { get; set; }
        }

        #endregion
    }
}