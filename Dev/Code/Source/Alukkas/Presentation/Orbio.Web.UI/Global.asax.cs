﻿using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Mvc;
using System.Configuration;
using Orbio.Services.Logging;
using Orbio.Core;
using Orbio.Web.UI.Controllers;

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

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //we don't do it in Application_BeginRequest because a user is not authenticated yet
            SetWorkingCulture();
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                if (!webHelper.IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController errorController = new CommonController();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Common");
                    routeData.Values.Add("action", "PageNotFound");

                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
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

        protected void SetWorkingCulture()
        {
            //if (!DataSettingsHelper.DatabaseIsInstalled())
            //    return;

            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<Orbio.Core.IWebHelper>();
            if (webHelper.IsStaticResource(this.Request))
                return;

            ////keep alive page requested (we ignore it to prevent creating a guest customer records)
            //string keepAliveUrl = string.Format("{0}keepalive/index", webHelper.GetStoreLocation());
            //if (webHelper.GetThisPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
            //    return;


            if (webHelper.GetThisPageUrl(false).StartsWith(string.Format("{0}admin", webHelper.GetStoreLocation()),
                StringComparison.InvariantCultureIgnoreCase))
            {
                //admin area


                //always set culture to 'en-US'
                //we set culture of admin area to 'en-US' because current implementation of Telerik grid 
                //doesn't work well in other cultures
                //e.g., editing decimal value in russian culture
                var culture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            //else
            //{
            //    //public store
            //    var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            //    var curcustomer = workContext.CurrentCustomer;
            //    //if (workContext.CurrentCustomer != null && workContext.WorkingLanguage != null)
            //    //{
            //    //    var culture = new CultureInfo(workContext.WorkingLanguage.LanguageCulture);
            //    //    Thread.CurrentThread.CurrentCulture = culture;
            //    //    Thread.CurrentThread.CurrentUICulture = culture;
            //    //}
            //}
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            var log404Errors =  ConfigurationManager.AppSettings["Log404Errors"]!=null && 
                ConfigurationManager.AppSettings["Log404Errors"].Equals("true");

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404 &&
                !log404Errors)
                return;

            try
            {
                //log
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                logger.Error(exc.Message, exc, workContext.CurrentCustomer);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}