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
using PagedList;

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
            var result = categoryServices.GetTopMenuCategories();
            var model = new CategoryDetailModel()
            {
                ParentCategories = (from c in result
                    select new CategoryModel(c)).ToList(),
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
            model.Categories.CategoryList = model.Categories.CategoryList.GetFormattedBreadCrumb();
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
            model.Categories.CategoryList = model.Categories.CategoryList.GetFormattedBreadCrumb();
            return View("AddorEditCategory", model);
        }

        public ActionResult CategoryProduct(int? Id,int? page)
        {
            var result = categoryServices.GetCategoryProducts(Id.GetValueOrDefault());
            var model = (from pcm in result
                         select new ProductModel(pcm)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            return PartialView(model.ToPagedList(pageNumber,pageSize));
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
        public ActionResult DeleteCategoryProduct(int? Id, int? cId)
        {
            categoryServices.DeleteCategoryProduct(Id.GetValueOrDefault(), cId.GetValueOrDefault());
            return RedirectToAction("EditCategory", new { Id = Id.GetValueOrDefault() });
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

        public ActionResult ManufacturerProduct(int? Id, int? page)
        {
            var result = manufacturerService.GetManufacturerProducts(Id.GetValueOrDefault());
            var model = (from pcm in result
                         select new ProductModel(pcm)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            return PartialView(model.ToPagedList(pageNumber, pageSize));
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
            if (!string.IsNullOrEmpty(model.Search))
            {
                var result = new ManufacturerDetailModel(manufacturerService.SearchManufacturerByName(model.Search));
                result.Search = model.Search;
                return View("ManageManufacturer", result);
            }
            else
            {
                return RedirectToAction("ManageManufacturer");
            }
        }

        /// <summary>
        /// updating a manufacturer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult UpdateManufacturer(ManufacturerDetailModel model)
        {
                var manufacturerDetail = GetManufacturer(model);
                var result = manufacturerService.AddOrUpdateManufacturer(manufacturerDetail.Manufacturer);
                if (model.Manufacturer.Id != 0)
                {
                    return RedirectToAction("ManageManufacturer");
                }
                else
                {
                    return RedirectToAction("EditManufacturers", new { Id = result });
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
                                          PictureId = model.Categories.PictureId,
                                          MetaKeywords = model.Categories.MetaKeyWords,
                                          MetaDescription = model.Categories.MetaDescription,
                                          MetaTitle = model.Categories.MetaTitle,
                                          Slug = model.Categories.SearchEngine,
                                          SubjectToACL = model.Categories.SubjectToACL,
                                          ParentCategoryId=model.Categories.ParentCategory,
                                          CategoryTemplateId = model.Categories.CategoryTemplate,
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
                SearchEngine=model.Manufacturer.SearchEngine,
            };
            return manufacturerModel;
        }
    }
}