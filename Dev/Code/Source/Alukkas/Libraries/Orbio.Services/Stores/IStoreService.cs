using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Stores;

namespace Orbio.Services.Stores
{
    /// <summary>
    /// Store service interface
    /// </summary>
    public partial interface IStoreService
    {       
        /// <summary>
        /// Gets all stores
        /// </summary>
        /// <returns>Store collection</returns>
        IList<Store> GetAllStores(string host);
    }
}
