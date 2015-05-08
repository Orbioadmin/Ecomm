using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core.Infrastructure;
using Orbio.Web.UI.Models.Home;
using Orbio.Core.Domain.Email;
using System.Data.SqlClient;
using System.Configuration;
using Orbio.Services.Email;
using Orbio.Core.Domain.Customers;
using Orbio.Core;
using Orbio.Web.UI.Filters;
using Orbio.Services.Common;

namespace Orbio.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService emailService;
        private readonly IStoreContext storeContext;

        public HomeController(IEmailService emailService, IStoreContext storeContext)
        {
            this.emailService = emailService;
            this.storeContext = storeContext;
        }
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

        [ContinueShoppingAttribute]
        public ActionResult ContactUs()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ContactUs(ContactModel model)
        //{
        //    //ViewBag.Message = "Your contact page.";
        //    if (ModelState.IsValid)
        //    {
        //        var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
        //        string returnUrl = workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.LastContinueShoppingPage, storeContext.CurrentStore.Id);
        //        var Sent = new EmailDetail();
        //        Sent.FromAddress = model.Email;
        //        Sent.FromName = model.Name;
        //        Sent.Password = ConfigurationManager.AppSettings["EmailPassword"];
        //        Sent.ToAddress = ConfigurationManager.AppSettings["EmailAddress"];
        //        Sent.ToName = ConfigurationManager.AppSettings["EmailName"];
        //        Sent.Subject = "Your contact page";
        //        Sent.Body = model.Message;
        //        emailService.SendNotification(model.Email, model.Name, model.Message, Sent.Subject);
        //        emailService.SentEmail(Sent);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    return View(model);
        //}

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
