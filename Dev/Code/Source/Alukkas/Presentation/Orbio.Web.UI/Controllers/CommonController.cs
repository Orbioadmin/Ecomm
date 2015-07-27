using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

    }
}
