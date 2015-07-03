using Nop.Core.Caching;
using Nop.Data;
using Orbio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Taxes.TaxProviders
{
    public class FixedTaxProvider : ITaxProvider
    {
        private readonly IDbContext dbContext;
        private readonly ICacheManager cacheManager;

        public FixedTaxProvider(IDbContext dbContext, ICacheManager cacheManager)
        {
            this.dbContext = dbContext;
            this.cacheManager = cacheManager;
        }
        
        public Dictionary<int, decimal> GetTaxRate(CalculateTaxRequest request)
        {
            var taxRates = new Dictionary<int,decimal>();
            var sqlParamList = new List<SqlParameter>();
            var taxCatIds = (from t in request.TaxCategoryIds
                             where t > 0
                             select t.ToString()).ToList();

            if (taxCatIds.Count > 0)
            {
                
                   
                    sqlParamList.Add(new SqlParameter { ParameterName = "@taxCategoryIds", Value = taxCatIds.Aggregate((t1, t2) => t1 + "," + t2), DbType = System.Data.DbType.String });

                    var result = dbContext.ExecuteFunction<Setting>("usp_GetFixedTaxRate",
                        sqlParamList.ToArray()
                        );
                    foreach (var taxRate in result)
                    {
                        var taxCatId = Convert.ToInt32(taxRate.Name);
                        if (!taxRates.ContainsKey(taxCatId))
                        {
                            taxRates.Add(taxCatId, Convert.ToDecimal(taxRate.Value));
                        }
                    }
                   
 
            }


            return taxRates;
        }
    }
}
