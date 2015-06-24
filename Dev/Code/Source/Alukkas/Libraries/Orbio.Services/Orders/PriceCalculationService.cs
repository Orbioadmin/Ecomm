using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Discounts;

namespace Orbio.Services.Orders
{
    public class PriceCalculationService : IPriceCalculationService
    {
        public decimal GetCartSubTotal(ICart cart, bool includeDiscounts)
        {
            var subTotal = 0.00M;
            foreach (var sci in cart.ShoppingCartItems)
            {
                subTotal += GetFinalPrice(sci, includeDiscounts);
            }


            return includeDiscounts? subTotal - GetDiscountAmount(cart.Discounts, subTotal):subTotal;
        }


        public decimal GetFinalPrice(IShoppingCartItem cartItem, bool includeDiscounts, bool includeQty = true)
        {
            var unitPrice = GetUnitPrice(cartItem);
            unitPrice = includeDiscounts? unitPrice - GetDiscountAmount(cartItem.Discounts, unitPrice):unitPrice;
            return includeQty? unitPrice * cartItem.Quantity: unitPrice;
        }


        public decimal GetUnitPrice(IShoppingCartItem cartItem)
        {
            //Add pva values
            var pa = 0M;
            foreach (var pva in cartItem.ProductVariantPriceAdjustments)
            {
                pa += pva;
            }
            return cartItem.Price + pa ;
        }

        private decimal GetDiscountAmount(IEnumerable<IDiscount> discounts, decimal finalPrice)
        {
            var discountAmount = 0M;
            if (discounts.Any())
            {
                foreach (var d in discounts)
                {
                    if (d.UsePercentage)
                    {
                        discountAmount += (finalPrice * d.DiscountPercentage) / 100;
                    }
                    else
                    {
                        discountAmount += d.DiscountAmount;
                    }
                }
            }

            return discountAmount;
        }


        public decimal GetAllDiscountAmount(ICart cart)
        {
            var discountAmount = 0M;
            discountAmount += GetDiscountAmount(cart.Discounts, GetCartSubTotal(cart, false));
            foreach (var sci in cart.ShoppingCartItems)
            {
                discountAmount += GetDiscountAmount(sci.Discounts, GetFinalPrice(sci, false, true));
            }
            return discountAmount;
        }
    }
}
