using System.Collections.Generic;
using System.Linq;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Discounts;
using Orbio.Services.Orders;
using Nop.Core.Infrastructure;

namespace Orbio.Web.UI.Models.Orders
{
    public class CartModel : ICart
    {
        private readonly IPriceCalculationService priceCalculationService;

         public CartModel(IPriceCalculationService priceCalculationService)
        {
            this.priceCalculationService = priceCalculationService;
            this.ShoppingCartItems = new List<ShoppingCartItemModel>();
            this.Discounts = new List<Discount>();
        }
         public CartModel(Cart cart)
             : this(EngineContext.Current.Resolve<IPriceCalculationService>())
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
                 return priceCalculationService.GetCartSubTotal(this, true).ToString("#,##0.00");
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