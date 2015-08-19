using System;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Orbio.Services.Common;
using Orbio.Core.Domain.Customers;
using System.Web;
using Orbio.Core;

namespace Orbio.Web.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ContinueShoppingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var store = EngineContext.Current.Resolve<IStoreContext>();
            if (filterContext.HttpContext.Request.UrlReferrer != null )
                genericAttributeService.SaveGenericAttributes(workContext.CurrentCustomer.Id, "Customer", SystemCustomerAttributeNames.LastContinueShoppingPage, filterContext.HttpContext.Request.UrlReferrer.PathAndQuery, store.CurrentStore.Id);

        }
    }
}