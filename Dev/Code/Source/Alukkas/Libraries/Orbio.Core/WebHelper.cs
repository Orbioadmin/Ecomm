using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Orbio.Core
{
    public class WebHelper:IWebHelper
    {
        private readonly HttpContextBase httpContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        public WebHelper(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        /// <summary>
        /// removes given query strings from the path and rewrites the path
        /// </summary>
        /// <param name="queryParamsToRemove">query params to be removed</param>
        public void RemoveQueryFromPath(HttpContextBase context, List<string> queryParamsToRemove)
        {
            var queryString = new NameValueCollection(context.Request.QueryString);

            foreach (var query in queryParamsToRemove)
            {
                queryString.Remove(query);
            }
            var newQueryString = "";

            for (var i = 0; i < queryString.Count; i++)
            {
                if (i > 0) newQueryString += "&";
                newQueryString += queryString.GetKey(i) + "=" + queryString[i];
            }

            var newPath = context.Request.Path + (!String.IsNullOrEmpty(newQueryString) ? "?" + newQueryString : String.Empty);
            context.RewritePath(newPath);
        }

        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file.</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        public virtual bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <returns>Page name</returns>
        public virtual string GetThisPageUrl(bool includeQueryString)
        {
            bool useSsl = IsCurrentConnectionSecured();
            return GetThisPageUrl(includeQueryString, useSsl);
        }


        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <param name="useSsl">Value indicating whether to get SSL protected page</param>
        /// <returns>Page name</returns>
        public virtual string GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            string url = string.Empty;
            if (httpContext == null || httpContext.Request == null)
                return url;

            if (includeQueryString)
            {
                string storeHost = GetStoreHost(useSsl);
                if (storeHost.EndsWith("/"))
                    storeHost = storeHost.Substring(0, storeHost.Length - 1);
                url = storeHost + httpContext.Request.RawUrl;
            }
            else
            {
                if (httpContext.Request.Url != null)
                {
                    url = httpContext.Request.Url.GetLeftPart(UriPartial.Path);
                }
            }
            url = url.ToLowerInvariant();
            return url;
        }

        /// <summary>
        /// Gets store location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store location</returns>
        public virtual string GetStoreLocation(bool useSsl)
        {
            //return HostingEnvironment.ApplicationVirtualPath;

            string result = GetStoreHost(useSsl);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            if (httpContext != null && httpContext.Request != null)
                result = result + httpContext.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            bool useSsl = false;
            if (httpContext != null && httpContext.Request != null)
            {
                useSsl = httpContext.Request.IsSecureConnection;
                //when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                //just uncomment it
                //useSSL = _httpContext.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSsl;
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store host location</returns>
        public virtual string GetStoreHost(bool useSsl)
        {
            var result = "";
            var httpHost = ServerVariables("HTTP_HOST");
            if (!String.IsNullOrEmpty(httpHost))
            {
                result = "http://" + httpHost;
                if (!result.EndsWith("/"))
                    result += "/";
            }

            //if (DataSettingsHelper.DatabaseIsInstalled())
            //{
                #region Database is installed

                //let's resolve IWorkContext  here.
                ////Do not inject it via contructor because it'll cause circular references
                //var storeContext = EngineContext.Current.Resolve<IStoreContext>();
                //var currentStore = storeContext.CurrentStore;
                //if (currentStore == null)
                //    throw new Exception("Current store cannot be loaded");

                //if (String.IsNullOrWhiteSpace(httpHost))
                //{
                //    //HTTP_HOST variable is not available.
                //    //This scenario is possible only when HttpContext is not available (for example, running in a schedule task)
                //    //in this case use URL of a store entity configured in admin area
                //    result = currentStore.Url;
                //    if (!result.EndsWith("/"))
                //        result += "/";
                //}

                if (useSsl)
                {
                    //if (!String.IsNullOrWhiteSpace(currentStore.SecureUrl))
                    //{
                    //    //Secure URL specified. 
                    //    //So a store owner don't want it to be detected automatically.
                    //    //In this case let's use the specified secure URL
                    //    result = currentStore.SecureUrl;
                    //}
                    //else
                    //{
                        //Secure URL is not specified.
                        //So a store owner wants it to be detected automatically.
                        result = result.Replace("http:/", "https:/");
                    //}
                }
                //else
                //{
                //    if (currentStore.SslEnabled && !String.IsNullOrWhiteSpace(currentStore.SecureUrl))
                //    {
                //        //SSL is enabled in this store and secure URL is specified.
                //        //So a store owner don't want it to be detected automatically.
                //        //In this case let's use the specified non-secure URL
                //        result = currentStore.Url;
                //    }
                //}
                #endregion
            //}
            //else
            //{
            //    #region Database is not installed
            //    if (useSsl)
            //    {
            //        //Secure URL is not specified.
            //        //So a store owner wants it to be detected automatically.
            //        result = result.Replace("http:/", "https:/");
            //    }
            //    #endregion
            //}


            if (!result.EndsWith("/"))
                result += "/";
            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Server variable</returns>
        public virtual string ServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                if (httpContext == null || httpContext.Request == null)
                    return result;

                //put this method is try-catch 
                //as described here http://www.nopcommerce.com/boards/t/21356/multi-store-roadmap-lets-discuss-update-done.aspx?p=6#90196
                if (httpContext.Request.ServerVariables[name] != null)
                {
                    result = httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }



        /// <summary>
        /// Gets store location
        /// </summary>
        /// <returns>Store location</returns>
        public virtual string GetStoreLocation()
        {
            bool useSsl = IsCurrentConnectionSecured();
            return GetStoreLocation(useSsl);
        }

        /// <summary>
        /// Get a value indicating whether the request is made by search engine (web crawler)
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Result</returns>
        public virtual bool IsSearchEngine(HttpContextBase context)
        {
            //we accept HttpContext instead of HttpRequest and put required logic in try-catch block
            //more info: http://www.nopcommerce.com/boards/t/17711/unhandled-exception-request-is-not-available-in-this-context.aspx
            if (context == null)
                return false;

            bool result = false;
            try
            {
                result = context.Request.Browser.Crawler;
                if (!result)
                {
                    //put any additional known crawlers in the Regex below for some custom validation
                    //var regEx = new Regex("Twiceler|twiceler|BaiDuSpider|baduspider|Slurp|slurp|ask|Ask|Teoma|teoma|Yahoo|yahoo");
                    //result = regEx.Match(request.UserAgent).Success;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }
            return result;
        }

        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (httpContext == null || httpContext.Request == null)
                return string.Empty;

            if (httpContext.Request.Headers != null)
            {
                //look for the X-Forwarded-For (XFF) HTTP header field
                //it's used for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer. 
                string xff = httpContext.Request.Headers.AllKeys
                    .Where(x => "X-FORWARDED-FOR".Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => httpContext.Request.Headers[k])
                    .FirstOrDefault();

                //if you want to exclude private IP addresses, then see http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc

                if (!String.IsNullOrEmpty(xff))
                {
                    string lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    if (!String.IsNullOrEmpty(lastIp))
                    {
                        return lastIp;
                    }
                }
            }

            if (httpContext.Request.UserHostAddress != null)
            {
                return httpContext.Request.UserHostAddress;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            //URL referrer is null in some case (for example, in IE 8)
            if (httpContext != null &&
                httpContext.Request != null &&
                httpContext.Request.UrlReferrer != null)
                referrerUrl = httpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }
    }
}
