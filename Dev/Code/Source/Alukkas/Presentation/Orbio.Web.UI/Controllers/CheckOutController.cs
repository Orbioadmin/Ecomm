using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orbio.Web.UI.Filters;

namespace Orbio.Web.UI.Controllers
{
    public class CheckOutController : Controller
    {
        //
        // GET: /CheckOut/

        [LoginRequired]
        public ActionResult Index()
        {
            return View();
        
        }

        [LoginRequired]
        public ActionResult Shipping()
        {
            //testdata remove this
            return Json("Success");
        }

    }
}
