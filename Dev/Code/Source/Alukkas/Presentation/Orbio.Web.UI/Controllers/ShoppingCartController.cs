using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Controllers
{
    public class ShoppingCartController : Controller
    {
         

        public ActionResult Cart()
        {
            return View();
        }

    }
}
