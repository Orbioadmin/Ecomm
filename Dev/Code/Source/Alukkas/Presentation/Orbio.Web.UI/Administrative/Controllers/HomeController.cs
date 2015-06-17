using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Core.Infrastructure;
using System.Web.Mvc;

namespace Orbio.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TopMenu()
        {
            return PartialView();
        }
    }
}
