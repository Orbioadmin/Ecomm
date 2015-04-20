using System;
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
using Orbio.Web.UI.Filters;
using Orbio.Core.Domain.Catalog;
using System.Data;

namespace Orbio.Web.UI.Controllers
{
    /// <summary>
    /// catalog controller
    /// </summary>
    public class CatalogController : Controller
    {
        private readonly ICategoryService categoryService;

        private readonly IProductService productService;

        private readonly IShoppingCartService shoppingCartService;
        // private readonly IWorkContext workContext;
        // private readonly IStoreContext storeContext;

        private readonly ICacheManager cacheManager;
        // private readonly CatalogSettings catalogSettings;
        private readonly IWebHelper webHelper;

        /// <summary>
        /// instantiates catalog controller
        /// </summary>
        public CatalogController(ICategoryService categoryService, IProductService productService, IShoppingCartService shoppingCartService,
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
            this.shoppingCartService = shoppingCartService;
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

        public ActionResult Search(string seName, string spec, string keyWord)
        {
            var model = new SearchModel();
            int pageNumber = 1;
            int pageSize = (ConfigurationManager.AppSettings["CatelogProductsPageSize"].ToString() != "") ? Convert.ToInt32(ConfigurationManager.AppSettings["CatelogProductsPageSize"]) : 10;
            if (keyWord.Length >= 3)
            {

                model = PrepareCategoryProductModelBySearch(seName, spec, keyWord, pageNumber, pageSize);
                ViewBag.searchkeyword = " ITEMS FOUND BY KEYWORD ''" + keyWord + "''";
            }
            else
            {
                model = PrepareCategoryProductModelBySearch(seName, spec, "0", pageNumber, pageSize);
                ViewBag.searchkeyword = " ITEMS FOUND";
                ViewBag.Error = "Search term minimum length is 3 characters";
            }
            ViewBag.slug = seName; ViewBag.spec = spec; ViewBag.keyWord = keyWord;
            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" } });
            return View(model);
        }

        public ActionResult Category(string seName, string spec, string keyWord)
        {
            int pageNumber = 1;
            int pageSize = (ConfigurationManager.AppSettings["CatelogProductsPageSize"].ToString() != "") ? Convert.ToInt32(ConfigurationManager.AppSettings["CatelogProductsPageSize"]) : 10;
            var model = PrepareCategoryProductModel(seName, spec, keyWord, pageNumber, pageSize);
            if (!string.IsNullOrEmpty(keyWord))
            { ViewBag.searchkeyword = " ITEMS FOUND BY KEYWORD ''" + keyWord + "''"; }
            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" } });
            ViewBag.MetaDescription = model.MetaDescription;
            ViewBag.MetaKeywords = model.MetaKeywords;
            ViewBag.spec = spec; ViewBag.keyWord = keyWord;
            return View(model);
        }
        [HttpPost]
        public ActionResult CategoryPaging(string seName, string spec, string keyWord, int? pageNumber)
        {
            int pageSize = (ConfigurationManager.AppSettings["CatelogProductsPageSize"].ToString() != "") ? Convert.ToInt32(ConfigurationManager.AppSettings["CatelogProductsPageSize"]) : 10;
            if (seName != "Search")
            {
                var model = PrepareCategoryProductModel(seName, spec, keyWord, pageNumber, pageSize);
                return PartialView("_CategoryProducts", model);
            }
            else
            {
                var model = PrepareCategoryProductModelBySearch(seName, spec, keyWord, pageNumber, pageSize);
                return PartialView("_CategoryProductBySearch", model);
            }
        }

        [ChildActionOnly]
        public ActionResult ProductFilter(int categoryId, int[] selectedSpecs, string selectedPriceRange, string keyWord)
        {
            var model = PrepareSpecificationFilterModel(categoryId, selectedSpecs, selectedPriceRange, keyWord);

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ProductFilterBySearch(string categoryId, int[] selectedSpecs, string selectedPriceRange, string keyWord)
        {
            var model = PrepareSpecificationFilterModelBySearch(categoryId, selectedSpecs, selectedPriceRange, keyWord);

            return PartialView("ProductFilter", model);
        }
        private List<SpecificationAttribute> PrepareSpecificationFilterModel(int categoryId, int[] selectedSpecs, string selectedPriceRange, string keyWord)
        {
            var specFilterModels = categoryService.GetSpecificationFiltersByCategoryId(categoryId, keyWord);
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
            var minProductPrice = (from price in specFilterModels
                                   select price.MinPrice).FirstOrDefault();
            var maxProductPrice = (from price in specFilterModels
                                   select price.MaxPrice).FirstOrDefault();
            int minPrice = Convert.ToInt32(minProductPrice);
            int maxPrice = Convert.ToInt32(maxProductPrice);
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
                    SelectedAttributeOptions = selectedPriceRange,
                    SpecificationAttributeOptions = new List<SpecificationAttributeOption> { 
                {new SpecificationAttributeOption{Name=minPrice.ToString(), ElementName="productPriceFilterMinValue" ,  FilterUrl = currentUrl}},
                {new SpecificationAttributeOption{Name=maxPrice.ToString(), ElementName="productPriceFilterMaxValue" , FilterUrl = currentUrl}}
                }
                });
            }
            return model;
        }

