using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Taxes.TaxProviders
{
    public interface ITaxProvider
    {
        Dictionary<int,decimal> GetTaxRate(CalculateTaxRequest request);
    }
}
