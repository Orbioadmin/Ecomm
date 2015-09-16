using Orbio.Core.Data;
using Orbio.Services.Admin.Attributes;
using Orbio.Web.UI.Areas.Admin.Models.Attribute;
using Orbio.Web.UI.Areas.Admin.Models.Catalog;
using Orbio.Web.UI.Filters;
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
            return PartialView(model);
        }

        public ActionResult EditProductAttribute(int Id)
        {
            var result = productAttributeService.GetProductAttributeById(Id);
            var model= new ProductAttributeModel(result);
            return View("CreateOrEditProductAttribute", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdateProductAttribute(ProductAttributeModel model)
        {
            productAttributeService.AddOrUpdateProductAttribute(model.Id, model.Name, model.Description);
            return RedirectToAction("ProductAttribute");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateProductAttribute(int Id, FormCollection form)
        {
            if (form != null)
            {
                var name = form["txtname" + Id];
                var description = form["txtdescription" + Id];
                var prodAttribute = new ProductAttributeModel
                {
                    Id = Id,
                    Name = name,
                    Description = description,
                };

                productAttributeService.AddOrUpdateProductAttribute(prodAttribute.Id, prodAttribute.Name, prodAttribute.Description);
            }
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

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
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

        public ActionResult AddSpecificationAttributeOption(int? Id, int? Spec)
        {
            var model = new SpecificationAttributeOptionModel();
            model.SpecificationAttributeId = Spec.GetValueOrDefault();
            return PartialView(model);
        }

        public ActionResult AddOrEditSpecificationAttributeOption(int? Id,int? Spec)
        {
            var model = new SpecificationAttributeOptionModel();
            if (Id != 0)
            {
                var result = specAttributeService.GetSpecificationAttributeOptionById(Id.GetValueOrDefault());
                if (result != null)
                {
                    model = new SpecificationAttributeOptionModel(result);
                }
            }
            model.SpecificationAttributeId = Spec.GetValueOrDefault();
            return View(model);
        }

        public ActionResult DeleteSpecificationAttributeOption(int? Id,int? Spec)
        {
            int result = specAttributeService.DeleteSpecificationAttributeOption(Id.GetValueOrDefault());
            return RedirectToAction("EditSpecificationAttribute", new { Id = Spec });
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddorEditOptions(FormCollection form)
        {
            var specModel = new SpecificationAttributeOptionModel();
            if(form!=null)
            {
                var name = form["txtname"];
                var displayorder = form["txtdisplayorder"];
                var id = form["hdnId"];
                var specId = form["hdnSpecId"];
                specModel = new SpecificationAttributeOptionModel
                {
                    Id = Convert.ToInt32(id),
                    Name = name,
                    DisplayOrder = Convert.ToInt32(displayorder),
                    SpecificationAttributeId = Convert.ToInt32(specId),
                };
                int result = specAttributeService.AddSpecificationOption(specModel.Id, specModel.Name, specModel.DisplayOrder, specModel.SpecificationAttributeId);
            }
            return RedirectToAction("EditSpecificationAttribute", new { Id = specModel.SpecificationAttributeId });
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOptions(FormCollection form)
        {
            var specModel = new SpecificationAttributeOptionModel();
            if(form!=null)
            {
                if(string.IsNullOrEmpty(form["hiddenSpecOptionId"]))
                    return RedirectToAction("ListSpecificationAttribute");

                int optionId = Convert.ToInt32(form["hiddenSpecOptionId"]);
                var name = form["txtname" + optionId];
                var displayorder = form["txtdisplayorder" + optionId];
                var specId = form["hiddenSpecId"];
                specModel = new SpecificationAttributeOptionModel
                {
                    Id = Convert.ToInt32(optionId),
                    Name = name,
                    DisplayOrder = Convert.ToInt32(displayorder),
                    SpecificationAttributeId = Convert.ToInt32(specId),
                };
                int result = specAttributeService.AddSpecificationOption(specModel.Id, specModel.Name, specModel.DisplayOrder, specModel.SpecificationAttributeId);
            }

            return RedirectToAction("EditSpecificationAttribute", new { Id = specModel.SpecificationAttributeId });
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