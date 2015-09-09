using Orbio.Core.Data;
using Orbio.Services.Admin.Attributes;
using Orbio.Web.UI.Areas.Admin.Models.Attribute;
using Orbio.Web.UI.Areas.Admin.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class AttributeController : Controller
    {
        private readonly IProductAttributeService productAttributeService;
        private readonly ISpecificationAttributeService specAttributeService;
        private readonly ICheckoutAttributeService checkoutAttributeService;

        public AttributeController(IProductAttributeService attributeService, ISpecificationAttributeService specAttributeService, 
            ICheckoutAttributeService checkoutAttributeService)
        {
            this.productAttributeService = attributeService;
            this.specAttributeService = specAttributeService;
            this.checkoutAttributeService = checkoutAttributeService;
        }
        // GET: Admin/Attribute
        public ActionResult Index()
        {
            return View();
        }

        #region Product Attribute

        public ActionResult ProductAttribute()
        {
            var result = productAttributeService.GetProductAttributes();
            var model = (from prod in result
                         select new ProductAttributeModel(prod)).ToList();
           
            return View(model);
        }

        public ActionResult AddProductAttribute()
        {
            var model = new ProductAttributeModel();
            return View("CreateOrEditProductAttribute", model);
        }

        public ActionResult EditProductAttribute(int Id)
        {
            var result = productAttributeService.GetProductAttributeById(Id);
            var model= new ProductAttributeModel(result);
            return View("CreateOrEditProductAttribute", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateProductAttribute(ProductAttributeModel model)
        {
            productAttributeService.AddOrUpdateProductAttribute(model.Id, model.Name, model.Description);
                return RedirectToAction("ProductAttribute");
        }

        public ActionResult DeleteProductAttribute(int Id)
        {
            int result = productAttributeService.DeleteProductAttribute(Id);
            return RedirectToAction("ProductAttribute");
        }

        #endregion

        #region SpecificationAttributes

        public ActionResult ListSpecificationAttribute()
        {
            var result = specAttributeService.GetSpecificationAttributes();
            var model = (from spec in result
                         select new SpecificationAttributeModel(spec)).ToList();

            return View(model);
        }

        public ActionResult AddSpecificationAttribute()
        {
            var model = new SpecificationAttributeModel();
            return View("SpecificationAttribute", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateSpecificationAttribute(SpecificationAttributeModel model)
        {
            int result = specAttributeService.AddOrUpdate(model.Id, model.Name, model.DisplayOrder);
            if(model.Id==0)
            {
                return RedirectToAction("EditSpecificationAttribute", new { Id=result });
            }
            return RedirectToAction("ListSpecificationAttribute");
        }

        public ActionResult EditSpecificationAttribute(int? Id)
        {
            var model = new SpecificationAttributeModel();
            var result = specAttributeService.GetSpecificationAttributesById(Id.GetValueOrDefault());
            if(result!=null)
            {
                model = new SpecificationAttributeModel(result);
            }
            return View("SpecificationAttribute", model);
        }

        public ActionResult DeleteSpecificationAttribute(int? Id)
        {
            int result = specAttributeService.DeleteSpecificationAttribute(Id.GetValueOrDefault());
            return RedirectToAction("ListSpecificationAttribute");
        }

        public ActionResult AddOrEditSpecificationAttributeOption(int? Id,int? Spec)
        {
            var model = new SpecificationAttributeOptionModel();
            if (Id != 0)
            {
                var result = specAttributeService.GetSpecificationAttributeOptionById(Id.GetValueOrDefault());
                model = new SpecificationAttributeOptionModel(result);
            }
            model.SpecificationAttributeId = Spec.GetValueOrDefault();
            return View(model);
        }

        public ActionResult DeleteSpecificationAttributeOption(int? Id,int? Spec)
        {
            int result = specAttributeService.DeleteSpecificationAttributeOption(Id.GetValueOrDefault());
            return RedirectToAction("EditSpecificationAttribute", new { Id = Spec });
        }

        [HttpPost]
        public ActionResult AddorEditOptions(SpecificationAttributeOptionModel model)
        {
            int result = specAttributeService.AddSpecificationOption(model.Id, model.Name, model.DisplayOrder, model.SpecificationAttributeId);
            return RedirectToAction("EditSpecificationAttribute", new { Id = model.SpecificationAttributeId });
        }

        #endregion

        #region Checkout Attribute

        public ActionResult GetCheckoutAttribute()
        {
            var result = checkoutAttributeService.GetCheckoutAttribute();
            var model = (from checkout in result
                         select new CheckoutAttributeModel(checkout)).ToList();
            return View("ListCheckoutAttribute", model);
        }

        public ActionResult EditCheckoutAttribute(int? Id)
        {
            var model = new CheckoutAttributeModel(checkoutAttributeService.GetCheckoutAttributeById(Id.GetValueOrDefault()));
            var taxCategory =checkoutAttributeService.GetTaxCategory();
            model.TaxCategory = (from T in taxCategory
                                 select new TaxCategoryModel(T)).ToList();
            return View("AddOrEditCheckoutAttribute",model);
        }

        public ActionResult AddCheckoutAttribute()
        {
            var model = new CheckoutAttributeModel();
            var taxCategory = checkoutAttributeService.GetTaxCategory();
            model.TaxCategory = (from T in taxCategory
                                 select new TaxCategoryModel(T)).ToList();
            return View("AddOrEditCheckoutAttribute", model);
        }

        public ActionResult SaveCheckoutAttribute(CheckoutAttributeModel model)
        {
                var checkout = new CheckoutAttribute()
                                {
                                    Id = model.Id,
                                    Name = model.Name,
                                    TextPrompt = model.TextPrompt,
                                    IsRequired = model.IsRequired,
                                    ShippableProductRequired = model.ShippableProduct,
                                    IsTaxExempt = model.IsTaxExempt,
                                    TaxCategoryId = model.TaxCategoryId,
                                    AttributeControlTypeId = model.ControlTypeId,
                                    DisplayOrder = model.DisplayOrder
                                };
                var result = checkoutAttributeService.AddOrEditCheckoutAttribute(checkout);
                if (model.Id != 0)
                {
                    return RedirectToAction("GetCheckoutAttribute");
                }
                else
                {
                    return RedirectToAction("EditCheckoutAttribute", new { Id = result });
                }
        }

        public ActionResult DeleteCheckoutAttribute(int Id)
        {
            var result = checkoutAttributeService.DeleteCheckoutAttribute(Id);
            return RedirectToAction("GetCheckoutAttribute");
        }

        public ActionResult DeleteCheckoutAttributeValue(int? Id,int? AttrId)
        {
            var result = checkoutAttributeService.DeleteCheckoutAttributeValue(Id.GetValueOrDefault());
            return RedirectToAction("EditCheckoutAttribute", new { Id = AttrId.GetValueOrDefault() });
        }

        public ActionResult SaveCheckoutAttributeValue(CheckoutAttributeValueModel model)
        {
            var checkValue = new CheckoutAttributeValue
                            {
                                Id = model.Id,
                                CheckoutAttributeId=model.CheckoutAttributeId,
                                Name = model.Name,
                                WeightAdjustment = model.WeightAdjustment,
                                PriceAdjustment = model.PriceAdjustment,
                                IsPreSelected = model.IsPreSelected,
                                DisplayOrder = model.DisplayOrder,
                            };
            var result = checkoutAttributeService.AddOrEditCheckoutAttributeValue(checkValue);
            return RedirectToAction("EditCheckoutAttribute", new { Id = model.CheckoutAttributeId });
        }

        public ActionResult AddCheckoutAttributeValue(int Id)
        {
            var model = new CheckoutAttributeValueModel();
            model.CheckoutAttributeId = Id;
            return View("AddOrEditCheckoutAttributeValue", model);
        }

        public ActionResult EditCheckoutAttributeValue(int Id, int AttrId)
        {
            var result = checkoutAttributeService.AddOrEditCheckoutAttributeValue(Id);
            var model = new CheckoutAttributeValueModel(result);
            return View("AddOrEditCheckoutAttributeValue", model);
        }

        #endregion
    }
}