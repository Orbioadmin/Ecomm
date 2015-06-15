using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Utility;

namespace Orbio.Core.Domain.Catalog
{
    public static class ProductExtensions
    {
        /// <summary>
        /// Formats the stock availability/quantity message
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="localizationService">Localization service</param>
        /// <returns>The stock message</returns>
        public static string FormatStockMessage(this ProductDetail product)
        {

            if (product == null)
                throw new ArgumentNullException("product");

            //if (localizationService == null)
            //    throw new ArgumentNullException("localizationService");

            string stockMessage = string.Empty;

            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock
                && product.DisplayStockAvailability)
            {
                //Madhu MB only supporting nobackorder mode for now
                //switch (product.BackorderMode)
                //{
                //    case BackorderMode.NoBackorders:
                //        {
                if (product.StockQuantity > 0)
                {
                    if (product.DisplayStockQuantity)
                    {
                        //display "in stock" with stock quantity
                        stockMessage = string.Format("{0} in stock", product.StockQuantity);//localizationService.GetResource("Products.Availability.InStockWithQuantity")
                    }
                    else
                    {
                        //display "in stock" without stock quantity
                        stockMessage = "In stock";//localizationService.GetResource("Products.Availability.InStock");
                    }
                }
                else
                {
                    //display "out of stock"
                    stockMessage = "Out of stock"; // localizationService.GetResource("Products.Availability.OutOfStock");
                }
                //        }
                //        break;
                //    case BackorderMode.AllowQtyBelow0:
                //        {
                //            if (product.StockQuantity > 0)
                //            {
                //                if (product.DisplayStockQuantity)
                //                {
                //                    //display "in stock" with stock quantity
                //                    stockMessage = string.Format(localizationService.GetResource("Products.Availability.InStockWithQuantity"), product.StockQuantity);
                //                }
                //                else
                //                {
                //                    //display "in stock" without stock quantity
                //                    stockMessage = localizationService.GetResource("Products.Availability.InStock");
                //                }
                //            }
                //            else
                //            {
                //                //display "in stock" without stock quantity
                //                stockMessage = localizationService.GetResource("Products.Availability.InStock");
                //            }
                //        }
                //        break;
                //    case BackorderMode.AllowQtyBelow0AndNotifyCustomer:
                //        {
                //            if (product.StockQuantity > 0)
                //            {
                //                if (product.DisplayStockQuantity)
                //                {
                //                    //display "in stock" with stock quantity
                //                    stockMessage = string.Format(localizationService.GetResource("Products.Availability.InStockWithQuantity"), product.StockQuantity);
                //                }
                //                else
                //                {
                //                    //display "in stock" without stock quantity
                //                    stockMessage = localizationService.GetResource("Products.Availability.InStock");
                //                }
                //            }
                //            else
                //            {
                //                //display "backorder" without stock quantity
                //                stockMessage = localizationService.GetResource("Products.Availability.Backordering");
                //            }
                //        }
                //        break;
                //    default:
                //        break;
                //}
            }

            return stockMessage;
        }

        public static string CalculatePrice(this IPriceComponent product)
        {
            IPriceCalculator priceCalculator = null;
            if (product.ProductPriceDetail != null)
            {
                if (product.ProductPriceDetail.PriceComponents.Count == 0 && product.ProductPriceDetail.ProductComponents.Count == 0)
                {
                    priceCalculator = new SimplePriceCalculator(product);
                }
                else
                {
                    priceCalculator = new ComponentPriceCalculator(product);
                }
            }
            else
            {
                priceCalculator = new SimplePriceCalculator(product);
            }
            return priceCalculator.FormattedPrice;
        }


