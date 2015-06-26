using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Orders
{
    public class TransientCart
    {
        public TransientCart()
        {
        }

        public TransientCart(ICart cart)
        {
            this.Discounts = new List<Discount>();
            this.Discounts.AddRange((from d in cart.Discounts
                                     select new Discount
                                     {
                                         DiscountAmount = d.DiscountAmount,
                                         DiscountPercentage = d.DiscountPercentage,
                                         DiscountTypeId = d.DiscountTypeId,
                                         UsePercentage = d.UsePercentage
                                     }).ToList());
            this.ShoppingCartItems = new List<TransientCartItem>();
            this.ShoppingCartItems.AddRange((from sci in cart.ShoppingCartItems
                                             select new TransientCartItem(sci)).ToList());
        }
        public List<TransientCartItem> ShoppingCartItems
        {
            get;
            set;
        }

        public List<Discount> Discounts
        {
            get;
            set;
        }
    }
}
