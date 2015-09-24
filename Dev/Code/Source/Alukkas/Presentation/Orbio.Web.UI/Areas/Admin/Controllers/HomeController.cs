using Orbio.Web.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        [AdminAuthorizeAttribute]
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