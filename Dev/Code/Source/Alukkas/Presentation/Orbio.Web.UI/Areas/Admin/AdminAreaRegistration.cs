using System.Web.Mvc;
using System.Web.Optimization;

namespace Orbio.Web.UI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Orbio.Web.UI.Areas.Admin.Controllers" }
            );
            RegisterBundles();
        }

        private void RegisterBundles()
        {
            BundleConfig.RegisterBundlesAdministration(BundleTable.Bundles);
        }     
    }
}