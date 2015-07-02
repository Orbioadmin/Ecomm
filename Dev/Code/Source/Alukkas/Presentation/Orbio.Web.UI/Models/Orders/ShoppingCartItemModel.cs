using Nop.Core.Infrastructure;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Discounts;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Orders;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbio.Web.UI.Models.Orders
{
    public class ShoppingCartItemModel : ProductDetailModel, IShoppingCartItem
    {
        
        public ShoppingCartItemModel()
        {
            //this.items = new List<ShoppingCartItemModel>();
           
        }
        public ShoppingCartItemModel(ShoppingCartItem productDetail)
            : base(productDetail)
        {
            
            //this.items = new List<ShoppingCartItemModel>();
            this.ItemCount = productDetail.ItemCount;
            this.SelectedQuantity = productDetail.Quantity.ToString();

            //this.Id = productDetail.Id;
            //this.Name = productDetail.Name;
            //this.SeName = productDetail.SeName;
            //this.ImageRelativeUrl = productDetail.ImageRelativeUrl;
            //this.CurrencyCode = productDetail.CurrencyCode;
            //this.ProductPrice.Price = productDetail.Price.ToString("0.00");
           
            //this.TotalPrice = (Convert.ToDecimal(ProductPrice.Price) * productDetail.Quantity).ToString("#,##0.00");
            var priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
            //this.ProductPrice.Price = priceCalculationService.GetFinalPrice(this, false, false).ToString("#,##0.00");
            this.ProductPrice.OldPrice = priceCalculationService.GetUnitPrice(this).ToString("#,##0.00");
            this.TotalPrice = priceCalculationService.GetFinalPrice(this, false, true).ToString("#,##0.00");
            this.CartId = productDetail.CartId;
            //if (productDetail.ProductPictures != null && productDetail.ProductPictures.Count > 0)
            //{
            //    var baseUrl = ConfigurationManager.AppSettings["ImageServerBaseUrl"];
            //    this.ProductPictures = (from pp in productDetail.ProductPictures
            //                            select new PictureModel
            //                            {
            //                                ImageUrl = pp.RelativeUrl != null ? baseUrl + GetThumbImageFileName(pp.RelativeUrl) : string.Empty,
            //                                FullSizeImageUrl = pp.RelativeUrl != null ? baseUrl + pp.RelativeUrl : string.Empty,
            //                                AlternateText = productDetail.Name,
            //                                Title = productDetail.ShortDescription
            //                            }).ToList();

            //    this.DefaultPicture = this.ProductPictures.First();
            //}
            //else
            //{
            //    //TODO: set default picture
            //    if (this.ProductPictures == null || this.ProductPictures.Count == 0)
            //    {
            //        this.ProductPictures = new List<PictureModel>();
            //        this.DefaultPicture = new PictureModel();
            //    }
            //}

            //this.AllowedQuantities = new List<int>();
            //if (!String.IsNullOrWhiteSpace(productDetail.AllowedQuantities))
            //{
            //    productDetail.AllowedQuantities
            //       .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //       .ToList()
            //       .ForEach(qtyStr =>
            //       {
            //           int qty = 0;
            //           if (int.TryParse(qtyStr.Trim(), out qty))
            //           {
            //               this.AllowedQuantities.Add(qty);
            //           }
            //       });
            //}

            //this.OrderMaximumQuantity = productDetail.OrderMaximumQuantity;

            //this.OrderMinimumQuantity = productDetail.OrderMinimumQuantity;

           
            this.IsRemove = false;
        }
        public int ItemCount { get; set; }
        //public string TotalPrice { get; set; }

        public int CartId { get; set; }

        public bool IsRemove { get; set; }

        public string TotalPrice{get;set;}

        public string UnitPrice
        {
            get
            {
                var priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
                return priceCalculationService.GetFinalPrice(this, false, false).ToString("#,##0.00");
            }
        }

        decimal IShoppingCartItem.Price
        {
            get
            {
                return Convert.ToDecimal(this.ProductPrice.Price);
            }
        }


        IEnumerable<IDiscount> IShoppingCartItem.Discounts
        {
            get { return this.Discounts; }
        }


        int IShoppingCartItem.Quantity
        {
            get { return Convert.ToInt32(this.SelectedQuantity); }
        }


        IEnumerable<decimal> IShoppingCartItem.ProductVariantPriceAdjustments
        {
            get
            {
                return (from pva in this.ProductVariantAttributes
                        from pvav in pva.Values
                        select Convert.ToDecimal(pvav.PriceAdjustment)).ToList();
            }
        }

        int IShoppingCartItem.TaxCategoryId
        {
            get { return this.TaxCategoryId; }
        }

        decimal IShoppingCartItem.FinalPrice
        {
            get
            {
                var priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
                return priceCalculationService.GetFinalPrice(this, true, true);
            }
        }
    }
}