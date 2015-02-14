using System;
using System.Linq;
using Orbio.Core;
using Orbio.Core.Domain.Stores;
using Orbio.Services.Stores;

namespace Orbio.Web.Framework
{
    /// <summary>
    /// Store context for web application
    /// </summary>
    public partial class WebStoreContext : IStoreContext
    {
        private readonly IStoreService storeService;
        private readonly IWebHelper webHelper;

        private Store cachedStore;

        public WebStoreContext(IStoreService storeService, IWebHelper webHelper)
        {
            this.storeService = storeService;
            this.webHelper = webHelper;
        }

        /// <summary>
        /// Gets or sets the current store
        /// </summary>
        public virtual Store CurrentStore
        {
            get
            {
                if (cachedStore != null)
                    return cachedStore;

                //ty to determine the current store by HTTP_HOST
                var host = webHelper.ServerVariables("HTTP_HOST");
                //var allStores = storeService.GetCurrentStore(host);
                var store = storeService.GetCurrentStore(host); ;// allStores.FirstOrDefault(s => s.ContainsHostValue(host));

                //if (store == null)
                //{
                //    //load the first found store
                //    store = allStores.FirstOrDefault();
                //}
                if (store == null)
                    throw new Exception("No store could be loaded");

                cachedStore = store;
                return cachedStore;
            }
            set
            {
                if (value == null)
                    throw new Exception("No store could be loaded");

                cachedStore = value;
            }
        }
    }
}
