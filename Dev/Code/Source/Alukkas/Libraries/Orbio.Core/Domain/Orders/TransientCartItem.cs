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
            this.Discounts.AddRange((from d in cartItem.Discounts
                                     select new Discount { DiscountAmount = d.DiscountAmount, DiscountPercentage=d.DiscountPercentage, 
                                     DiscountTypeId = d.DiscountTypeId, UsePercentage = d.UsePercentage}));
            this.ProductVariantPriceAdjustments = new List<decimal>();
            this.ProductVariantPriceAdjustments.AddRange(cartItem.ProductVariantPriceAdjustments);
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

        public List<decimal> ProductVariantPriceAdjustments
        {
            get;
            set;
        }
    }
}
