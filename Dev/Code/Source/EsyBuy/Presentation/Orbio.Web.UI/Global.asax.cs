using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Mvc;

namespace Orbio.Web.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            //initialize engine context
            EngineContext.Initialize(false);

            

            //set dependency resolver
            var dependencyResolver = new NopDependencyResolver();
            DependencyResolver.SetResolver(dependencyResolver);
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            //madhu MB 
            //TODO: add code to check if topmenu has changed
            var paramChanged = "topmenu";
            if (custom.Equals("topmenu"))
            {
                return paramChanged;
            }
            return base.GetVaryByCustomString(context, custom);
        }
    }
}