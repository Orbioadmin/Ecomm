﻿using System.Web.Routing;
using Nop.Web.Framework.Localization;
using Nop.Web.Framework.Mvc.Routes;
using Nop.Web.Framework.Seo;

namespace Orbio.Web.UI.Infrastructure
{
    public partial class GenericUrlRouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //generic URLs
            routes.MapCommonPathRoute("GenericUrl",
                                       "{generic_se_name}",
                                       new { controller = "Common", action = "GenericUrl" },
                                       new[] { "Nop.Web.Controllers" });

            ////define this routes to use in UI views (in case if you want to customize some of them later)
            //routes.MapLocalizedRoute("Product",
            //                         "{SeName}",
            //                         new {controller = "Catalog", action = "Product"},
                                     //new[] {"Nop.Web.Controllers"});

            routes.MapCommonPathRoute("Category",
                            "{SeName}",
                            new { controller = "Catalog", action = "Category" },
                            new[] { "Orbio.Web.UI.Controllers" });


            //shopping cart
            routes.MapCommonPathRoute("ShoppingCart",
                            "cart/",
                            new { controller = "ShoppingCart", action = "Cart" },
                            new[] { "Orbio.Web.UI.Controllers" });


            //checkout
            routes.MapCommonPathRoute("Checkout",
                            "checkout/",
                            new { controller = "Checkout", action = "Index" },
                            new[] { "Orbio.Web.UI.Controllers" });
            ////////wishlist
            //////routes.MapCommonPathRoute("Wishlist",
            //////                "wishlist/{customerGuid}",
            //////                new { controller = "ShoppingCart", action = "Wishlist", customerGuid = UrlParameter.Optional },
            //////                new[] { "Nop.Web.Controllers" });

            //routes.MapLocalizedRoute("Manufacturer",
            //                "{SeName}",
            //                new { controller = "Catalog", action = "Manufacturer" },
            //                new[] { "Nop.Web.Controllers" });

            //routes.MapLocalizedRoute("NewsItem",
            //                "{SeName}",
            //                new { controller = "News", action = "NewsItem" },
            //                new[] { "Nop.Web.Controllers" });

            //routes.MapLocalizedRoute("BlogPost",
            //                "{SeName}",
            //                new { controller = "Blog", action = "BlogPost" },
            //                new[] { "Nop.Web.Controllers" });



            //the last route. it's used when none of registered routes could be used for the current request
            //but it this case we cannot process non-registered routes (/controller/action)
            //routes.MapLocalizedRoute(
            //    "PageNotFound-Wildchar",
            //    "{*url}",
            //    new { controller = "Common", action = "PageNotFound" },
            //    new[] { "Nop.Web.Controllers" });
        }

        public int Priority
        {
            get
            {
                //it should be the last route
                //we do not set it to -int.MaxValue so it could be overriden (if required)
                return -1000000;
            }
        }
    }
}
