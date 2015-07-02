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
    }
}
