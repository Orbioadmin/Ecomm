using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Data;
using Orbio.Core.Domain.Stores;

namespace Orbio.Services.Stores
{
    /// <summary>
    /// Store service
    /// </summary>
    public class StoreService : IStoreService
    {

        private readonly IDbContext context;
        private readonly ICacheManager cacheManager;
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string STORES_ALL_KEY = "Nop.stores.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string STORES_BY_ID_KEY = "Nop.stores.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string STORES_PATTERN_KEY = "Nop.stores.";

        #endregion

        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public StoreService(IDbContext context, ICacheManager cacheManager)
        {
            this.context = context;
            this.cacheManager = cacheManager;
        }
        public IList<Store> GetAllStores(string host)
        {
           
            string key = STORES_ALL_KEY;
            return cacheManager.Get(key, () =>
            {
                 var result = context.ExecuteFunction<Store>("usp_Store_GetAllStores",
                    new SqlParameter() { ParameterName = "@host", Value = host, DbType = System.Data.DbType.String });

                 return result != null ? result.ToList() : new List<Store>();
                 
            });
             
        }

        
    }
}
