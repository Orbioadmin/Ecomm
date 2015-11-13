using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Discounts;
using Orbio.Services.Taxes;
using Orbio.Core;

namespace Orbio.Services.Orders
{
    public class PriceCalculationService : IPriceCalculationService
    {
        public readonly ITaxCalculationService taxCalculationService;
        protected readonly IWorkContext workContext;

        public PriceCalculationService(ITaxCalculationService taxCalculationService, IWorkContext workContext)
        {
            this.taxCalculationService = taxCalculationService;
            this.workContext = workContext;
        }

        public decimal GetCartSubTotal(ICart cart, bool includeDiscounts)
        {
            var subTotal = 0.00M;
            foreach (var sci in cart.ShoppingCartItems)
            {
                subTotal += GetFinalPrice(sci, true);
            }
           
            if (includeDiscounts)
            {
                var orderDiscounts = (from d in cart.Discounts
                                      where d.RequiresCouponCode == false
                                      select d).ToList();

                subTotal =  subTotal - GetDiscountAmount(orderDiscounts, subTotal);
                var coupon = (from d in cart.Discounts
                              where d.RequiresCouponCode == true
                              select d).FirstOrDefault();

                if (coupon != null)
                {
                    subTotal = subTotal - GetDiscountAmount(coupon, subTotal);
                }
            }
            return  subTotal;
        }
      
        public decimal GetOrderTotal(ICart cart, bool includeDiscounts)
        {
            var subTotal = GetCartSubTotal(cart, includeDiscounts);
            var taxRates = new Dictionary<int, decimal>();
            var taxAmount = taxCalculationService.CalculateTax(cart, workContext.CurrentCustomer, out taxRates);
            var shippingAmount = GetShippingGiftTotal(cart);
            //need to add shipping and taxes
            return subTotal + taxAmount + shippingAmount;
        }

        public decimal GetFinalPrice(IShoppingCartItem cartItem, bool includeDiscounts, bool includeQty = true)
        {
            var unitPrice = GetUnitPrice(cartItem);
            unitPrice = includeDiscounts ? unitPrice - GetDiscountAmount(cartItem.Discounts, unitPrice) : unitPrice;
            return includeQty? unitPrice * cartItem.Quantity: unitPrice;
        }

        //public decimal GetFinalDiscountPrice(IShoppingCartItem cartItem, bool includeDiscounts, bool includeQty = true)
        //{
        //    var unitPrice = GetUnitPrice(cartItem);
        //    //unitPrice = includeDiscounts ? unitPrice - GetDiscountAmount(cartItem.Discounts, unitPrice) : unitPrice;
        //    unitPrice = unitPrice - GetDiscountAmount(cartItem.Discounts, unitPrice);
        //    return includeQty ? unitPrice * cartItem.Quantity : unitPrice;
        //}

        public decimal GetUnitPrice(IShoppingCartItem cartItem)
        {
            //Add pva values
            var pa = 0M;
            var pvas = from pva in cartItem.ProductVariantPriceAdjustments
                       from pvav in pva.ProductAttributeValues
                       select pvav.PriceAdjustment;
            foreach (var pva in pvas)
            {
                pa += pva;
            }
            var unitPrice = cartItem.Price + pa ;
            return unitPrice;
        }

        public decimal GetAllDiscountAmount(ICart cart)
        {
            var discountAmount = 0M;
            var subTotal = this.GetCartSubTotal(cart, false);
            foreach (var sci in cart.ShoppingCartItems)
            {
                discountAmount += GetDiscountAmount(sci.Discounts, GetFinalPrice(sci, false, false)) * sci.Quantity;
            }
            var orderDiscounts = (from d in cart.Discounts
                                  where d.RequiresCouponCode == false
                                  select d).ToList();

            discountAmount += GetDiscountAmount(orderDiscounts, subTotal - discountAmount);
            
            return discountAmount;
        }

        public decimal GetShippingGiftTotal(ICart cart)
        {
            var totalAmount = 0M;
            var shippingCharge = 0M;
            var giftCharges = (from g in cart.ShoppingCartItems
                               where g.IsGiftWrapping
                               select g).ToList();
            var items = (from ship in cart.ShoppingCartItems
                                  where !ship.IsFreeShipping
                                  select ship).ToList();
            if(items!=null)
            {
                shippingCharge = (items.Count > 0) ? items.Max(m => m.AdditionalShippingCharge) : 0;
            }

            foreach (var item in giftCharges)
            {
                totalAmount = totalAmount + (item.GiftCharge * item.Quantity);
            }
            totalAmount=totalAmount+ shippingCharge;
            return totalAmount; 
        }

