using Nop.Core.Infrastructure;
using Nop.Data;
using Orbio.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Taxes.TaxProviders
{
    public class TaxProviderFactory
    {

        public static ITaxProvider CreateTaxProvider(Customer customer)
        {
            //make decision to create tax provider in future here
            return new FixedTaxProvider(EngineContext.Current.Resolve<IDbContext>());
        }
    }
}