        public static Dictionary<string, string> GetComponentDetails(this IPriceComponent product)
        {
            Dictionary<string, string> componentDetails = new Dictionary<string, string>();
            if (product.ProductPriceDetail==null ||  product.ProductPriceDetail.PriceComponents.Count == 0 && product.ProductPriceDetail.ProductComponents.Count == 0)
            {
                return componentDetails;
            }
            else
            {
                decimal goldRate = 0;
                //Product component price total
                decimal componentPriceTotal = 0;
                foreach (var componentPrice in product.ProductPriceDetail.ProductComponents)
                {
                    componentPriceTotal += componentPriceTotal + componentPrice.UnitPrice;
                }
                //Product gold price
                try
                {
                    goldRate = product.GoldWeight * ((product.ProductUnit * product.MarketUnitPrice / (product.PriceUnit)));
                }
                catch
                { }

                //Product price component total
                decimal priceComponentTotal = 0;
                foreach (var priceComponent in product.ProductPriceDetail.PriceComponents)
                {
                    if (priceComponent.Price != null)
                    {
                        priceComponentTotal += Convert.ToDecimal(priceComponent.Price);
                    }
                    else if (priceComponent.Percentage != null)
                    {
                        priceComponentTotal += Convert.ToDecimal((goldRate * priceComponent.Percentage) / 100);
                    }
                    else if (priceComponent.ItemPrice != null)
                    {
                        priceComponentTotal += Convert.ToDecimal(priceComponent.ItemPrice);
                    }
                }
                componentDetails.Add("GOLD", goldRate.ToString("#,##0.00"));
                componentDetails.Add("Other St.", componentPriceTotal.ToString("#,##0.00"));
                componentDetails.Add("Other", priceComponentTotal.ToString("#,##0.00"));
                return componentDetails;
            }

        }


        public static string GetProductPriceDetailXml(this IPriceComponent product)
        {
            var prodPriceComp = new ProductPriceComponent
            {
                GoldWeight = product.GoldWeight,
                MarketUnitPrice = product.MarketUnitPrice,
                PriceUnit = product.PriceUnit,
                ProductUnit = product.ProductUnit,
                ProductPriceDetail = product.ProductPriceDetail
            };

            var xml = Serializer.GenericSerializer<ProductPriceComponent>(prodPriceComp);

            return xml;
        }
    }


    public interface IPriceCalculator { string FormattedPrice { get; } }

    public class SimplePriceCalculator : IPriceCalculator
    {
        private IPriceComponent product;

        public SimplePriceCalculator(IPriceComponent product)
        {
            this.product = product;
        }

        public string FormattedPrice
        {
            get { return product.Price.ToString("#,##0.00"); }
        }
    }

    public class ComponentPriceCalculator : IPriceCalculator
    {
        private IPriceComponent product;

        public ComponentPriceCalculator(IPriceComponent product)
        {
            decimal goldRate = 0;
            //Product component price total
            decimal componentPriceTotal = 0;
            foreach (var componentPrice in product.ProductPriceDetail.ProductComponents)
            {
                componentPriceTotal += componentPriceTotal + componentPrice.UnitPrice;
            }
            //Product gold price
            try
            {
                goldRate = product.GoldWeight * ((product.ProductUnit * product.MarketUnitPrice / (product.PriceUnit)));
            }
            catch
            { }

            //Product price component total
            decimal priceComponentTotal = 0;
            foreach (var priceComponent in product.ProductPriceDetail.PriceComponents)
            {
                if (priceComponent.Price != null)
                {
                    priceComponentTotal += Convert.ToDecimal(priceComponent.Price);
                }
                else if (priceComponent.Percentage != null)
                {
                    priceComponentTotal += Convert.ToDecimal((goldRate * priceComponent.Percentage) / 100);
                }
                else if (priceComponent.ItemPrice != null)
                {
                    priceComponentTotal += Convert.ToDecimal(priceComponent.ItemPrice);
                }
            }
            decimal productPrice = goldRate + componentPriceTotal + priceComponentTotal;
            product.Price = productPrice;
            this.product = product;
        }

        public string FormattedPrice
        {
            get { return product.Price.ToString("#,##0.00"); }
        }
    }

    /// <summary>
    /// class to serialize ipricecomponent 
    /// </summary>
    public class ProductPriceComponent
    {
        public decimal GoldWeight { get; set; }

        public decimal MarketUnitPrice { get; set; }

        public int PriceUnit { get; set; }

        public decimal ProductUnit { get; set; }

        public ProductPriceDetail ProductPriceDetail { get; set; }
    }

}
