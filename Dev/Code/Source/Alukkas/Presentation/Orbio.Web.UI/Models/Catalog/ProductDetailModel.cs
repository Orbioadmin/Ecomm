using Nop.Core.Infrastructure;
using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Discounts;
using Orbio.Services.Orders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Orbio.Web.UI.Models.Catalog
{
    public class ProductDetailModel : ProductOverViewModel, IShoppingCartItem
    {
        

        public ProductDetailModel()
        {
            this.ProductVariantAttributes = new List<ProductVariantAttributeModel>();
            this.SpecificationAttributes = new List<SpecificationAttribute>();
        }
        public ProductDetailModel(ProductDetail productDetail) : base(productDetail)  
        {
           
            this.Id = productDetail.Id;
            this.Name = productDetail.Name;
            this.ShortDescription = productDetail.ShortDescription;
            this.FullDescription = productDetail.FullDescription;
            this.SeName = productDetail.SeName;
            this.ViewPath = productDetail.ViewPath;
            this.ImageRelativeUrl = productDetail.ImageRelativeUrl;
            this.CurrencyCode = productDetail.CurrencyCode;
          
            //this.ProductPrice.Price = productDetail.Price;
             if (productDetail.BreadCrumbs != null && productDetail.BreadCrumbs.Count > 0)
            {               
                this.BreadCrumbs = (from c in productDetail.BreadCrumbs
                                    select new CategoryModel { Name = c.Name, SeName = c.SeName }).ToList();
            }

            if (productDetail.SpecificationFilters != null && productDetail.SpecificationFilters.Count > 0)
            {
                var specFilterByspecAttribute = from sa in productDetail.SpecificationFilters
                                           group sa by sa.SubTitle;

                //var specFilterByspecAttribute = from sa in productDetail.SpecificationFilters
                //                                group sa by sa.SpecificationAttributeName;
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
                                                                                 SpecificationAttributeName = sao.SpecificationAttributeName,
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
            else
            {
                //TODO: set default picture
                if (this.ProductPictures == null || this.ProductPictures.Count==0)
                {
                    this.ProductPictures = new List<PictureModel>();
                    this.DefaultPicture = new PictureModel();
                }
            }

            if (productDetail.ProductAttributeVariants != null)
            {
                this.ProductVariantAttributes = (from pv in productDetail.ProductAttributeVariants
                                                 select new ProductVariantAttributeModel
                                                 {
                                                     Id = pv.Id,
                                                     AttributeControlType = pv.AttributeControlType,
                                                     TextPrompt = pv.TextPrompt,
                                                     SizeGuideUrl=pv.SizeGuideUrl,
                                                     IsRequired = pv.IsRequired,
                                                     ProductAttributeId = pv.ProductAttributeId,
                                                     Values = new List<ProductVariantAttributeValueModel>((from pvv in pv.ProductVariantAttributeValues
                                                                                                           select new ProductVariantAttributeValueModel
                                                                                                           {
                                                                                                               Id = pvv.Id,
                                                                                                               ColorSquaresRgb = pvv.ColorSquaresRgb,
                                                                                                               Name = pvv.Name,
                                                                                                               PictureUrl = pvv.PictureUrl,
                                                                                                               PriceAdjustment = pvv.CalculatePrice(),
                                                                                                               IsPreSelected=pvv.IsPreSelected,
                                                                                                               DisplayOrder=pvv.DisplayOrder
                                                                                                               //PriceAdjustmentValue =  need TODO: format + or -
                                                                                                           }))
                                                 }).ToList();
            }
           
            this.StockAvailability = productDetail.FormatStockMessage();
            this.IsFreeShipping = productDetail.IsFreeShipping;
            this.IsShipEnabled = productDetail.IsShipEnabled;
            this.DeliveredIn = productDetail.DeliveredIn;
            this.OrderMaximumQuantity = productDetail.OrderMaximumQuantity;
            this.OrderMinimumQuantity = productDetail.OrderMinimumQuantity;
            this.AllowedQuantities = new List<int>();
            if (!String.IsNullOrWhiteSpace(productDetail.AllowedQuantities))
            {
               
                 productDetail
                    .AllowedQuantities
                    .Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .ForEach(qtyStr =>
                                 {
                                     int qty = 0;
                                     if (int.TryParse(qtyStr.Trim(), out qty))
                                     {
                                         this.AllowedQuantities.Add(qty);
                                     }
                                 } ); 
            }

            var priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
            this.ProductPrice.OldPrice = priceCalculationService.GetUnitPrice(this);
            
            this.PriceDetailXml = productDetail.GetProductPriceDetailXml();
            //var pvValues = (from pva in this.ProductVariantAttributes
            //                from pvav in pva.Values
            //                select pvav).ToList();
            //decimal priceAdjustment = 0;
            //if (pvValues.Count > 0)
            //{
            //    foreach (var pvav in pvValues)
            //    {
            //        priceAdjustment += Convert.ToDecimal(pvav.PriceAdjustment);
            //    }
            //}

            //this.ProductPrice.Price = (priceAdjustment + productDetail.Price).ToString("#,##0.00");

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


        public int OrderMinimumQuantity { get; set; }

        public int OrderMaximumQuantity { get; set; }

        public List<int> AllowedQuantities { get; set; }

        public string SelectedQuantity { get; set; }

        public string AttributeXml { get; set; }

        public string PriceDetailXml { get; set; }


        decimal IShoppingCartItem.Price
        {
            get
            {
                return this.ProductPrice.Price;
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


        IEnumerable<IProductAttribute> IShoppingCartItem.ProductVariantPriceAdjustments
        {
            get
            {
                return this.ProductVariantAttributes;
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

        private static string GetThumbImageFileName(string imageUrl)
        {
            var fileName = imageUrl;
            if (imageUrl.IndexOf('?') > 0)
            {
                fileName = imageUrl.Substring(0, imageUrl.IndexOf('?'));
            }

            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1, fileName.Length - fileName.LastIndexOf('/') - 1);
            fileName = Path.GetFileNameWithoutExtension(fileName);

            return fileName.Length > 0 ? imageUrl.Replace(fileName, fileName + "_tb") : fileName;
        }





        int IShoppingCartItem.ProductId
        {
            get { return this.Id; }
        }


        

        string IShoppingCartItem.PriceDetailXml
        {
            get { return this.PriceDetailXml; }
        }


        string IShoppingCartItem.AttributeXml
        {
            get { return this.AttributeXml; }
        }
    }
}