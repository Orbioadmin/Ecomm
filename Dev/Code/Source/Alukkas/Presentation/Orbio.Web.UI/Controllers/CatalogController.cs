﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Nop.Core.Caching;
using Nop.Core.Infrastructure;
using Orbio.Core;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Catalog;
using Orbio.Web.UI.Infrastructure.Cache;
using Orbio.Web.UI.Models.Catalog;
using Orbio.Services.Orders;
using System.Xml;
using System.Diagnostics;

namespace Orbio.Web.UI.Controllers
{
    /// <summary>
    /// catalog controller
    /// </summary>
    public class CatalogController : Controller
    {
        private readonly ICategoryService categoryService;

        private readonly IProductService productService;

        private readonly IShoppingCartService shoppingcartservice;
        // private readonly IWorkContext workContext;
        // private readonly IStoreContext storeContext;

        private readonly ICacheManager cacheManager;
        // private readonly CatalogSettings catalogSettings;
        private readonly IWebHelper webHelper;

        /// <summary>
        /// instantiates catalog controller
        /// </summary>
        public CatalogController(ICategoryService categoryService, IProductService productService, IShoppingCartService shoppingcartservice,
            //IWorkContext workContext,
            //IStoreContext storeContext,       
            //CatalogSettings catalogSettings,          
            ICacheManager cacheManager,
            IWebHelper webHelper
            )
        {
            this.categoryService = categoryService;

            this.productService = productService;

            this.shoppingcartservice = shoppingcartservice;
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


            int flag = (ConfigurationManager.AppSettings["LoadTopMenufromDb"].ToString() != "") ? Convert.ToInt32(ConfigurationManager.AppSettings["LoadTopMenufromDb"]) : 1;
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

        [ChildActionOnly]
        public ActionResult SearchBox()
        {

            var cachedModel = cacheManager.Get(string.Format(ModelCacheEventConsumer.CATEGORY_MENU_MODEL_KEY, 1, 4, 1),
                () => PrepareCategorySimpleModels());
            var model = new SearchModel()
            {
                Categories = cachedModel
            };
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();

            ViewBag.UserName = string.IsNullOrEmpty(workContext.CurrentCustomer.Username) ? "Guest" : workContext.CurrentCustomer.Username;
            return PartialView("SearchBox", model);
        }

        public ActionResult Search(string seName, string spec, string minPrice, string maxPrice, string keyword)
        {
            var model = new SearchModel();
            if (keyword.Length >= 3)
            {

                model = PrepareCategoryProductModelBySearch(seName, spec, minPrice, maxPrice, keyword);
                //ViewBag.MetaDescription = model.MetaDescription;
                //ViewBag.MetaKeywords = model.MetaKeywords;
            }
            else
            {
                model = PrepareCategoryProductModelBySearch(seName, spec, minPrice, maxPrice, "0");
                ViewBag.Error = "Search term minimum length is 3 characters";
            }
            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" } });
            return View(model);
        }

        public ActionResult Category(string seName, string spec, string minPrice, string maxPrice, string keyword)
        {
            var model = PrepareCategoryProductModel(seName, spec, minPrice, maxPrice, keyword);

            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" } });
            ViewBag.MetaDescription = model.MetaDescription;
            ViewBag.MetaKeywords = model.MetaKeywords;
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ProductFilter(int categoryId, int minPrice, int maxPrice, int[] selectedSpecs, string keyword)
        {
            var model = PrepareSpecificationFilterModel(categoryId, minPrice, maxPrice, selectedSpecs, keyword);

            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult ProductFilterBySearch(string categoryId, int minPrice, int maxPrice, int[] selectedSpecs, string keyword)
        {
            var model = PrepareSpecificationFilterModelBySearch(categoryId, minPrice, maxPrice, selectedSpecs, keyword);

            return PartialView("ProductFilter", model);
        }
        private List<SpecificationAttribute> PrepareSpecificationFilterModel(int categoryId, int minPrice, int maxPrice, int[] selectedSpecs, string keyword)
        {
            var specFilterModels = categoryService.GetSpecificationFiltersByCategoryId(categoryId, keyword);
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
                                                                             FilterUrl = currentUrl,
                                                                             Selected = selectedSpecs != null && selectedSpecs.Length > 0 && selectedSpecs.Any(i => i == sao.SpecificationAttributeOptionId)
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

        private List<SpecificationAttribute> PrepareSpecificationFilterModelBySearch(string categoryId, int minPrice, int maxPrice, int[] selectedSpecs, string keyword)
        {
            var specFilterModels = categoryService.GetSpecificationFiltersByCategory(string.IsNullOrEmpty(categoryId) ? "0" : categoryId, keyword);
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
                                                                             FilterUrl = currentUrl,
                                                                             Selected = selectedSpecs != null && selectedSpecs.Length > 0 && selectedSpecs.Any(i => i == sao.SpecificationAttributeOptionId)
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

        [HttpPost]
        public ActionResult Product(ProductDetailModel product, ShoppingCartType cartType, FormCollection formkey)
        {
            TempData.Add("product", product);
            TempData.Add("cartType", cartType);
            //TempData.Remove("cartType");
            //var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            //var curcustomer = workContext.CurrentCustomer;
            //string selectedAttributes = string.Empty;
            //int count = 0;
            //foreach (var attribute in product.ProductVariantAttributes)
            //{
            //    switch (attribute.AttributeControlType)
            //    {
            //        case Orbio.Core.Domain.Catalog.AttributeControlType.TableBlock:
            //            {
            //                foreach (var values in attribute.Values)
            //                {
            //                    if (values.Id !=0)
            //                    {
            //                        int selectedAttributeId = int.Parse(values.Id.ToString());
            //                        selectedAttributes = AddCartProductAttribute(selectedAttributes,
            //                                   attribute, selectedAttributeId.ToString());
            //                    }
            //                }

            //                break;
            //            }
            //    }
            //    count++;
            //}
            //shoppingcartservice.AddCartItem("add", Convert.ToInt32(cartType), curcustomer.Id, product.Id, selectedAttributes,Convert.ToInt32(product.SelectedQuantity));
            return RedirectToRoute("Category", new { p = "pt", seName = product.SeName });
        }

        public ActionResult Product(string seName)
        {
            ProductDetailModel selectedProduct = null;
            ShoppingCartType selectedcarttype;
            ViewBag.Errors = string.Empty;
            if (TempData.ContainsKey("product"))
            {
                selectedProduct = (ProductDetailModel)TempData["product"];
                selectedcarttype = (ShoppingCartType)TempData["cartType"];
                var errorString = string.Empty;
                if (selectedProduct.ProductVariantAttributes.Count > 0)
                {

                    errorString = selectedProduct.ProductVariantAttributes.GetProductVariantErrors();
                }
                if (string.IsNullOrEmpty(errorString))
                {
                    var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                    var curcustomer = workContext.CurrentCustomer;
                    string selectedAttributes = string.Empty;
                    int count = 0;
                    foreach (var attribute in selectedProduct.ProductVariantAttributes)
                    {
                        switch (attribute.AttributeControlType)
                        {
                            case Orbio.Core.Domain.Catalog.AttributeControlType.TableBlock:
                                {
                                    foreach (var values in attribute.Values)
                                    {
                                        if (values.Id != 0)
                                        {
                                            int selectedAttributeId = int.Parse(values.Id.ToString());
                                            selectedAttributes = AddCartProductAttribute(selectedAttributes,
                                                       attribute, selectedAttributeId.ToString());
                                        }
                                    }

                                    break;
                                }
                        }
                        count++;
                    }
                    shoppingcartservice.AddCartItem("add", Convert.ToInt32(selectedcarttype), curcustomer.Id, selectedProduct.Id, selectedAttributes, Convert.ToInt32(selectedProduct.SelectedQuantity));
                    ViewBag.Sucess = "Cart Added";
                }
                ViewBag.Errors = errorString;

            }
            var model = PrepareProductdetailsModel(seName);
            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            model.SeName = seName;
            if (selectedProduct != null)
            {
                model.SelectedQuantity = selectedProduct.SelectedQuantity;
            }
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" }, { "selectedQty" } });
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult RelatedProducts(int productId)
        {
            var model = PrepareRelatedProductdetailsModel(productId);

            return PartialView(model.ProductDetail);
        }

        private CategoryModel PrepareCategoryProductModel(string seName, string filterIds, string minPrice, string maxPrice, string keyword)
        {

            var model = new CategoryModel(categoryService.GetProductsBySlug(seName, string.IsNullOrEmpty(filterIds) ? null : filterIds,
                string.IsNullOrEmpty(minPrice) ? (decimal?)null : Convert.ToDecimal(minPrice), string.IsNullOrEmpty(maxPrice) ? (decimal?)null : Convert.ToDecimal(maxPrice), keyword));
            if (filterIds != null)
            {
                model.SelectedSpecificationAttributeIds = (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                           select Convert.ToInt32(f)).ToArray();
            }
            return model;
        }

        private SearchModel PrepareCategoryProductModelBySearch(string seName, string filterIds, string minPrice, string maxPrice, string keyword)
        {

            var model = new SearchModel(categoryService.GetProductsBySearch(seName, string.IsNullOrEmpty(filterIds) ? null : filterIds,
                string.IsNullOrEmpty(minPrice) ? (decimal?)null : Convert.ToDecimal(minPrice), string.IsNullOrEmpty(maxPrice) ? (decimal?)null : Convert.ToDecimal(maxPrice), keyword));
            if (filterIds != null)
            {
                model.SelectedSpecificationAttributeIds = (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                           select Convert.ToInt32(f)).ToArray();
            }

            return model;
        }

        private ProductDetailModel PrepareProductdetailsModel(string seName)
        {
            var model = new ProductDetailModel(productService.GetProductsDetailsBySlug(seName));

            return model;
        }
        private RelatedProductsModel PrepareRelatedProductdetailsModel(int productId)
        {
            var model = new RelatedProductsModel(productService.GetRelatedProductsById(productId));

            return model;
        }
        private IList<CategorySimpleModel> PrepareCategorySimpleModels()
        {
            return (from c in categoryService.GetTopMenuCategories()
                    select new CategorySimpleModel(c)).ToList();
        }

        private string AddCartProductAttribute(string attributes, ProductVariantAttributeModel pva, string value)
        {
            string result = string.Empty;
            try
            {
                var xmlDoc = new XmlDocument();
                if (String.IsNullOrEmpty(attributes))
                {
                    var element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(attributes);
                }
                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement pvaElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductVariantAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        int id = 0;
                        if (int.TryParse(str1, out id))
                        {
                            if (id == pva.Id)
                            {
                                pvaElement = (XmlElement)node1;
                                break;
                            }
                        }
                    }
                }

                //create new one if not found
                if (pvaElement == null)
                {
                    pvaElement = xmlDoc.CreateElement("ProductVariantAttribute");
                    pvaElement.SetAttribute("ID", pva.Id.ToString());
                    rootElement.AppendChild(pvaElement);
                }

                var pvavElement = xmlDoc.CreateElement("ProductVariantAttributeValue");
                pvaElement.AppendChild(pvavElement);

                var pvavVElement = xmlDoc.CreateElement("Value");
                pvavVElement.InnerText = value;
                pvavElement.AppendChild(pvavVElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception exc)
            {
                Debug.Write(exc.ToString());
            }
            return result;
        }

    }
}
