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
    public sealed class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var store = EngineContext.Current.Resolve<IStoreContext>();
            if (!workContext.CurrentCustomer.IsRegistered)
            {
                filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary { 
                            
                            { "controller", "Account" }, 
                            { "action", "Login" },
                            //{ "returnUrl", filterContext.HttpContext.Request.RawUrl } // how do I get this?
                        }
                   );
            }
            genericAttributeService.SaveGenericAttributes("save", workContext.CurrentCustomer.Id, "Customer", SystemCustomerAttributeNames.LastContinueShoppingPage, filterContext.HttpContext.Request.RawUrl, store.CurrentStore.Id);

        }
    }
}