using System;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Orbio.Services.Common;
using Orbio.Core.Domain.Customers;
using System.Web;

namespace Orbio.Web.UI.Filters
{
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
         public override void OnActionExecuting(ActionExecutingContext filterContext)
         {
             var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
             if (!workContext.CurrentCustomer.IsAdmin)
             {
                 filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { 
                            
                            { "controller", "Account" }, 
                            { "action", "Login" },
                            { "Area", String.Empty }
                            //{ "returnUrl", filterContext.HttpContext.Request.RawUrl } // how do I get this?
                        }
                    );

             }

         }
    }
}