using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Checkout;
using Orbio.Web.UI.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Orders
{
    public partial class OrderModel
    {
        public OrderModel()
        {
            TaxRates = new List<TaxRate>();
            GiftCards = new List<GiftCard>();
            Items = new List<OrderItemModel>();
            UsedDiscounts = new List<UsedDiscountModel>();
            OrderNotes = new List<OrderNote>();
            AvailableShippingMethods = new List<SelectListItem>();
        }

        public bool IsLoggedInAsVendor { get; set; }

        //identifiers

        public int Id { get; set; }

        public Guid OrderGuid { get; set; }

        //customer info

        public int CustomerId { get; set; }

        public string CustomerInfo { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerIp { get; set; }

        public int? AffiliateId { get; set; }

        //Used discounts

        public IList<UsedDiscountModel> UsedDiscounts { get; set; }

        //totals
        public bool AllowCustomersToSelectTaxDisplayType { get; set; }
        //public TaxDisplayType TaxDisplayType { get; set; }

        public string OrderSubtotalInclTax { get; set; }

        public string OrderSubtotalExclTax { get; set; }

        public string OrderSubTotalDiscountInclTax { get; set; }

        public string OrderSubTotalDiscountExclTax { get; set; }

        public string OrderShippingInclTax { get; set; }

        public string OrderShippingExclTax { get; set; }

        public string PaymentMethodAdditionalFeeInclTax { get; set; }

        public string PaymentMethodAdditionalFeeExclTax { get; set; }

        public string Tax { get; set; }
        public IList<TaxRate> TaxRates { get; set; }
        public bool DisplayTax { get; set; }
        public bool DisplayTaxRates { get; set; }

        public string OrderTotalDiscount { get; set; }

        public int RedeemedRewardPoints { get; set; }

        public string RedeemedRewardPointsAmount { get; set; }

        public string OrderTotal { get; set; }

        public string RefundedAmount { get; set; }

        //edit totals

        public decimal OrderSubtotalInclTaxValue { get; set; }

        public decimal OrderSubtotalExclTaxValue { get; set; }

        public decimal OrderSubTotalDiscountInclTaxValue { get; set; }

        public decimal OrderSubTotalDiscountExclTaxValue { get; set; }

        public decimal OrderShippingInclTaxValue { get; set; }

        public decimal OrderShippingExclTaxValue { get; set; }

        public decimal PaymentMethodAdditionalFeeInclTaxValue { get; set; }

        public decimal PaymentMethodAdditionalFeeExclTaxValue { get; set; }

        public decimal TaxValue { get; set; }

        public string TaxRatesValue { get; set; }

        public decimal OrderTotalDiscountValue { get; set; }

        public string OrderTotalAdjustment { get; set; }

        [DisplayName("Free Shipping")]
        public bool OrderFreeShipping { get; set; }

        public decimal OrderTotalValue { get; set; }

        //associated recurring payment id

        public int RecurringPaymentId { get; set; }

        //order status

        public string OrderStatus { get; set; }

        public int OrderStatusId { get; set; }

        //payment info

        public string PaymentStatus { get; set; }

        public string PaymentMethod { get; set; }

        //credit card info
        public bool AllowStoringCreditCardNumber { get; set; }

        public string CardType { get; set; }

        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string CardCvv2 { get; set; }

        public string CardExpirationMonth { get; set; }

        public string CardExpirationYear { get; set; }

        //misc payment info
        public bool DisplayPurchaseOrderNumber { get; set; }

        public string PurchaseOrderNumber { get; set; }

        public string AuthorizationTransactionId { get; set; }

        public string CaptureTransactionId { get; set; }

        public string SubscriptionTransactionId { get; set; }

        //shipping info
        public bool IsShippable { get; set; }

        public string ShippingStatus { get; set; }

        public int ShippingStatusId { get; set; }

        public Address ShippingAddress { get; set; }

        public string ShippingMethod { get; set; }

        public string ShippingAddressGoogleMapsUrl { get; set; }
        public bool CanAddNewShipments { get; set; }

        public Shipping shipping { get; set; }

       

        //billing info

        public Address BillingAddress { get; set; }

        public string VatNumber { get; set; }

        //gift cards
        public IList<GiftCard> GiftCards { get; set; }

        //items
        public bool HasDownloadableProducts { get; set; }
        public IList<OrderItemModel> Items { get; set; }

        //creation date

        public DateTime CreatedOn { get; set; }

        //checkout attributes
        public string CheckoutAttributeInfo { get; set; }


        //order notes
        public List<OrderNote> OrderNotes { get; set; }

        public string AddOrderNoteMessage { get; set; }

        public bool AddOrderNoteDisplayToCustomer { get; set; }
        //refund info

        public decimal AmountToRefund { get; set; }
        public decimal MaxAmountToRefund { get; set; }
        public string PrimaryStoreCurrencyCode { get; set; }

        //aggergator properties
        public string aggregatorprofit { get; set; }
        public string aggregatorshipping { get; set; }
        public string aggregatortax { get; set; }
        public string aggregatortotal { get; set; }

        public List<SelectListItem> AvailableShippingMethods { get; set; }

        #region Nested Classes

        public partial class OrderItemModel
        {
            public OrderItemModel()
            {
                ReturnRequestIds = new List<int>();
                PurchasedGiftCardIds = new List<int>();
            }
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string VendorName { get; set; }
            public string Sku { get; set; }
            public string ImageUrl { get; set; }

            public string UnitPriceInclTax { get; set; }
            public string UnitPriceExclTax { get; set; }
            public decimal UnitPriceInclTaxValue { get; set; }
            public decimal UnitPriceExclTaxValue { get; set; }

            public int Quantity { get; set; }

            public string DiscountInclTax { get; set; }
            public string DiscountExclTax { get; set; }
            public decimal DiscountInclTaxValue { get; set; }
            public decimal DiscountExclTaxValue { get; set; }

            public string SubTotalInclTax { get; set; }
            public string SubTotalExclTax { get; set; }
            public decimal SubTotalInclTaxValue { get; set; }
            public decimal SubTotalExclTaxValue { get; set; }

            public string AttributeInfo { get; set; }
            public string RecurringInfo { get; set; }
            public IList<int> ReturnRequestIds { get; set; }
            public IList<int> PurchasedGiftCardIds { get; set; }

            public bool IsDownload { get; set; }
            public int DownloadCount { get; set; }
            //public DownloadActivationType DownloadActivationType { get; set; }
            public bool IsDownloadActivated { get; set; }
            public Guid LicenseDownloadGuid { get; set; }
            public bool IsGift { get; set; }
        }

        public partial class TaxRate
        {
            public string Rate { get; set; }
            public string Value { get; set; }
        }

        public partial class GiftCard
        {
            public string CouponCode { get; set; }
            public string Amount { get; set; }
        }

        public partial class OrderNote
        {
            public int Id { get; set; }

            public int OrderId { get; set; }

            public bool DisplayToCustomer { get; set; }

            public string Note { get; set; }

            public DateTime CreatedOn { get; set; }
        }

        public partial class Shipping
        {
            public int Id { get; set; }

            public string TrackingNumber { get; set; }

            public decimal? TotalWeight { get; set; }

            public string Comment { get; set; }

            public DateTime? DateShipped { get; set; }

            public DateTime? DateDelivered { get; set; }

            public DateTime CreatedOn { get; set; }
        }

        public partial class UploadLicenseModel
        {
            public int OrderId { get; set; }

            public int OrderItemId { get; set; }

            public int LicenseDownloadId { get; set; }

        }

        public partial class AddOrderProductModel
        {
            public AddOrderProductModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }


            public string SearchProductName { get; set; }

            public int SearchCategoryId { get; set; }

            public int SearchManufacturerId { get; set; }

            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public int OrderId { get; set; }

            #region Nested classes

            public partial class ProductModel
            {
                public string Name { get; set; }

                public string Sku { get; set; }
            }

            public partial class ProductDetailsModel
            {
                public ProductDetailsModel()
                {
                    ProductVariantAttributes = new List<ProductVariantAttributeModel>();
                    GiftCard = new GiftCardModel();
                    Warnings = new List<string>();
                }

                public int ProductId { get; set; }

                public int OrderId { get; set; }

                //public ProductType ProductType { get; set; }

                public string Name { get; set; }

                public decimal UnitPriceInclTax { get; set; }

                public decimal UnitPriceExclTax { get; set; }

                public int Quantity { get; set; }

                public decimal SubTotalInclTax { get; set; }

                public decimal SubTotalExclTax { get; set; }

                //product attrbiutes
                public IList<ProductVariantAttributeModel> ProductVariantAttributes { get; set; }
                //gift card info
                public GiftCardModel GiftCard { get; set; }

                public List<string> Warnings { get; set; }

            }

            public partial class ProductVariantAttributeModel
            {
                public ProductVariantAttributeModel()
                {
                    Values = new List<ProductVariantAttributeValueModel>();
                }

                public int ProductAttributeId { get; set; }

                public string Name { get; set; }

                public string TextPrompt { get; set; }

                public bool IsRequired { get; set; }

                public AttributeControlType AttributeControlType { get; set; }

                public IList<ProductVariantAttributeValueModel> Values { get; set; }
            }

            public partial class ProductVariantAttributeValueModel
            {
                public string Name { get; set; }

                public bool IsPreSelected { get; set; }
            }


            public partial class GiftCardModel
            {
                public bool IsGiftCard { get; set; }

                public string RecipientName { get; set; }

                public string RecipientEmail { get; set; }

                public string SenderName { get; set; }

                public string SenderEmail { get; set; }

                public string Message { get; set; }

                //public GiftCardType GiftCardType { get; set; }
            }
            #endregion
        }

        public partial class UsedDiscountModel 
        {
            public int DiscountId { get; set; }
            public string DiscountName { get; set; }
        }

        #endregion
    }
}