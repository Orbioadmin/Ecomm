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
using System.IO;
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
        [AdminAuthorizeAttribute]
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

        public List<Orbio.Web.UI.Models.Catalog.ProductOverViewModel> GetProductList(ProductListModel model)
        {
            return (from p in _productService.GetAllProductsSeachOrDefault(model.SearchProductNameOrSku,model.CategoryId,model.ManufactureId)
                    select new Orbio.Web.UI.Models.Catalog.ProductOverViewModel(p)).ToList();
        }

        [AdminAuthorizeAttribute]
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
            var productDetails = new Orbio.Core.Domain.Admin.Product.ProductDetail
            {
                product = product.ToDomainModel(),
                seName = model.SeName,
                productTags = model.SelectedProductTags,
                catgoryIds = model.SelectedCategories,
                manufactureIds = model.SelectedManufature
            };
            _productService.InsertNewProduct(productDetails);
            return RedirectToAction("List");
            
        }

        //edit product
        [AdminAuthorizeAttribute]
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null || product.Deleted)
                //No product found with the specified id
                return RedirectToAction("List");
            var model = product.ToModel();
            PrepareProductModel(model, product);
            return View(model);
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

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult uploadImg()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    default:
                        break;
                }
            }

            string path = System.IO.Path.Combine(
                                   Server.MapPath("~/Areas/Admin/images/"), fileName);
            // file is uploaded
            HttpPostedFileBase file = Request.Files["qqfile"];
            file.SaveAs(path);
            return RedirectToAction("List");
        }

        protected void PrepareProductModel(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model, Product product)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (product != null)
            {
                model.Name = product.Name;
                model.SeName = model.SeName;
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
            var tags = _productService.GetAllProductTags();
            foreach (var tag in tags)
            {
                model.AvailableProductTags.Add(new SelectListItem()
                {
                    Text = tag.Name,
                    Value = tag.Id.ToString()
                });
            }
            if (product != null)
            {
                int[] productTags = new int[10];
                for (int i = 0; i < product.ProductTags.Count; i++)
                {
                    var pt = product.ProductTags.ToList()[i];
                    productTags[i] = pt.Id;
                }
                model.SelectedProductTags = productTags;
            }

            //Category Mapping

            var result = _categoryService.GetCategoryDetails();
            var categorymodel = new CategoryDetailModel(result);
            foreach (var category in categorymodel.Categories.CategoryList.GetFormattedBreadCrumb())
            {
                model.AvailableCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            }

            //Manufature mapping
            var manufatures = _manufatureService.GetAllManufacturers();
            foreach (var manufature in manufatures.ManufacturerList)
            {
                model.AvailableManufatures.Add(new SelectListItem()
                {
                    Text = manufature.Name,
                    Value = manufature.Id.ToString()
                });
            }

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