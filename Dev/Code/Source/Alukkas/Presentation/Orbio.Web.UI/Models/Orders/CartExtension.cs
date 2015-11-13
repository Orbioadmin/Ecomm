using Orbio.Core.Domain.Orders;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public static class CartExtension
    {

        public static CartModel ConvertToCartModel(this TransientCart transientCart)
        {
            var cartModel = new CartModel();
            cartModel.Discounts = transientCart.Discounts;
            cartModel.ShoppingCartItems = (from sci in transientCart.ShoppingCartItems
                                           select new ShoppingCartItemModel
                                           {
                                               AttributeXml = sci.AttributeXml,
                                               ProductPrice = new Catalog.ProductOverViewModel.ProductPriceModel { Price = sci.Price },
                                               TaxCategoryId = sci.TaxCategoryId,
                                               SelectedQuantity = sci.Quantity.ToString(),
                                               PriceDetailXml = sci.PriceDetailXml,
                                               Id = sci.ProductId,
                                               Discounts = sci.Discounts,
                                               IsGiftWrapping=sci.IsGiftWrapping,
                                               GiftCharge=sci.GiftCharge,
                                               IsFreeShipping=sci.IsFreeShipping,
                                               AdditionalShippingCharge=sci.AdditionalShippingCharge,
                                               ProductVariantAttributes = (from pva in sci.Attributes
                                                                           select new ProductVariantAttributeModel
                                                                           {
                                                                               TextPrompt = pva.AttributeName,
                                                                               Values = (from pvav in pva.AttributeValues
                                                                                         select new ProductVariantAttributeValueModel { Name = pvav.AttributeValue,
                                                                                          PriceAdjustment = pvav.PriceAdjustment}).ToList()
                                                                           }).ToList()
                                           }).ToList();
            return cartModel;
        }
    }
}