        public decimal GetAllDiscountAmount(ICart cart, out List<int> appliedDiscountIds)
        {
            appliedDiscountIds = new List<int>();
            var discountAmount = 0M;
            var subTotal = this.GetCartSubTotal(cart, false);
            foreach (var sci in cart.ShoppingCartItems)
            {
                int appliedDiscountId = 0;
                discountAmount += GetDiscountAmount(sci.Discounts, GetFinalPrice(sci, false, false), out appliedDiscountId) * sci.Quantity;
                if (appliedDiscountId != int.MinValue && appliedDiscountIds.Exists(d=>d == appliedDiscountId))
                {                    
                    appliedDiscountIds.Add(appliedDiscountId);
                }
            }
            var orderDiscounts = (from d in cart.Discounts
                                  where d.RequiresCouponCode == false
                                  select d).ToList();
            int appliedOrderDiscountId = 0;
            //discountAmount += GetDiscountAmount(orderDiscounts, subTotal - discountAmount, out appliedOrderDiscountId);
            discountAmount += GetDiscountAmount(orderDiscounts, subTotal, out appliedOrderDiscountId);
            if (appliedOrderDiscountId != int.MinValue && appliedDiscountIds.Exists(d => d == appliedOrderDiscountId))
            {
                appliedDiscountIds.Add(appliedOrderDiscountId);
            }
            var coupon = (from d in cart.Discounts
                          where d.RequiresCouponCode == true
                          select d).FirstOrDefault();

            if (coupon != null)
            {
                //discountAmount += GetDiscountAmount(coupon, subTotal - discountAmount);
                discountAmount += GetDiscountAmount(coupon, subTotal);
                appliedDiscountIds.Add(coupon.Id);
            }

            return discountAmount;
        }

        public decimal GetAllCouponDiscountAmount(ICart cart,string discount)
        {
            var discountAmount = 0M;
            var subTotal = this.GetCartSubTotal(cart, false);
            var coupon = (from d in cart.Discounts
                          where d.RequiresCouponCode == true
                          select d).FirstOrDefault();

            if (coupon != null)
            {
                discountAmount += GetDiscountAmount(coupon, subTotal);
            }

            return discountAmount;
        }

        public decimal GetDiscountAmount(IEnumerable<IDiscount> discounts, decimal finalPrice)
        {
            var discountAmount = 0M;
            var maxDiscountAmount = 0M;
            if (discounts.Any())
            {
                foreach (var d in discounts)
                {
                    if (d.UsePercentage)
                    {
                        discountAmount = (finalPrice * d.DiscountPercentage) / 100;
                    }
                    else
                    {
                        discountAmount = d.DiscountAmount;
                    }

                    if (discountAmount > maxDiscountAmount)
                    {
                        maxDiscountAmount = discountAmount;
                    }
                }
            }

            return maxDiscountAmount;
        }
        public decimal GetDiscountAmount(IEnumerable<IDiscount> discounts, decimal finalPrice, out int appliedDiscountId)
        {
            appliedDiscountId = int.MinValue;
            var discountAmount = 0M;
            var maxDiscountAmount = 0M;
            if (discounts.Any())
            {
                foreach (var d in discounts)
                {
                    appliedDiscountId = d.Id;
                    if (d.UsePercentage)
                    {
                        discountAmount = (finalPrice * d.DiscountPercentage) / 100;
                    }
                    else
                    {
                        discountAmount = d.DiscountAmount;
                    }

                    if (discountAmount > maxDiscountAmount)
                    {
                        maxDiscountAmount = discountAmount;
                        appliedDiscountId = d.Id;
                    }
                }
            }

            return maxDiscountAmount;
        }

        private decimal GetDiscountAmount(IDiscount discount, decimal finalPrice)
        {
            var discountAmount = 0M;
           
            if (discount!=null)
            {
                if (discount.UsePercentage)
                    {
                        discountAmount = (finalPrice * discount.DiscountPercentage) / 100;
                    }
                    else
                    {
                        discountAmount = discount.DiscountAmount;
                    }
             }

            return discountAmount;
        }





       
    }
}
