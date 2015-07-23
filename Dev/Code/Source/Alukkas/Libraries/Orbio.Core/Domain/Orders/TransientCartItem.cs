using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    public class TransientCartItem 
    {
        public TransientCartItem()
        {
        }

        public TransientCartItem(IShoppingCartItem cartItem)
        {
            this.Price = cartItem.Price;
            this.Quantity = cartItem.Quantity;
            this.Discounts = new List<Discount>();
            this.Discounts.AddRange(cartItem.Discounts.Cast<Discount>());
            //this.Discounts.AddRange((from d in cartItem.Discounts
            //                         select new Discount {  Id = d.Id, DiscountAmount = d.DiscountAmount, DiscountPercentage=d.DiscountPercentage, 
            //                         DiscountTypeId = d.DiscountTypeId, UsePercentage = d.UsePercentage, RequiresCouponCode=d.RequiresCouponCode}));
            this.Attributes = new List<TransientCartAttribute>();
            this.Attributes.AddRange((from pva in cartItem.ProductVariantPriceAdjustments
                                      select new TransientCartAttribute
                                      {
                                          AttributeName = pva.AttributeName,
                                          AttributeValues =
                                              new List<TransientCartAttributeValue>((from pvav in pva.ProductAttributeValues
                                                                                     select new TransientCartAttributeValue {  AttributeValue=pvav.AttributeValue,
                                                                                     PriceAdjustment=pvav.PriceAdjustment}))
                                      }).ToList());
            this.TaxCategoryId = cartItem.TaxCategoryId;
            this.FinalPrice = cartItem.FinalPrice;
            this.ProductId = cartItem.ProductId;
            this.AttributeXml = cartItem.AttributeXml;
            this.PriceDetailXml = cartItem.PriceDetailXml;
        }

        public decimal Price
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public List<Discount> Discounts { get; set; }

        public List<TransientCartAttribute> Attributes { get; set; }

        public int TaxCategoryId { get; set; }

        public decimal FinalPrice { get; set; }

        public int ProductId { get; set; }

        public string AttributeXml { get; set; }

        public string PriceDetailXml { get; set; }

 
    }
}
