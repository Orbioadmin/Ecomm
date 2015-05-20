
namespace Orbio.Core.Domain.Catalog.Abstract
{
    /// <summary>
    /// interface for price components
    /// </summary>
    public interface IPriceComponent
    {
        /// <summary>
        /// gets or sets the price
        /// </summary>
        decimal Price { get; set; }

        /// <summary>
        /// gets or sets product price detail
        /// </summary>
        ProductPriceDetail ProductPriceDetail { get; set; }

        /// <summary>
        /// Gets or sets GoldWeight
        /// </summary>
        decimal GoldWeight { get; set; }

        /// <summary>
        /// Gets or sets ProductUnit
        /// </summary>
        decimal ProductUnit { get; set; }

        /// <summary>
        /// Gets or sets PriceUnit
        /// </summary>
        int PriceUnit { get; set; }

        /// <summary>
        /// Gets or sets MarketUnitPrice
        /// </summary>
        decimal MarketUnitPrice { get; set; }
    }
}
