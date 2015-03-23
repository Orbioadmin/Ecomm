using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Orbio.Web.UI.Models.Home;

namespace Orbio.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PriceBar()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curcustomer = workContext.CurrentCustomer;
            var model = new PriceBarModel { UserNameOrEmail =  string.IsNullOrEmpty(curcustomer.Username)?"Guest":curcustomer.Username };
            return PartialView(model);
        }

        public ActionResult Policies(string SeName)
        {
            return View(SeName);
        }
    }
}