        private List<SpecificationAttribute> PrepareSpecificationFilterModelBySearch(string categoryId, int[] selectedSpecs, string selectedPriceRange, string keyWord)
        {
            var specFilterModels = categoryService.GetSpecificationFiltersByCategory(string.IsNullOrEmpty(categoryId) ? "0" : categoryId, keyWord);
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
            var minProductPrice = (from price in specFilterModels
                                   select price.MinPrice).FirstOrDefault();
            var maxProductPrice = (from price in specFilterModels
                                   select price.MaxPrice).FirstOrDefault();
            int minPrice = Convert.ToInt32(minProductPrice);
            int maxPrice = Convert.ToInt32(maxProductPrice);
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
                    SelectedAttributeOptions = selectedPriceRange,
                    SpecificationAttributeOptions = new List<SpecificationAttributeOption> { 
                {new SpecificationAttributeOption{Name=minPrice.ToString(), ElementName="productPriceFilterMinValue" ,  FilterUrl = currentUrl}},
                {new SpecificationAttributeOption{Name=maxPrice.ToString(), ElementName="productPriceFilterMaxValue" , FilterUrl = currentUrl}}
                }
                });
            }
            return model;
        }

        [HttpPost]
        public ActionResult Product(ProductDetailModel product, FormCollection formCollection)
        {
            var cartType = ShoppingCartType.None;
             
            if (formCollection["Buy"] != null)
            {
                cartType = ShoppingCartType.ShoppingCart;
            }
            else if (formCollection["WishList"] != null)
            {
                cartType = ShoppingCartType.Wishlist;
            }
            else
            {               
                var pvavElement = (from k in formCollection.AllKeys
                                   where k.StartsWith("pvav_")
                                   select k).FirstOrDefault();
                if (!string.IsNullOrEmpty(pvavElement))
                {
                    TempData.AddTempData("pvavElement", pvavElement);
                }
            }
         
            TempData.AddTempData("product", product);

            TempData.AddTempData("cartType", cartType);           
 
            
            return RedirectToRoute("Category", new { p = "pt", seName = product.SeName });
        }

        public ActionResult Product(string seName)
        {
            var model = PrepareProductdetailsModel(seName);
            ProductDetailModel selectedProduct = null;
            ShoppingCartType selectedCartType = ShoppingCartType.None;
            ViewBag.Errors = string.Empty;
            if (TempData.ContainsKey("product"))
            {
                var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                var curCustomer = workContext.CurrentCustomer;
                selectedProduct = (ProductDetailModel)TempData["product"];
                selectedCartType = (ShoppingCartType)TempData["cartType"];
                TempData.Remove("product");
                TempData.Remove("cartType");
                int selectedquantity = Convert.ToInt32(selectedProduct.SelectedQuantity);
               

                model.ProductVariantAttributes.ValidateProductVariantAttributes(selectedProduct.ProductVariantAttributes, TempData["pvavElement"]==null?null:TempData["pvavElement"].ToString());
                //if (selectedCartType == ShoppingCartType.ShoppingCart || selectedCartType == ShoppingCartType.Wishlist)
                //{
                //    /*selected product attribute varient value*/
                //    string selectedAttributes = string.Empty;
                //    int count = 0;
                //    int selectedAttributeId = 1;
                //    foreach (var attribute in selectedProduct.ProductVariantAttributes)
                //    {
                //        switch (attribute.AttributeControlType)
                //        {
                //            case Orbio.Core.Domain.Catalog.AttributeControlType.TableBlock:
                //                {
                //                    if (TempData.ContainsKey("pvaid"))
                //                    {
                //                        selectedAttributeId = int.Parse(TempData["pvaid"].ToString());
                //                        selectedAttributes = AddCartProductAttribute(selectedAttributes,
                //                                   attribute, selectedAttributeId.ToString());
                //                        TempData.Remove("pvaid");
                //                    }
                //                    else if (TempData.ContainsKey("pvaidDefault"))
                //                    {
                //                        selectedAttributeId = int.Parse(TempData["pvaidDefault"].ToString());
                //                        selectedAttributes = AddCartProductAttribute(selectedAttributes,
                //                                   attribute, selectedAttributeId.ToString());
                //                        TempData.Remove("pvaidDefault");
                //                    }
                //                    else
                //                    {
                //                        foreach (var value in attribute.Values)
                //                        {
                //                            int maxdisplayorder = 0;
                //                            int i = 0;
                //                            string name = "";
                //                            if (value.DisplayOrder > maxdisplayorder)
                //                            {
                //                                name = value.Name;
                //                                maxdisplayorder = value.DisplayOrder;
                //                            }
                //                            if (value.Id != 0)
                //                            {
                //                                if (value.Name != name && i == 0)
                //                                {
                //                                    selectedAttributes = string.Empty;
                //                                    selectedAttributeId = int.Parse(value.Id.ToString());
                //                                    selectedAttributes = AddCartProductAttribute(selectedAttributes,
                //                                               attribute, selectedAttributeId.ToString());
                //                                    i++;
                //                                }
                //                                if (TempData.ContainsKey("pvaid"))
                //                                {
                //                                    TempData.Remove("pvaid");
                //                                }
                //                                //else
                //                                TempData.Add("pvaid", value.Id);
                //                            }
                //                            if (value.Name == name && i == 0)
                //                            {
                //                                selectedAttributes = string.Empty;
                //                                selectedAttributeId = int.Parse(value.Id.ToString());
                //                                selectedAttributes = AddCartProductAttribute(selectedAttributes,
                //                                           attribute, selectedAttributeId.ToString());
                //                            }
                //                        }
                //                    }
                //                    break;
                //                }
                //        }
                //        count++;
                //    }
                //}
              
                if (selectedCartType == ShoppingCartType.ShoppingCart)
                {
                   
                    var errorString = ValidateProduct(model, selectedProduct, selectedquantity);

                    if (string.IsNullOrEmpty(errorString))
                    {
                        var selectedAttributes = GetSelectedAttributeXml(model);
                        shoppingCartService.AddCartItem("add", selectedCartType, 0, curCustomer.Id, selectedProduct.Id, selectedAttributes, Convert.ToInt32(selectedProduct.SelectedQuantity));

                        ViewBag.Sucess = "Item added to the Cart";
                        bool flag = (ConfigurationManager.AppSettings["DisplayCartAfterAddingProduct"].ToString() != "") ? Convert.ToBoolean(ConfigurationManager.AppSettings["DisplayCartAfterAddingProduct"]) : false;
                        if (flag)
                        {
                            return RedirectToRoute("ShoppingCart");
                        }
                    }
                    else
                    {
                        ViewBag.Errors = errorString;
                    }
                }
                else if (selectedCartType == ShoppingCartType.Wishlist)
                {
                    var selectedAttributes = GetSelectedAttributeXml(model);
                    string result = shoppingCartService.AddWishlistItem("addWishList", selectedCartType, 0, curCustomer.Id, selectedProduct.Id, selectedAttributes, Convert.ToInt32(selectedProduct.SelectedQuantity));

                    ViewBag.Sucess = "Item added to the WishList";
                    bool flag = (ConfigurationManager.AppSettings["DisplayWishListAfterAddingProduct"].ToString() != "") ? Convert.ToBoolean(ConfigurationManager.AppSettings["DisplayWishListAfterAddingProduct"]) : false;
                    if (result == "updated")
                    {
                        return RedirectToAction("MyAccount", "Customer", new { wish = "#wish" });
                    }
                    if (flag)
                    {
                        return RedirectToAction("MyAccount", "Customer", new { wish = "#wish" });
                    }
                }

                //else
                //{
                    
                //    //add/substract price
                //    //int attr_count = 0;
                //    //int value_count = 0;
                //    //foreach (var prod_attribute in model.ProductVariantAttributes)
                //    //{
                //    //    foreach (var values in prod_attribute.Values)
                //    //    {
                //    //        if (selectedAttributeId == values.Id)
                //    //        {
                //    //            double subtotal = double.Parse(model.ProductPrice.Price) + double.Parse(values.PriceAdjustment);
                //    //            model.ProductPrice.Price = subtotal.ToString();
                //    //            string pvavliid = string.Format("liProductVariantAttributes_{0}__Values_{1}__Id", attr_count, value_count);
                //    //            ViewData["productvariantid"] = pvavliid;
                //    //        }
                //    //        value_count++;
                //    //    }
                //    //    attr_count++;
                //    //}
                //}
            }

            var queryString = new NameValueCollection(ControllerContext.HttpContext.Request.QueryString);
            model.SeName = seName;
            if (selectedProduct != null)
            {
                model.SelectedQuantity = selectedProduct.SelectedQuantity;
            }
            else
            {
                model.ProductVariantAttributes.SetIsPreSelected();
                
                //var attributes = (from prod_attribute in model.ProductVariantAttributes
                //                  select prod_attribute).ToList().FirstOrDefault();
                //int maxdisplayorder = 0;
                //string name = "";
                //bool ispreselected;
                //int attr_count = 0;
                //int value_count = 0;
                //foreach (var attributes in model.ProductVariantAttributes)
                //{
                //    foreach (var value in attributes.Values)
                //    {
                //        if (value.DisplayOrder > maxdisplayorder)
                //        {
                //            maxdisplayorder = value.DisplayOrder;
                //            name = value.Name;
                //            ispreselected = value.IsPreSelected;
                //            if (ispreselected)
                //            {
                //                if (TempData.ContainsKey("pvaidDefault"))
                //                {
                //                    TempData.Remove("pvaidDefault");
                //                }
                //                TempData.Add("pvaidDefault", value.Id);
                //                ViewBag.name = name;
                //                model.ProductPrice.Price = (Convert.ToDouble(value.PriceAdjustment) + Convert.ToDouble(model.ProductPrice.Price)).ToString();
                //                string pvavliid = string.Format("liProductVariantAttributes_{0}__Values_{1}__Id", attr_count, value_count);
                //                ViewData["productvariantid"] = pvavliid;
                //            }
                //        }
                //        value_count++;
                //    }
                //    attr_count++;
                //}
            }
            UpdateProductPrice(model);
            webHelper.RemoveQueryFromPath(ControllerContext.HttpContext, new List<string> { { "spec" }, { "selectedQty" } });
            return View(model);
        }

        private string GetSelectedAttributeXml(ProductDetailModel model)
        {
            var selectedAttributes = string.Empty;
            var selectedPvas = (from pva in model.ProductVariantAttributes
                                from pvav in pva.Values
                                where pvav.IsPreSelected == true
                                select new { Pva = pva, PvavId = pvav.Id }).ToList();
            if (selectedPvas.Count > 0)
            {
                foreach (var spva in selectedPvas)
                {
                    selectedAttributes = AddCartProductAttribute(selectedAttributes, spva.Pva, spva.PvavId.ToString());
                }
            }
            return selectedAttributes;
        }

        private static void UpdateProductPrice(ProductDetailModel model)
        {
            var pvValues = (from pva in model.ProductVariantAttributes
                            from pvav in pva.Values
                            where pvav.IsPreSelected == true
                            select pvav).ToList();
            if (pvValues.Count > 0)
            {
                foreach (var pvav in pvValues)
                {
                    model.ProductPrice.Price = (Convert.ToDouble(pvav.PriceAdjustment) + Convert.ToDouble(model.ProductPrice.Price)).ToString();
                }
                //string pvavliid = string.Format("liProductVariantAttributes_{0}__Values_{1}__Id", attr_count, value_count);
                //ViewData["productvariantid"] = pvavliid;
            }
        }

        private static string ValidateProduct(ProductDetailModel model, ProductDetailModel selectedProduct, int selectedquantity)
        {
            var errorString = string.Empty;
            if (selectedProduct.ProductVariantAttributes.Count > 0)
            {

                errorString = selectedProduct.ProductVariantAttributes.GetProductVariantErrors(model.ProductVariantAttributes);
            }

            if (selectedquantity < model.OrderMinimumQuantity || selectedquantity > model.OrderMaximumQuantity)
            {
                errorString += "Select a quantity between " + model.OrderMinimumQuantity + " and " + model.OrderMaximumQuantity;
            }
            return errorString;
        }

        [ChildActionOnly]
        public ActionResult RelatedProducts(int productId)
        {
            var model = PrepareRelatedProductdetailsModel(productId);

            return PartialView(model.ProductDetail);
        }

        [ChildActionOnly]
        public ActionResult AssociatedProducts(int productId)
        {
            var model = PrepareAssociatedProductdetailsModel(productId);

            return PartialView(model.ProductDetail);
        }

        private CategoryModel PrepareCategoryProductModel(string seName, string filterIds, string keyWord, int? pageNumber, int? pageSize)
        {
            var specificationAttributeIds = string.IsNullOrWhiteSpace(filterIds) ? new List<string>() : (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                                                         where f.Split('~').Length <= 1
                                                                                                         select f).ToList();

            var model = new CategoryModel(categoryService.GetProductsBySlug(seName, specificationAttributeIds.Count == 0 ? null : specificationAttributeIds.Aggregate((f1, f2) => f1 + "," + f2),
                (decimal?)null, (decimal?)null, keyWord, pageNumber, pageSize));
            var minPrice = 0M;
            var maxPrice = 0M;
            var priceModel = (from pr in
                                  (
                                      from p in model.Products
                                      select p.ProductPrice)
                              select Convert.ToDecimal(pr.Price)).ToList();
            if (priceModel.Count > 0)
            {
                minPrice = priceModel.Min();
                maxPrice = priceModel.Max();
            }

            model.MinPrice = minPrice;
            model.MaxPrice = maxPrice;

            if (filterIds != null)
            {

                model.SelectedSpecificationAttributeIds = (from f in specificationAttributeIds
                                                           select Convert.ToInt32(f)).ToArray();
                var selectedPriceRange = (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          where f.Split('~').Length > 1
                                          select f).FirstOrDefault();

                model.SelectedPriceRange = selectedPriceRange;

                if (!string.IsNullOrWhiteSpace(model.SelectedPriceRange))
                {

                    var priceRanges = model.SelectedPriceRange.Split('~');

                    if (priceRanges.Length > 1)
                    {
                        var selectedMinPrice = 0M;
                        if (decimal.TryParse(priceRanges[0], out selectedMinPrice))
                        {
                            if (selectedMinPrice >= minPrice)
                            {
                                minPrice = selectedMinPrice;
                            }
                        }

                        var selectedMaxPrice = 0M;
                        if (decimal.TryParse(priceRanges[1], out selectedMaxPrice))
                        {
                            if (selectedMaxPrice <= maxPrice)
                            {
                                maxPrice = selectedMaxPrice;
                            }
                        }

                        var filteredProducts = (from p in model.Products
                                                where Convert.ToDecimal(p.ProductPrice.Price) >= minPrice &&
                                             Convert.ToDecimal(p.ProductPrice.Price) <= maxPrice
                                                select p).ToList();
                        model.Products = filteredProducts;
                    }
                }

            }
            return model;
        }

        private SearchModel PrepareCategoryProductModelBySearch(string seName, string filterIds, string keyWord, int? pageNumber, int? pageSize)
        {
            var specificationAttributeIds = string.IsNullOrWhiteSpace(filterIds) ? new List<string>() : (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                                                         where f.Split('~').Length <= 1
                                                                                                         select f).ToList();

            var model = new SearchModel(categoryService.GetProductsBySearch(seName, specificationAttributeIds.Count == 0 ? null : specificationAttributeIds.Aggregate((f1, f2) => f1 + "," + f2),
                (decimal?)null, (decimal?)null, keyWord, pageNumber, pageSize));
            var minPrice = 0M;
            var maxPrice = 0M;
            var priceModel = (from pr in
                                  (
                                      from p in model.Products
                                      select p.ProductPrice)
                              select Convert.ToDecimal(pr.Price)).ToList();
            if (priceModel.Count > 0)
            {
                minPrice = priceModel.Min();
                maxPrice = priceModel.Max();
            }

            model.MinPrice = minPrice;
            model.MaxPrice = maxPrice;
            if (filterIds != null)
            {

                model.SelectedSpecificationAttributeIds = (from f in specificationAttributeIds
                                                           select Convert.ToInt32(f)).ToArray();
                var selectedPriceRange = (from f in filterIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          where f.Split('~').Length > 1
                                          select f).FirstOrDefault();

                model.SelectedPriceRange = selectedPriceRange;




                if (!string.IsNullOrWhiteSpace(model.SelectedPriceRange))
                {

                    var priceRanges = model.SelectedPriceRange.Split('~');

                    if (priceRanges.Length > 1)
                    {
                        var selectedMinPrice = 0M;
                        if (decimal.TryParse(priceRanges[0], out selectedMinPrice))
                        {
                            if (selectedMinPrice >= minPrice)
                            {
                                minPrice = selectedMinPrice;
                            }
                        }

                        var selectedMaxPrice = 0M;
                        if (decimal.TryParse(priceRanges[1], out selectedMaxPrice))
                        {
                            if (selectedMaxPrice <= maxPrice)
                            {
                                maxPrice = selectedMaxPrice;
                            }
                        }

                        var filteredProducts = (from p in model.Products
                                                where Convert.ToDecimal(p.ProductPrice.Price) >= minPrice &&
                                             Convert.ToDecimal(p.ProductPrice.Price) <= maxPrice
                                                select p).ToList();
                        model.Products = filteredProducts;
                    }
                }
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
        private AssociatedProductsModel PrepareAssociatedProductdetailsModel(int productId)
        {
            var model = new AssociatedProductsModel(productService.GetAssociatedProductsById(productId));

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

        [LoginRequired]
        public ActionResult Review(ProductDetailModel product, string name, string seName)
        {
            var model = new ReviewModel();
            model.Rating = 5;
            model.Name = name;
            model.SeName = seName;
            return View(model);
        }

        [HttpPost]
        public ActionResult Review(ReviewModel model, string seName, string name)
        {
            if (ModelState.IsValid)
            {
                var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
                if (workContext.CurrentCustomer.IsRegistered)
                {
                    workContext.CurrentCustomer.IsApproved = false;
                    if (model.Rating > 0 || model.Rating < 6)
                    {
                        var productdetails = PrepareProductdetailsModel(seName);
                        var productresult = productService.InsertReviews(workContext.CurrentCustomer.Id, productdetails.Id
                        , workContext.CurrentCustomer.IsApproved, model.ReviewTitle, model.ReviewText, model.Rating, model.CustomerName);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please rate the product");
                        return View(model);
                    }
                }
                else
                {

                    ModelState.AddModelError("", "Only registered customers can write reviews");
                    return View(model);
                }
                return RedirectToRoute("Category", new { p = "pt", seName = seName });
            }
            else
            {
                return View(model);
            }
        }


        //[ChildActionOnly]
        public ActionResult ProductReview(ReviewModel model, int id, string seName, int pageNumber)
        {
            List<ReviewModel> list = new List<ReviewModel>();
            list = GetCustomerReview(id, pageNumber);
            return PartialView(list);
        }

        [ChildActionOnly]
        public ActionResult ProductRating(ReviewModel model, int id)
        {
            var ratings = productService.GetCustomerReviews(id, "Rating", 0, 0);
            var rating = GetRatings(ratings);
            return PartialView(rating);
        }

        [ChildActionOnly]
        public ActionResult ProductStar(ReviewModel model, int id)
        {
            var ratings = productService.GetCustomerReviews(id, "Rating", 0, 0);
            var rating = GetRatings(ratings);
            return PartialView(rating);
        }
        private List<ReviewModel> GetCustomerReview(int id, int pageNumber)
        {
            int pageSize = (ConfigurationManager.AppSettings["CatelogProductsReviewPageSize"].ToString() != "") ? Convert.ToInt32(ConfigurationManager.AppSettings["CatelogProductsReviewPageSize"]) : 2;
            var customerReviews = productService.GetCustomerReviews(id, "Review", pageNumber, pageSize);

            var model = (from cr in customerReviews
                         select new ReviewModel
                         {
                             ReviewTitle = cr.ReviewTitle,
                             ReviewText = cr.ReviewText,
                             Rating = cr.Rating,
                             CustomerName = cr.CustomerName
                         }).ToList();
            return model;
        }


        private List<ReviewModel> GetRatings(List<ProductReview> ratings)
        {
            var rating = (from cr in ratings
                          select new ReviewModel
                          {
                              OneStarRating = cr.OneStarRating,
                              TwoStarRating = cr.TwoStarRating,
                              ThreeStarRating = cr.ThreeStarRating,
                              FourStarRating = cr.FourStarRating,
                              FiveStarRating = cr.FiveStarRating,
                              StarCount = cr.StarCount,
                              AvgRating = ((Convert.ToDouble(cr.OneStarRating * 1) + Convert.ToDouble(cr.TwoStarRating * 2) + Convert.ToDouble(cr.ThreeStarRating * 3) + Convert.ToDouble(cr.FourStarRating * 4) + Convert.ToDouble(cr.FiveStarRating * 5)) / Convert.ToDouble(cr.StarCount))
                          }).ToList();

            return rating;
        }      


    }

    public static class TempDataExtension
    {
        public static void AddTempData(this TempDataDictionary tempData, string key, object value)
        {
            if (tempData.ContainsKey(key))
            {
                tempData.Remove(key);
            }
            tempData.Add(key, value);
        }
    }
}
