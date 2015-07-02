using Orbio.Core.Domain.Catalog.Abstract;
using Orbio.Core.Domain.Customers;
using System.Collections.Generic;

namespace Orbio.Services.Taxes
{
    public interface ITaxCalculationService
    {        
        decimal CalculateTax(ICart cart, Customer customer, out Dictionary<int, decimal> taxRate);
        decimal CalculateTax(ICart cart, Customer customer);
    }
}
