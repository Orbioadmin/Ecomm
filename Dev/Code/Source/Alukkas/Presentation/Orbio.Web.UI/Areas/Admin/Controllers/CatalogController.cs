using Nop.Core.Caching;
using Orbio.Core.Domain.Admin.Catalog;
using Orbio.Services.Admin.Catalog;
using Orbio.Services.Catalog;
using Orbio.Web.UI.Areas.Admin.Models.Catalog;
using Orbio.Web.UI.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICacheManager cacheManager;
        private readonly ICategoryService categoryService;
        private readonly ICategoryServices categoryServices;
        private readonly IManufacturerService manufacturerService;
        
        public CatalogController(ICacheManager cacheManager, ICategoryService categoryService, ICategoryServices categoryServices,
            IManufacturerService manufacturerService)
        {
            this.cacheManager = cacheManager;
            this.categoryService = categoryService;
            this.categoryServices = categoryServices;
            this.manufacturerService = manufacturerService;
        }

        // GET: Admin/Catalog
        public ActionResult Index()
        {
            return View();
        }

        #region Category
        /// <summary>
        /// catalog->listing all categories in tree view
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageCategories()
        {
            var cachedModel = cacheManager.Get(string.Format(ModelCacheEventConsumer.CATEGORY_MENU_MODEL_KEY, 1, 4, 1),
                      () => PrepareCategoryModels());
            var model = new CategoryDetailModel()
            {
                ParentCategories = cachedModel,
            };
            return View(model);
        }

        /// <summary>
        /// Adding New Category
        /// </summary>
        /// <returns>CategoryListModel</returns>
        public ActionResult AddCategory()
        {
            var result = categoryServices.GetCategoryDetails();
            var model = new CategoryDetailModel(result);
            model.Categories.CategoryList = GetFormattedBreadCrumb(model.Categories.CategoryList, categoryService);
            return View("AddorEditCategory", model);
        }

        /// <summary>
        /// Editing a Category
        /// </summary>
        /// <param name="Id">Category Id</param>
        /// <returns>CategoryListModel</returns>
        public ActionResult EditCategory(int Id)
        {
            var result = categoryServices.GetCategoryDetailsById(Id);
            var model = new CategoryDetailModel(result);
            model.Categories.CategoryList = GetFormattedBreadCrumb(model.Categories.CategoryList, categoryService);
            return View("AddorEditCategory", model);
        }

        /// <summary>
        /// updating a category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateCategory(CategoryDetailModel model)
        {
            if (model.Categories.Name!=null)
            {
                var categoryDetail = GetCategory(model);
                var result = categoryServices.AddOrUpdateCategory(categoryDetail.Categories);
                return RedirectToAction("ManageCategories");
            }
            else 
            {
                return View("AddorEditCategory", model); 
            }
        }

        /// <summary>
        /// Deleting an existing category
        /// </summary>
        /// <param name="Id">Category Id</param>
        /// <returns></returns>
        public ActionResult DeleteCategory(int Id)
        {
            var result = categoryServices.DeleteCategory(Id);
            return RedirectToAction("ManageCategories");
        }

        /// <summary>
        /// for category tree view 
        /// </summary>
        /// <returns></returns>
        private IList<CategoryModel> PrepareCategoryModels()
        {
            var result= (from c in categoryService.GetTopMenuCategories()
                    select new CategoryModel(c)).ToList();
            return result;
        }


        /// <summary>
        /// Deleteing Category Products
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <param name="cId">Product Id</param>
        /// <returns></returns>
        public ActionResult DeleteCategoryProduct(int id, int cId)
        {
            categoryServices.DeleteCategoryProduct(id, cId);
            return RedirectToAction("EditCategory", new { id });
        }


    #endregion



        #region manufacturer
        /// <summary>
        /// catalog->listing all manufacturers
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageManufacturer()
        {
          var model=new ManufacturerDetailModel(manufacturerService.GetAllManufacturers());
          return View(model);
        }

        /// <summary>
        /// Editing an existing manufacturer
        /// </summary>
        /// <param name="Id">Manufacturer Id</param>
        /// <returns></returns>
        public ActionResult EditManufacturers(int Id)
        {
            var result = manufacturerService.GetManufacturerDetailsById(Id);
            var model = new ManufacturerDetailModel(result);
            return View("AddOrEditManufacturer", model);
        }

        /// <summary>
        /// Adding new manufacturer
        /// </summary>
        /// <returns></returns>
        public ActionResult AddManufacturer()
        {
            var result = manufacturerService.GetManufacturerDetails();
            var model = new ManufacturerDetailModel(result);
            return View("AddOrEditManufacturer", model);
        }

        /// <summary>
        /// Searching manufacturer by name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult SearchManufacturer(ManufacturerDetailModel model)
        {
            var result = new ManufacturerDetailModel(manufacturerService.SearchManufacturerByName(model.Search));
            result.Search = model.Search;
            return View("ManageManufacturer", result);
        }

        /// <summary>
        /// updating a manufacturer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult UpdateManufacturer(ManufacturerDetailModel model)
        {
            if (ModelState.IsValid)
            {
                var manufacturerDetail = GetManufacturer(model);
                var result = manufacturerService.AddOrUpdateManufacturer(manufacturerDetail.Manufacturer);
                return RedirectToAction("ManageManufacturer");
            }
            else
            {
                return View("ManageManufacturer", model);
            }
        }

        /// <summary>
        /// Deleting a manufacturer
        /// </summary>
        /// <param name="Id">Manufacturer Id</param>
        /// <returns></returns>
        public ActionResult DeleteManufacturer(int Id)
        {
            var result = manufacturerService.DeleteManufacturer(Id);
            return RedirectToAction("ManageManufacturer");
        }




   /// <summary>
  /// Deleting a manufacturer product
   /// </summary>
   /// <param name="id">Manufacture Id</param>
   /// <param name="pId">Product Id</param>
   /// <returns></returns>
        public ActionResult DeleteManufacturerProduct(int id,int pId)
        {
           manufacturerService.DeleteManufacturerProduct(id, pId);
           return RedirectToAction("EditManufacturers", new { id});
        }




        #endregion

        /// <summary>
        /// for adding new category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CategoryDetails GetCategory(CategoryDetailModel model)
        {
          var categoryModel = new CategoryDetails();
          categoryModel.Categories = new CategoryList()
                                      {
                                          Id = model.Categories.Id,
                                          Name = model.Categories.Name,
                                          Description = model.Categories.Description,
                                          ShowOnHomePage = model.Categories.ShowOnHomePage,
                                          Published = model.Categories.Published,
                                          DisplayOrder = model.Categories.DisplayOrder,
                                          Picture = model.Categories.Picture,
                                          MetaKeyWords = model.Categories.MetaKeyWords,
                                          MetaDescription = model.Categories.MetaDescription,
                                          MetaTitle = model.Categories.MetaTitle,
                                          SearchEngine = model.Categories.SearchEngine,
                                          SubjectToACL = model.Categories.SubjectToACL,
                                          ParentCategory=model.Categories.ParentCategory,
                                          CategoryTemplate = model.Categories.CategoryTemplate,
                                      };
            return categoryModel;
        }

        /// <summary>
        /// adding new manufacturer 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Manufacturers GetManufacturer(ManufacturerDetailModel model)
        {
            var manufacturerModel = new Manufacturers();
            manufacturerModel.Manufacturer = new ManufacturerDetails()
            {
                Id = model.Manufacturer.Id,
                Name = model.Manufacturer.Name,
                Description = model.Manufacturer.Description,
                Published = model.Manufacturer.Published,
                DisplayOrder = model.Manufacturer.DisplayOrder,
                Picture = model.Manufacturer.Picture,
                MetaKeyWords = model.Manufacturer.MetaKeyWords,
                MetaDescription = model.Manufacturer.MetaDescription,
                MetaTitle = model.Manufacturer.MetaTitle,
                SubjectToACL = model.Manufacturer.SubjectToACL,
                ManufacturerTemplate = model.Manufacturer.ManufacturerTemplate,
            };
            return manufacturerModel;
        }

        /// <summary>
        /// generating parent categories
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="categoryService"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public List<CategoryModel> GetFormattedBreadCrumb(List<CategoryModel> categoryList,
            ICategoryService categoryService, string separator = ">>")
        {
            if (categoryList == null)
                throw new ArgumentNullException("category");

            var resultList = new List<CategoryModel>();
            foreach (var item in categoryList)
            {
                int Id = item.Id;
                string result = string.Empty;
                var alreadyProcessedCategoryIds = new List<int>() { };
                var category = new CategoryModel();
                while (item != null && //not null
                   !alreadyProcessedCategoryIds.Contains(item.Id)) //prevent circular references
                {
                    if (String.IsNullOrEmpty(result))
                    {
                        result = item.Name;
                    }
                    else
                    {
                        result = string.Format("{0} {1} {2}", item.Name, separator, result);
                        goto ListCategories;
                    }

                    alreadyProcessedCategoryIds.Add(item.Id);

                    category = (from c in resultList
                                where c.Id == item.ParentCategory
                                select c).FirstOrDefault();
                    if (category != null)
                    {
                        item.Id = category.Id;
                        item.Name = category.Name;
                    }
                }
            ListCategories:
                resultList.Add(new CategoryModel()
                {
                    Id = Id,
                    Name = result,
                });
            }
            return resultList;
        }
    }
}