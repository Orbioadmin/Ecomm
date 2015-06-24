using Orbio.Core.Domain.Discounts;
using System.Collections.Generic;

namespace Orbio.Core.Domain.Catalog.Abstract
{
    public interface ICart
    {
        IEnumerable<IShoppingCartItem> ShoppingCartItems { get; }
        IEnumerable<IDiscount> Discounts { get; }
    }
}
