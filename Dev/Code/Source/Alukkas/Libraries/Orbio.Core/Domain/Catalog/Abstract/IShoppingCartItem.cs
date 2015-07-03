
using Orbio.Core.Domain.Discounts;
using System.Collections.Generic;
namespace Orbio.Core.Domain.Catalog.Abstract
{
    public interface IShoppingCartItem
    {
        decimal Price { get; }
        int Quantity { get; }
        IEnumerable<IDiscount> Discounts { get; }
        IEnumerable<decimal> ProductVariantPriceAdjustments { get; }
        int TaxCategoryId { get; }
        decimal FinalPrice { get; }
    }
}
