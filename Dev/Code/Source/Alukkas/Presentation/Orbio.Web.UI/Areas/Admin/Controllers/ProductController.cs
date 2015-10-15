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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Orbio.Services.Admin.Media;
using System.Web.Script.Serialization;
using Orbio.Services.Admin.Attributes;
using Orbio.Services.Admin.Seo;
using Orbio.Services.Admin.Discount;
using Orbio.Core.Domain.Discounts;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        #region Fields

        public readonly ICategoryServices _categoryService;
        public readonly IManufacturerService _manufatureService;
        public readonly IProductService _productService;
        public readonly Orbio.Services.Catalog.IProductService _productServices;
        public readonly IShippingService _shippingService;
        public readonly ITaxCategoryService _taxCategoryService;
        public readonly IPictureService _pictureService;
        public readonly ISpecificationAttributeService _specificationAttributeService;
        public readonly IProductAttributeService _productAttributeService;
        public readonly IDiscountService _discountService;
        public readonly IUrlRecordService _urlRecordService;
        #endregion

        #region Constructors
        public ProductController(ICategoryServices categoryService, IManufacturerService manufatureService, IProductService productService, IShippingService shippingService, ITaxCategoryService taxCategoryService,
            IPictureService pictureService, ISpecificationAttributeService specificationAttributeService, IProductAttributeService productAttributeService, Orbio.Services.Catalog.IProductService productServices,
            IDiscountService discountService, IUrlRecordService urlRecordService)
        {
            this._categoryService = categoryService;
            this._manufatureService = manufatureService;
            this._productService = productService;
            this._shippingService = shippingService;
            this._taxCategoryService = taxCategoryService;
            this._pictureService = pictureService;
            this._specificationAttributeService = specificationAttributeService;
            this._productAttributeService = productAttributeService;
            this._productServices = productServices;
            this._discountService = discountService;
            this._urlRecordService = urlRecordService;
        }
        #endregion

        #region Methods

        #region Product list / create / edit / delete

        //List products
        [AdminAuthorizeAttribute]
        public ActionResult List(ProductListModel model)
        {
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

        public ActionResult ProductList(ProductListModel model,int? page)
        {
            var resultModel = GetProductList(model);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);
            return View(resultModel.ToPagedList(pageNumber,pageSize));
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
            var productDetails = CreateOrUpdateProduct(model);
            int productId = _productService.InsertNewProduct(productDetails);
            return RedirectToAction("Edit", "Product", new { id = productId });
            
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
            model.SeName = _urlRecordService.GetSeName(product.Id, "Product");
            model.Pictures = (from p in product.Product_Picture_Mapping
                              orderby p.DisplayOrder ascending
                              select p).ToList();
            model.ProductSpecification = (from p in product.Product_SpecificationAttribute_Mapping
                                          orderby p.DisplayOrder ascending
                                          select p).ToList();
            model.ProductVariantAttribute = (from p in product.Product_ProductAttribute_Mapping
                                             orderby p.DisplayOrder ascending
                                             select p).ToList();
            PrepareProductModel(model, product);
            return View(model);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProduct(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var productDetails = CreateOrUpdateProduct(model);
            _productService.UpdateProduct(productDetails);
            return RedirectToAction("Edit", "Product", new { id = model.Id });

        }

        public Orbio.Core.Domain.Admin.Product.ProductDetail CreateOrUpdateProduct(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var product = model.ToEntity();
            product.CreatedOnUtc = DateTime.UtcNow;
            product.UpdatedOnUtc = DateTime.UtcNow;
            var productDetails = new Orbio.Core.Domain.Admin.Product.ProductDetail
            {
                product = product.ToDomainModel(),
                seName = _urlRecordService.ValidateSeName(product.Id,model.SeName,product.Name,"Product"),
                productTags = model.SelectedProductTags,
                catgoryIds = model.SelectedCategories,
                manufactureIds = model.SelectedManufature,
                relatedProductIds = model.SelectedRelatedProducts,
                similarProductIds = model.SelectedSimilarProducts,
                discountIds = model.SelectedDiscounts,
            };
            return productDetails;
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

                #region Mapping Product Tags
                int[] productTags = new int[product.ProductTags.Count];
                for (int i = 0; i < product.ProductTags.Count; i++)
                {
                    var pt = product.ProductTags.ToList()[i];
                    productTags[i] = pt.Id;
                }
                model.SelectedProductTags = productTags;
                #endregion
                #region Mapping Categories
                int[] productCategory = new int[product.Product_Category_Mapping.Count];
                for (int i = 0; i < product.Product_Category_Mapping.Count; i++)
                {
                    var pt = product.Product_Category_Mapping.ToList()[i];
                    productCategory[i] = pt.CategoryId;
                }
                model.SelectedCategories = productCategory;
                #endregion
                #region Mapping Manufatures
                int[] productManufacture = new int[product.Product_Manufacturer_Mapping.Count];
                for (int i = 0; i < product.Product_Manufacturer_Mapping.Count; i++)
                {
                    var pt = product.Product_Manufacturer_Mapping.ToList()[i];
                    productManufacture[i] = pt.ManufacturerId;
                }
                model.SelectedManufature = productManufacture;
                #endregion
                #region Mapping Related Products
                var relatedProductsMapping = _productServices.GetRelatedProductsById(product.Id);
                int[] productRelatedProducts = new int[relatedProductsMapping.ProductDetails.Count];
                for (int i = 0; i < relatedProductsMapping.ProductDetails.Count; i++)
                {
                    var pt = relatedProductsMapping.ProductDetails.ToList()[i];
                    productRelatedProducts[i] = pt.Id;
                }
                model.SelectedRelatedProducts = productRelatedProducts;
                #endregion
                #region Mapping Similar Products
                var similarProductsMapping = _productServices.GetSimilarProductsById(product.Id);
                int[] productSimilarProducts = new int[similarProductsMapping.ProductDetails.Count];
                for (int i = 0; i < similarProductsMapping.ProductDetails.Count; i++)
                {
                    var pt = similarProductsMapping.ProductDetails.ToList()[i];
                    productSimilarProducts[i] = pt.Id;
                }
                model.SelectedSimilarProducts = productSimilarProducts;
                #endregion
                #region Mapping Product Discounts
                var productsDiscounts = _productService.GetAllDiscountsForProductsById(product.Id);
                int[] productDiscount = new int[productsDiscounts.Count];
                for (int i = 0; i < productsDiscounts.Count; i++)
                {
                    var pt = productsDiscounts.ToList()[i];
                    productDiscount[i] = pt.Id;
                }
                model.SelectedDiscounts = productDiscount;
                #endregion
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

            //Related Products
            var relatedProducts = _productService.GetAllProductsSeachOrDefault("",0,0);
            foreach (var relatedProduct in relatedProducts)
            {
                model.AvailableRelatedProducts.Add(new SelectListItem()
                {
                    Text = relatedProduct.Name,
                    Value = relatedProduct.Id.ToString()
                });
            }

            //Similar Products
            var similarProducts = _productService.GetAllProductsSeachOrDefault("",0,0);
            foreach (var similarProduct in similarProducts)
            {
                model.AvailableSimilarProducts.Add(new SelectListItem()
                {
                    Text = similarProduct.Name,
                    Value = similarProduct.Id.ToString()
                });
            }

            //Discounts for Products
            var discounts = _discountService.GetAllDiscountsForProducts(DiscountType.AssignedToSkus);
            foreach (var discount in discounts)
            {
                model.AvailableDiscounts.Add(new SelectListItem()
                {
                    Text = discount.Name,
                    Value = discount.Id.ToString()
                });
            }

            //tax categories
            var taxCategories = _taxCategoryService.GetAllTaxCategories();
            model.AvailableTaxCategories.Add(new SelectListItem() { Text = "---", Value = "0" });
            foreach (var tc in taxCategories)
                model.AvailableTaxCategories.Add(new SelectListItem() { Text = tc.Name, Value = tc.Id.ToString() });

            //specification attributes
            var specificationAttributes = _specificationAttributeService.GetSpecificationAttributes();
            for (int i = 0; i < specificationAttributes.Count; i++)
            {
                var sa = specificationAttributes[i];
                model.AddSpecificationAttributeModel.AvailableAttributes.Add(new SelectListItem() { Text = sa.Name, Value = sa.Id.ToString() });
                if (i == 0)
                {
                    //attribute options
                    foreach(var sao in _specificationAttributeService.GetSpecificationAttributeOptionBySpecId(sa.Id))
                        model.AddSpecificationAttributeModel.AvailableOptions.Add(new SelectListItem() { Text = sao.Name, Value = sao.Id.ToString() });
                }
            }

            //product attributes
            var productAttributes = _productAttributeService.GetProductAttributes();
            foreach (var productAttribute in productAttributes)
            {
                model.AddVariantAttributeModel.AvailableProductAttributes.Add(new SelectListItem()
                {
                    Text = productAttribute.Name,
                    Value = productAttribute.Id.ToString()
                });
            }

            //default values
                model.StockQuantity = 10000;
                model.OrderMinimumQuantity = 1;
                model.OrderMaximumQuantity = 10000;
                model.AllowCustomerReviews = true;
                model.Published = true;
        }

        #endregion

        #region Product Image
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadProductImage(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var product = _productService.GetProductById(model.Id);
            var pictureCount = (from p in product.Product_Picture_Mapping
                                select p).LastOrDefault();
            int displayOrder = (pictureCount != null) ? pictureCount.DisplayOrder : 0;
            var productPicture = new Product_Picture_Mapping
            {
                ProductId = model.Id,
                PictureId = model.PictureModel.PictureId,
                DisplayOrder = displayOrder + 1
            };
            _productService.InsertToProductPictureMapping(productPicture);
            return RedirectToAction("Edit", "Product", new { id = model.Id });
        }

        [HttpPost]
        public ActionResult UpdatePicture(FormCollection formCollection)
        {
            var value = formCollection["Ids"];
            object[] ids = System.Web.Helpers.Json.Decode(value);
            int[] pictureIds = ids.Select(n => Convert.ToInt32(n)).ToArray();
            _productService.UpdatePictureDisplayOrder(pictureIds);
            return Json(new { success = true });
        }

        public ActionResult DeleteProductPicture(int id, int productId)
        {
            _productService.DeleteProductPicture(id);
            return RedirectToAction("Edit", "Product", new { id = productId });
        }

        #endregion

        #region Product Specification Attribute

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewProductSpecificationAttribute(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var productSpecification = new Product_SpecificationAttribute_Mapping { 
            ProductId = model.Id,
            SpecificationAttributeOptionId = model.AddSpecificationAttributeModel.SpecificationAttributeOptionId,
            AllowFiltering = model.AddSpecificationAttributeModel.AllowFiltering,
            ShowOnProductPage = model.AddSpecificationAttributeModel.ShowOnProductPage,
            SubTitle = model.AddSpecificationAttributeModel.SubTitle
            };
            _productService.InsertProductSpecificationAttribute(productSpecification);
            return RedirectToAction("Edit", "Product", new { id = model.Id });
        }

        [HttpPost]
        public ActionResult EditProductSpecification(int Id, FormCollection form)
        {
                var allowFilter = form["AddSpecificationAttributeModel_AllowFiltering" + Id];
                var showOnHome = form["AddSpecificationAttributeModel_ShowOnProductPage" + Id];
                var productSpecification = new Product_SpecificationAttribute_Mapping
                {
                    Id = Id,
                    AllowFiltering = Convert.ToBoolean(allowFilter),
                    ShowOnProductPage = Convert.ToBoolean(showOnHome),
                };
                int productId = _productService.UpdateProductSpecificationAttribute(productSpecification);
                return RedirectToAction("Edit", "Product", new { id = productId });
        }


        [HttpPost]
        public ActionResult UpdateSpecificationDisplayOrder(FormCollection formCollection)
        {
            var value = formCollection["SpecIds"];
            object[] ids = System.Web.Helpers.Json.Decode(value);
            int[] specificationIds = ids.Select(n => Convert.ToInt32(n)).ToArray();
            _productService.UpdateSpecificationDisplayOrder(specificationIds);
            return Json(new { success = true });
        }

        public ActionResult DeleteSpecificationAttribute(int id, int productId)
        {
            _productService.DeleteProductSpecificationAttribute(id);
            return RedirectToAction("Edit", "Product", new { id = productId });
        }

        #endregion

        #region Product Variant Attribute

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewProductVariantAttribute(Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var productVariantAttribute = new Product_ProductAttribute_Mapping
            {
                ProductId = model.Id,
                ProductAttributeId = model.AddVariantAttributeModel.ProductAttributeId,
                IsRequired = model.AddVariantAttributeModel.IsRequired,
                AttributeControlTypeId = model.AddVariantAttributeModel.AttributeControlTypeId,
                TextPrompt = model.AddVariantAttributeModel.TextPrompt,
            };
            _productService.InsertProductVariantAttribute(productVariantAttribute);
            return RedirectToAction("Edit", "Product", new { id = model.Id });
        }

        [HttpPost]
        public ActionResult AddSizeGuideUrl(int productVariantAttributeId,Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var picture = _pictureService.GetPictureById(model.PictureModel.PictureId);
            var productAttribute = new Product_ProductAttribute_Mapping
            {
                Id = productVariantAttributeId,
                SizeGuideUrl = picture.RelativeUrl
            };
            _productService.AddOrUpdateProductVariantAttributeSizeGuide(productAttribute);
            return RedirectToAction("AddProductVariantSizeGuide", "Product", new { productVariantAttributeId = productVariantAttributeId });
        }

        public ActionResult DeleteSizeGuideUrl(int id)
        {
            _productService.DeleteSizeGuideUrl(id);
            return RedirectToAction("AddProductVariantSizeGuide", "Product", new { productVariantAttributeId = id });
        }

        public ActionResult AddProductVariantSizeGuide(int productVariantAttributeId)
        {

            var productVariantAttribute = _productService.GetProductVariantAttributeProductVariantAttributeId(productVariantAttributeId);
            var model = new Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel
            {
                Id = productVariantAttribute.Select(p => p.Product.Id).FirstOrDefault(),
                Name = productVariantAttribute.Select(p => p.Product.Name).FirstOrDefault(),
                ProductVariantAttribute = productVariantAttribute
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProductVariantAttribute(int Id, FormCollection form)
        {
            var productAttributeId = form["AddVariantAttributeModel_ProductAttributeId" + Id];
            var textPrompt = form["AddVariantAttributeModel_TextPrompt" + Id];
            var isRequired = form["AddVariantAttributeModel_IsRequired" + Id];
            var attributeControlTypeId = form["AddVariantAttributeModel_AttributeControlTypeId" + Id];
            var productVariantAttribute = new Product_ProductAttribute_Mapping
            {
                Id=Id,
                ProductAttributeId = Convert.ToInt32(productAttributeId),
                IsRequired = Convert.ToBoolean(isRequired),
                AttributeControlTypeId = Convert.ToInt32(attributeControlTypeId),
                TextPrompt = textPrompt,
            };
            int productId = _productService.UpdateProductVariantAttribute(productVariantAttribute);
            return RedirectToAction("Edit", "Product", new { id = productId });
        }

        public ActionResult DeleteProductVariantAttribute(int id, int productId)
        {
            _productService.DeleteProductVariantAttribute(id);
            return RedirectToAction("Edit", "Product", new { id = productId });
        }

        [HttpPost]
        public ActionResult UpdateProductVariantAttributeDisplayOrder(FormCollection formCollection)
        {
            var value = formCollection["PvaIds"];
            object[] ids = System.Web.Helpers.Json.Decode(value);
            int[] PvaIds = ids.Select(n => Convert.ToInt32(n)).ToArray();
            _productService.UpdateProductVariantAttributeDisplayOrder(PvaIds);
            return Json(new { success = true });
        }

        #endregion

        #region Product Variant Attribute Value

        public ActionResult AddOrUpdateProductAttributeValue(int productVariantAttributeId)
        {
            var productVariantAttributeValue = _productAttributeService.GetProductVariantAttributeValueByProductVariantAttributeId(productVariantAttributeId);
            var model = new Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel {
                Id = productVariantAttributeValue.Select(p=>p.Product_ProductAttribute_Mapping.Product.Id).FirstOrDefault(),
                Name = productVariantAttributeValue.Select(p=>p.Product_ProductAttribute_Mapping.Product.Name).FirstOrDefault(),
                ProductVariantAttributeValue = productVariantAttributeValue
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNewProductVariantAttributeValue(int productVariantAttributeId,Orbio.Web.UI.Areas.Admin.Models.Product.ProductModel model)
        {
            var productVariantAttributeValue = new ProductVariantAttributeValue
            {
                ProductVariantAttributeId = productVariantAttributeId,
                Name = model.AddVariantAttributeValueModel.Name,
                PriceAdjustment = model.AddVariantAttributeValueModel.PriceAdjustment,
                WeightAdjustment = model.AddVariantAttributeValueModel.WeightAdjustment,
                IsPreSelected = model.AddVariantAttributeValueModel.IsPreSelected,
            };
            _productAttributeService.InsertProductVariantAttributeValue(productVariantAttributeValue);
            return RedirectToAction("AddOrUpdateProductAttributeValue", "Product", new { productVariantAttributeId = productVariantAttributeId });
        }

        [HttpPost]
        public ActionResult EditProductVariantAttributeValue(int Id,FormCollection form)
        {
            var name = form["AddVariantAttributeValueModel_Name" + Id];
            var priceAdjustment = form["AddVariantAttributeValueModel_PriceAdjustment" + Id];
            var weightAdjustment = form["AddVariantAttributeValueModel_WeightAdjustment" + Id];
            var isPreSelected = form["AddSpecificationAttributeModel_IsPreSelected" + Id];
            var productVariantAttributeValue = new ProductVariantAttributeValue
            {
                Id=Id,
                Name = name,
                PriceAdjustment = Convert.ToDecimal(priceAdjustment),
                WeightAdjustment = Convert.ToDecimal(weightAdjustment),
                IsPreSelected = Convert.ToBoolean(isPreSelected),
            };
            int productVariantAttributeId = _productAttributeService.UpdateProductVariantAttributeValue(productVariantAttributeValue);
            return RedirectToAction("AddOrUpdateProductAttributeValue", "Product", new { productVariantAttributeId = productVariantAttributeId });
        }

        public ActionResult DeleteProductVariantAttributeValue(int id, int productVariantAttributeId)
        {
            _productAttributeService.DeleteProductVariantAttributeValue(id);
            return RedirectToAction("AddOrUpdateProductAttributeValue", "Product", new { productVariantAttributeId = productVariantAttributeId });
        }

        [HttpPost]
        public ActionResult UpdateProductVariantAttributeValueDisplayOrder(FormCollection formCollection)
        {
            var value = formCollection["PvavIds"];
            object[] ids = System.Web.Helpers.Json.Decode(value);
            int[] PvavIds = ids.Select(n => Convert.ToInt32(n)).ToArray();
            _productAttributeService.UpdateProductVariantAttributeValueDisplayOrder(PvavIds);
            return Json(new { success = true });
        }

        #endregion

        #endregion
    }
}