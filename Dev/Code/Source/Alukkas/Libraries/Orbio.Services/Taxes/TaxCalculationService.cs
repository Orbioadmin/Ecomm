using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Customers;
using Orbio.Services.Orders;
using Orbio.Services.Taxes.TaxProviders;
using System.Collections.Generic;
using System.Linq;
namespace Orbio.Services.Taxes
{
    public class TaxCalculationService : ITaxCalculationService
    {
       
       
        public decimal CalculateTax(ICart cart, Customer customer, out Dictionary<int, decimal> taxRate)
        {
            taxRate = new Dictionary<int, decimal>();
            var taxAmount = decimal.Zero;
            taxRate = TaxProviderFactory.CreateTaxProvider(customer).GetTaxRate(new CalculateTaxRequest
            {
                Customer = customer,
                TaxCategoryIds =
                    new List<int>((from sci in cart.ShoppingCartItems
                                       select sci.TaxCategoryId).ToList().Distinct())
            });
            foreach (var item in cart.ShoppingCartItems)
            {
               
                if(taxRate.ContainsKey(item.TaxCategoryId))
                {
                    taxAmount += item.FinalPrice * (taxRate[item.TaxCategoryId] / 100);
                }
            }
            return taxAmount;
        }


        public decimal CalculateTax(ICart cart, Customer customer)
        {
            var taxRate = new Dictionary<int, decimal>();
           return CalculateTax(cart, customer, out taxRate);
        }


        public decimal CalculateTax(decimal price, int taxCategoryId, Customer customer)
        {
            var taxAmount = decimal.Zero;
            var taxRate = TaxProviderFactory.CreateTaxProvider(customer).GetTaxRate(new CalculateTaxRequest
            {
                Customer = customer,
                TaxCategoryIds =
                    new List<int>{{taxCategoryId}}
            });

            if (taxRate.ContainsKey(taxCategoryId))
            {
                taxAmount = price * (taxRate[taxCategoryId] / 100);
            }

            return taxAmount;
        }


        public decimal CalculateTax(ICart cart, Customer customer, out Dictionary<decimal, decimal> taxRates)
        {
            taxRates = new Dictionary<decimal, decimal>();
            var taxAmount = decimal.Zero;
            var taxRateCategories = TaxProviderFactory.CreateTaxProvider(customer).GetTaxRate(new CalculateTaxRequest
            {
                Customer = customer,
                TaxCategoryIds =
                    new List<int>((from sci in cart.ShoppingCartItems
                                   select sci.TaxCategoryId).ToList().Distinct())
            });
            foreach (var item in cart.ShoppingCartItems)
            {

                if (taxRateCategories.ContainsKey(item.TaxCategoryId))
                {
                    taxAmount += item.FinalPrice * (taxRateCategories[item.TaxCategoryId] / 100);
                    if (taxRates.ContainsKey(taxRateCategories[item.TaxCategoryId]))
                    {
                        taxRates[taxRateCategories[item.TaxCategoryId]] = taxRates[taxRateCategories[item.TaxCategoryId]] + item.FinalPrice * (taxRateCategories[item.TaxCategoryId] / 100);
                    }
                    else
                    {
                        taxRates.Add(taxRateCategories[item.TaxCategoryId], item.FinalPrice * (taxRateCategories[item.TaxCategoryId] / 100));
                    }
                }
            }
            return taxAmount;
        }
    }
}
