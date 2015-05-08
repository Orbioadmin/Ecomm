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

        [HttpPost]
        public ActionResult ContactUs(ContactModel model)
        {
            //ViewBag.Message = "Your contact page.";
            if (ModelState.IsValid)
            {
                var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                string returnUrl = workContext.CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.LastContinueShoppingPage, storeContext.CurrentStore.Id);
                var sentemail = new Mail_Sending();
                sentemail.FromAddress = model.Email;
                sentemail.FromName = model.Name;
                sentemail.Password = ConfigurationManager.AppSettings["EmailPassword"];
                sentemail.ToAddress = ConfigurationManager.AppSettings["EmailFromAddress"];
                sentemail.ToName = ConfigurationManager.AppSettings["EmailFromName"];
                sentemail.Subject = "Your contact page";
                sentemail.Body = model.Message;
                emailService.SendNotification(model.Email, model.Name, model.Message, sentemail.Subject);
                emailService.SentEmail(sentemail);
                return RedirectToLocal(returnUrl);
            }
            return View(model);
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
