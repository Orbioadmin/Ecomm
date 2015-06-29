using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog.Abstract;

namespace Orbio.Services.Orders
{
    public interface IPriceCalculationService
    {
        decimal GetCartSubTotal(ICart cart, bool includeDiscounts);
        decimal GetOrderTotal(ICart cart, bool includeDiscounts);
        decimal GetFinalPrice(IShoppingCartItem cartItem, bool includeDiscounts, bool includeQty);
        decimal GetUnitPrice(IShoppingCartItem cartItem);
        decimal GetAllDiscountAmount(ICart cart);
    }
}
