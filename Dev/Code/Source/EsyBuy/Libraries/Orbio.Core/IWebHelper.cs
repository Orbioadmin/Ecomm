using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Orbio.Core
{

    public interface IWebHelper
    {
        /// <summary>
        /// removes given query strings from the path and rewrites the path
        /// </summary>
        /// <param name="queryParamsToRemove">query params to be removed</param>
        void RemoveQueryFromPath(HttpContextBase context, List<string> queryParamsToRemove);
    }
}
