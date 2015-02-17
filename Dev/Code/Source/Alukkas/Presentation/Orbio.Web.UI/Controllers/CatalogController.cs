using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Services.Catalog;
using Orbio.Web.UI.Infrastructure.Cache;
using Orbio.Web.UI.Models.Catalog;

namespace Orbio.Web.UI.Controllers
{
    /// <summary>
    /// catalog controller
    /// </summary>
    public class CatalogController : Controller
    {
        private readonly ICategoryService categoryService;

        private readonly IProductService productService;
       // private readonly IWorkContext workContext;
       // private readonly IStoreContext storeContext;

        private readonly ICacheManager cacheManager;
       // private readonly CatalogSettings catalogSettings;
       private readonly IWebHelper webHelper;

        /// <summary>
        /// instantiates catalog controller
        /// </summary>
        public CatalogController(ICategoryService categoryService, IProductService productService,
            //IWorkContext workContext,
            //IStoreContext storeContext,       
            //CatalogSettings catalogSettings,          
            ICacheManager cacheManager,
            IWebHelper webHelper
            )
        {
            this.categoryService = categoryService;

            this.productService = productService;
            //this.workContext = workContext;
            //this.storeContext = storeContext;

            //this.catalogSettings = catalogSettings;

            this.cacheManager = cacheManager;
            this.webHelper = webHelper;
        }

        /// <summary>
        /// action to return topmenu data
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
       // [OutputCache(VaryByCustom="topmenu", Duration=86400)]
        public ActionResult TopMenu()
        {
            // var customerRolesIds = workContext.CurrentCustomer.CustomerRoles
            //    .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            //string cacheKey = string.Format(ModelCacheEventConsumer.CATEGORY_MENU_MODEL_KEY, workContext.WorkingLanguage.Id,
            //    string.Join(",", customerRolesIds), storeContext.CurrentStore.Id);

      
            int flag = (ConfigurationManager.AppSettings["LoadTopMenufromDb"].ToString() != "")?Convert.ToInt32(ConfigurationManager.AppSettings["LoadTopMenufromDb"]):1;
            if (flag == 1)
            {

                var cachedModel = cacheManager.Get(string.Format(ModelCacheEventConsumer.CATEGORY_MENU_MODEL_KEY, 1, 4, 1),
                    () => PrepareCategorySimpleModels());
                var model = new TopMenuModel()
                 {
                     Categories = cachedModel,
                     // RecentlyAddedProductsEnabled = catalogSettings.RecentlyAddedProductsEnabled,
                     BlogEnabled = false, //blogSettings.Enabled,
                     ForumEnabled = false //forumSettings.ForumsEnabled
                 };
                ViewBag.NumberOfCategory = Convert.ToInt32(ConfigurationManager.AppSettings["TopMenuNoOfCategory"]);
                return PartialView(model);
            }
            else
            {
                 var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                
                ViewBag.UserName = string.IsNullOrEmpty(workContext.CurrentCustomer.Username) ? "Guest" : workContext.CurrentCustomer.Username;
                return PartialView("TopMenuStatic");
            }
        }

        public ActionResult Category(string seName,string spec, string minPrice, string maxPrice)
        {
            var model = PrepareCategoryProductModel(seName, spec,  minPrice, maxPrice);
           
            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string>{{"spec"}});
            ViewBag.MetaDescription = model.MetaDescription;
            ViewBag.MetaKeywords = model.MetaKeywords;            
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ProductFilter(int categoryId, int minPrice, int maxPrice, int[] selectedSpecs)
        {
            var model = PrepareSpecificationFilterModel(categoryId, minPrice, maxPrice, selectedSpecs);

            return PartialView(model);
        }

        private List<SpecificationAttribute> PrepareSpecificationFilterModel(int categoryId,int minPrice, int maxPrice, int[] selectedSpecs)
        {
            var specFilterModels = categoryService.GetSpecificationFiltersByCategoryId(categoryId);
            var specFilterByspecAttribute = from sa in specFilterModels
                                            group sa by sa.SpecificationAttributeName;
            var currentUrl = ControllerContext.RequestContext.HttpContext.Request.Url.AbsoluteUri;

            //var specs = selectedSpecs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries); 
            var model = (from sag in specFilterByspecAttribute
                         select new SpecificationAttribute
                         {
                             Type = "Specification",
                             Name = sag.Key,
                             SpecificationAttributeOptions =
                                 new List<SpecificationAttributeOption>((from sao in sag
                                                                         select new SpecificationAttributeOption
                                                                         {
                                                                             Id = sao.SpecificationAttributeOptionId,
                                                                             Name = sao.SpecificationAttributeOptionName,
                                                                             FilterUrl = currentUrl ,
                                                                             Selected = selectedSpecs!=null && selectedSpecs.Length>0 && selectedSpecs.Any(i => i == sao.SpecificationAttributeOptionId)
                                                                         }))
                         }).ToList();
            if (model.Count > 0)
            {
                var priceFilterIndex = Convert.ToInt32(ConfigurationManager.AppSettings["PriceFilterIndex"] == null ? "1" : ConfigurationManager.AppSettings["PriceFilterIndex"]);
                if (priceFilterIndex > model.Count)
                {
                    priceFilterIndex = model.Count;
                }
                model.Insert(priceFilterIndex, new SpecificationAttribute
                {
                    Name = "Price",
                    Type = "Price",
                    SpecificationAttributeOptions = new List<SpecificationAttributeOption> { 
                {new SpecificationAttributeOption{Name=minPrice.ToString(), ElementName="productPriceFilterMinValue"}},
                {new SpecificationAttributeOption{Name=maxPrice.ToString(), ElementName="productPriceFilterMaxValue"}}
                }
                });
            }
            return model;
        }

        public ActionResult Product(string seName)
        {
            var model = PrepareProductdetailsModel(seName);
            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" } });
            return View(model);
        }


        private CategoryModel PrepareCategoryProductModel(string seName, string filterIds, string minPrice, string maxPrice)
        {
            
            var model = new CategoryModel(categoryService.GetProductsBySlug(seName, string.IsNullOrEmpty(filterIds)?null:filterIds,
                string.IsNullOrEmpty(minPrice) ? (decimal?)null : Convert.ToDecimal(minPrice), string.IsNullOrEmpty(maxPrice) ? (decimal?)null : Convert.ToDecimal(maxPrice)));
            if (filterIds != null)
            {
                model.SelectedSpecificationAttributeIds = (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                           select Convert.ToInt32(f)).ToArray();
            }
            return model;
        }

        private ProductOverViewModel PrepareProductdetailsModel(string seName)
        {
            var model = new ProductDetailModel(productService.GetProductsDetailsBySlug(seName));

            return model;
        }

        private IList<CategorySimpleModel> PrepareCategorySimpleModels()
        {
           return (from c in categoryService.GetTopMenuCategories()
                                select new CategorySimpleModel(c)).ToList();
        }

    }
}
