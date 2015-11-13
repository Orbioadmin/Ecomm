
using Orbio.Core.Domain.Discounts;
using System.Collections.Generic;
namespace Orbio.Core.Domain.Catalog.Abstract
{
    public interface IShoppingCartItem
    {
        decimal Price { get; }
        int Quantity { get; }
        IEnumerable<IDiscount> Discounts { get; }
        IEnumerable<IProductAttribute> ProductVariantPriceAdjustments { get; }
        int TaxCategoryId { get; }
        decimal FinalPrice { get; }
        int ProductId { get; }
        string AttributeXml { get; }        
        string PriceDetailXml { get; }
        bool IsGiftWrapping { get; set; }
        decimal GiftCharge { get; set; }
        bool IsFreeShipping { get; set; }
        decimal AdditionalShippingCharge { get; set; }
    }
}
