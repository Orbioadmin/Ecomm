using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Orbio.Core
{
    public class WebHelper:IWebHelper
    {
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
    }
}
