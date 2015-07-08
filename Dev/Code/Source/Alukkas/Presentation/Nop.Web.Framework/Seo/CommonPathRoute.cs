using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Events;
using Orbio.Services.Seo;

namespace Nop.Web.Framework.Seo
{
    /// <summary>
    /// Provides properties and methods for defining a SEO friendly route, and for getting information about the route.
    /// </summary>
    public class CommonPathRoute : Route
    {
         #region Constructors

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern and handler class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public CommonPathRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern, handler class and default parameter values.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public CommonPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern, handler class, default parameter values and constraints.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public CommonPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern, handler class, default parameter values, 
        /// constraints,and custom values.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. The route handler might need these values to process the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public CommonPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Returns information about the requested route.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        /// An object that contains the values from the route definition.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var data = base.GetRouteData(httpContext);
            var path = GetQueryParam(httpContext, "p");
            if (!string.IsNullOrEmpty(path) && data != null) // && DataSettingsHelper.DatabaseIsInstalled())
            {

                //var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
                var slug = data.Values["SeName"] as string;
                //var urlRecord = urlRecordService.GetBySlug(slug);
                //if (urlRecord == null)
                //{
                //    //no URL record found

                //    //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                //    //var response = httpContext.Response;
                //    //response.Status = "302 Found";
                //    //response.RedirectLocation = webHelper.GetStoreLocation(false);
                //    //response.End();
                //    //return null;

                //    data.Values["controller"] = "Common";
                //    data.Values["action"] = "PageNotFound";
                //    return data;
                //}
                //if (!urlRecord.IsActive)
                //{
                //    //URL record is not active. let's find the latest one
                //    var activeSlug = urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, urlRecord.LanguageId);
                //    if (!string.IsNullOrWhiteSpace(activeSlug))
                //    {
                //        //the active one is found
                //        var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                //        var response = httpContext.Response;
                //        response.Status = "301 Moved Permanently";
                //        response.RedirectLocation = string.Format("{0}{1}", webHelper.GetStoreLocation(false), activeSlug);
                //        response.End();
                //        return null;
                //    }
                //    else
                //    {
                //        //no active slug found

                //        //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                //        //var response = httpContext.Response;
                //        //response.Status = "302 Found";
                //        //response.RedirectLocation = webHelper.GetStoreLocation(false);
                //        //response.End();
                //        //return null;

                //        data.Values["controller"] = "Common";
                //        data.Values["action"] = "PageNotFound";
                //        return data;
                //    }
                //}

                //process URL
                switch (path)
                {
                    case "pt":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Product";
                           // data.Values["productid"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                        }
                        break;
                    case "ct":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Category";
                           // data.Values["categoryid"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                            data.Values["keyword"] = GetQueryParam(httpContext, "q");
                            data.Values["filterIds"] = GetQueryParam(httpContext, "filterIds");
                            data.Values["minPrice"] = GetQueryParam(httpContext, "minPrice");
                            data.Values["maxPrice"] = GetQueryParam(httpContext, "maxPrice");

                        }
                        break;
                    case "sr":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Search";
                            // data.Values["categoryid"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                            data.Values["keyword"] = GetQueryParam(httpContext, "q");
                            data.Values["filterIds"] = GetQueryParam(httpContext, "filterIds");
                            data.Values["minPrice"] = GetQueryParam(httpContext, "minPrice");
                            data.Values["maxPrice"] = GetQueryParam(httpContext, "maxPrice");

                        }
                        break;
                    case "mn":
                        {
                            data.Values["controller"] = "Catalog";
                            data.Values["action"] = "Manufacturer";
                            //data.Values["manufacturerid"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                        }
                        break;
                    case "ns":
                        {
                            data.Values["controller"] = "News";
                            data.Values["action"] = "NewsItem";
                           // data.Values["newsItemId"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                        }
                        break;
                    case "bt":
                        {
                            data.Values["controller"] = "Blog";
                            data.Values["action"] = "BlogPost";
                            //data.Values["blogPostId"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                        }
                        break;
                    case "ft":
                        {
                            data.Values["controller"] = "Home";
                            data.Values["action"] = "Policies";
                            // data.Values["productid"] = urlRecord.EntityId;
                            data.Values["SeName"] = slug;// urlRecord.Slug;
                        }
                        break;
                    default:
                        {
                            data.Values["controller"] = "Common";
                            data.Values["action"] = "PageNotFound";
                          //  return data;
                            //no record found

                            //generate an event this way developers could insert their own types
                            //EngineContext.Current.Resolve<IEventPublisher>()
                            //    .Publish(new CustomUrlRecordEntityNameRequested(data, new Nop.Core.Domain.Seo.UrlRecord
                            //    {
                            //        EntityId = urlRecord.EntityId,
                            //        EntityName = urlRecord.EntityName,
                            //        Slug = urlRecord.Slug,
                            //        IsActive = urlRecord.IsActive,
                            //        LanguageId = urlRecord.LanguageId
                            //    }));
                        }
                        break;
                }
            }
            else if(data!=null)
            {
                path = data.Values["SeName"] as string;

                switch (path)
                {
                    case "cart":
                        {
                            data.Values["controller"] = "ShoppingCart";
                            data.Values["action"] = "Cart";
                            //data.Values["blogPostId"] = urlRecord.EntityId;
                            
                        }
                        break;
                    case "checkout":
                        {
                            data.Values["controller"] = "CheckOut";
                            data.Values["action"] = "Index";
                            //data.Values["blogPostId"] = urlRecord.EntityId;

                        }
                        break;
                    default:
                          data.Values["controller"] = "Common";
                           data.Values["action"] = "PageNotFound";
                        break;
                }
              
                //return data;
            }
            return data;
        }

        private static string GetQueryParam(HttpContextBase httpContext, string paramName)
        {
            var param = string.Empty;
            if (httpContext.Request.Params.Count > 0 && httpContext.Request.Params[paramName] != null)
            {
                param = httpContext.Request.Params[paramName].ToLowerInvariant();
            }
            return param;
        }

        #endregion
    }
}
