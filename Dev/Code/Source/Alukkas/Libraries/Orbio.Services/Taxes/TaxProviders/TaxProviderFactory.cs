using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Nop.Data;
using Orbio.Core.Domain.Customers;

namespace Orbio.Services.Taxes.TaxProviders
{
    public class TaxProviderFactory
    {
        private static volatile ITaxProvider taxProvider;
        private static object syncRoot = new object();


        public static ITaxProvider CreateTaxProvider(Customer customer)
        {
            if (taxProvider == null)
            {
                lock (syncRoot)
                {
                    if (taxProvider == null)
                        taxProvider = new FixedTaxProvider(EngineContext.Current.Resolve<IDbContext>(), EngineContext.Current.Resolve<ICacheManager>());
                }
            }
             return taxProvider;
        }
    }
}
