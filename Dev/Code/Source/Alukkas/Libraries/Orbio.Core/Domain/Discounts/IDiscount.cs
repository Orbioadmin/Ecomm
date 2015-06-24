using System;
namespace Orbio.Core.Domain.Discounts
{
    public interface IDiscount
    {
        decimal DiscountAmount { get; set; }
        decimal DiscountPercentage { get; set; }
        int DiscountTypeId { get; set; }
        bool UsePercentage { get; set; }
    }
}
