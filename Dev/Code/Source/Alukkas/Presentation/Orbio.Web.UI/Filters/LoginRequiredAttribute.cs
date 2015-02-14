using System;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Core.Infrastructure;

namespace Orbio.Web.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            if (!workContext.CurrentCustomer.IsRegistered)
            {
                filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary { 
                            
                            { "controller", "Account" }, 
                            { "action", "Login" },
                            { "returnUrl", filterContext.HttpContext.Request.RawUrl } // how do I get this?
                        }
                   );
            }

        }
    }
}