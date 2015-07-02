using System.Collections.Generic;
using System.Linq;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Discounts;
using Orbio.Services.Orders;
using Nop.Core.Infrastructure;
using Orbio.Services.Taxes;
using Orbio.Core;

namespace Orbio.Web.UI.Models.Orders
{
    public class CartModel : ICart
    {
        private readonly IPriceCalculationService priceCalculationService;
        private readonly ITaxCalculationService taxCalculationService;

        public CartModel()
        {
            this.priceCalculationService = EngineContext.Current.Resolve<IPriceCalculationService>();
            this.taxCalculationService = EngineContext.Current.Resolve<ITaxCalculationService>();
            this.ShoppingCartItems = new List<ShoppingCartItemModel>();
            this.Discounts = new List<Discount>();
        }

        public CartModel(IPriceCalculationService priceCalculationService, ITaxCalculationService taxCalculationService)
        {
            this.priceCalculationService = priceCalculationService;
            this.taxCalculationService = taxCalculationService;
            this.ShoppingCartItems = new List<ShoppingCartItemModel>();
            this.Discounts = new List<Discount>();
        }
         public CartModel(Cart cart)
            : this(EngineContext.Current.Resolve<IPriceCalculationService>(), EngineContext.Current.Resolve<ITaxCalculationService>())
         {
             if (cart.ShoppingCartItems != null && cart.ShoppingCartItems.Count > 0)
             {
                 this.ShoppingCartItems = (from p in cart.ShoppingCartItems
                                    select new ShoppingCartItemModel(p)).ToList();
             }

             if (cart.Discounts != null && cart.Discounts.Count > 0)
             {
                 this.Discounts = cart.Discounts;
             }
         }

         public List<ShoppingCartItemModel> ShoppingCartItems { get; private set; }

         public List<Discount> Discounts { get; set; }

         public Discount AppliedCoupon
         {
             get
             {
                 return (from d in this.Discounts
                         where d.RequiresCouponCode == true
                         select d).FirstOrDefault();
             }
         }

        /// <summary>
        /// to get the applied coupon code in the cart
        /// </summary>
         public string AppliedCouponCode { get; set; }

         public string TaxAmount
         {
             get
             {
                 var workContext = EngineContext.Current.Resolve<IWorkContext>();

                 return taxCalculationService.CalculateTax(this, workContext.CurrentCustomer).ToString("#,##0.00");
             }
         }

         public string SubTotal
         {
             get
             {
                 return priceCalculationService.GetCartSubTotal(this, false).ToString("#,##0.00");
             }
         }

         public string DiscountAmount
         {
             get
             {
                 return priceCalculationService.GetAllDiscountAmount(this).ToString("#,##0.00");
             }
         }

         public string Total
         {
             get
             {
                 return priceCalculationService.GetOrderTotal(this, true).ToString("#,##0.00");
             }
         }

         IEnumerable<IShoppingCartItem> ICart.ShoppingCartItems
         {
             get
             {
                 return this.ShoppingCartItems;

             }
         }


         IEnumerable<IDiscount> ICart.Discounts
         {
             get { return this.Discounts;  }
         }
    }
}