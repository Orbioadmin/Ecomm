using Orbio.Core.Data;
using Orbio.Services.Admin.Catalog;
using Orbio.Services.Admin.Orders;
using Orbio.Services.Admin.Products;
using Orbio.Services.Admin.Tax;
using Orbio.Web.UI.Areas.Admin.Models.Catalog;
using Orbio.Web.UI.Areas.Admin.Models.Product;
using Orbio.Web.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        #region Fields

        public readonly ICategoryServices _categoryService;
        public readonly IManufacturerService _manufatureService;
        public readonly IProductService _productService;
        public readonly IShippingService _shippingService;
        public readonly ITaxCategoryService _taxCategoryService;
        #endregion

        #region Constructors
        public ProductController(ICategoryServices categoryService, IManufacturerService manufatureService, IProductService productService, IShippingService shippingService, ITaxCategoryService taxCategoryService)
        {
            this._categoryService = categoryService;
            this._manufatureService = manufatureService;
            this._productService = productService;
            this._shippingService = shippingService;
            this._taxCategoryService = taxCategoryService;
        }
        #endregion

        #region Methods

        #region Product list / create / edit / delete

        //List products
        public ActionResult List()
        {
            var model = new ProductListModel();
            //catgory List
            var categoryModel = new CategoryDetailModel(_categoryService.GetCategoryDetails());
            model.AvailableCategories.Add(new SelectListItem() { Text = "All Category", Value = "0" });
            foreach (var c in categoryModel.Categories.CategoryList.GetFormattedBreadCrumb())
                model.AvailableCategories.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });

            //manufacturers
            var manufactureModel = new ManufacturerDetailModel(_manufatureService.GetAllManufacturers());
            model.AvailableManufacturers.Add(new SelectListItem() { Text = "All Manufature", Value = "0" });
            foreach (var c in manufactureModel.ManufacturerList)
                model.AvailableManufacturers.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString() });

            return View(model);
        }
        [ChildActionOnly]
        public ActionResult ProductList(ProductListModel model)
        {
            var resultModel = GetProductList(model);

            return View(resultModel);
        }

        public List<Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel> GetProductList(ProductListModel model)
        {
            return (from p in _productService.GetAllProductsSeachOrDefault(model.SearchProductNameOrSku,model.CategoryId,model.ManufactureId)
                    select new Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel(p)).ToList();
        }

        public ActionResult Create()
        {
            var model = new Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel();
            PrepareProductModel(model, null);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var product = model.ToEntity();
            product.CreatedOnUtc = DateTime.UtcNow;
            product.UpdatedOnUtc = DateTime.UtcNow;
            _productService.AddNewProduct(product);
            return RedirectToAction("List");
            
        }

        //edit product
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null || product.Deleted)
                //No product found with the specified id
                return RedirectToAction("List");
            var model = product.ToModel();
            PrepareProductModel(model, product);
            return View();
        }

        //delete products
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSelected(int[] deleteProductId)
        {
            if (deleteProductId == null)
            {
                ModelState.AddModelError("", "None of the reconds has been selected for delete action !");
                return RedirectToAction("List");
            }

            _productService.DeleteSelectedProducts(deleteProductId);

            return RedirectToAction("List");

        }

        protected void PrepareProductModel(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model, Product product)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (product != null)
            {
                model.Name = product.Name;
                model.CreatedOnUtc = product.CreatedOnUtc;
                model.UpdatedOnUtc = product.UpdatedOnUtc;
            }

            //delivery dates
            model.AvailableDeliveryDates.Add(new SelectListItem()
            {
                Text = "None",
                Value = "0"
            });
            var deliveryDates = _shippingService.GetAllDeliveryDates();

            foreach (var deliveryDate in deliveryDates)
            {
                model.AvailableDeliveryDates.Add(new SelectListItem()
                {
                    Text = deliveryDate.Name,
                    Value = deliveryDate.Id.ToString()
                });
            }

            //warehouses
            model.AvailableWarehouses.Add(new SelectListItem()
            {
                Text = "None",
                Value = "0"
            });
            var warehouses = _shippingService.GetAllWarehouses();
            foreach (var warehouse in warehouses)
            {
                model.AvailableWarehouses.Add(new SelectListItem()
                {
                    Text = warehouse.Name,
                    Value = warehouse.Id.ToString()
                });
            }

            //product tags
            //if (product != null)
            //{
            //    var result = new StringBuilder();
            //    for (int i = 0; i < product.ProductTags.Count; i++)
            //    {
            //        var pt = product.ProductTags.ToList()[i];
            //        result.Append(pt.Name);
            //        if (i != product.ProductTags.Count - 1)
            //            result.Append(", ");
            //    }
            //    model.ProductTags = result.ToString();
            //}

            //tax categories
            var taxCategories = _taxCategoryService.GetAllTaxCategories();
            model.AvailableTaxCategories.Add(new SelectListItem() { Text = "---", Value = "0" });
            foreach (var tc in taxCategories)
                model.AvailableTaxCategories.Add(new SelectListItem() { Text = tc.Name, Value = tc.Id.ToString() });


            //default values
                model.StockQuantity = 10000;
                model.OrderMinimumQuantity = 1;
                model.OrderMaximumQuantity = 10000;

                model.IsShipEnabled = false;
                model.AllowCustomerReviews = true;
                model.Published = true;
        }

        #endregion

        #endregion
    }
}