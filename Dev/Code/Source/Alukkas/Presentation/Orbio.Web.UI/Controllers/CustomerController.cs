using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Orbio.Web.UI.Filters;
using Orbio.Web.UI.Models.Home;

namespace Orbio.Web.UI.Controllers
{
    public class CustomerController : Controller
    {
        [LoginRequiredAttribute]
        public ActionResult MyAccount()
        {
            return View();
        }

     }
}