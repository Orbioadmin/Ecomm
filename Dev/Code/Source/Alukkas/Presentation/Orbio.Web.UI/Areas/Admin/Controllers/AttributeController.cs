using Orbio.Services.Admin.Attributes;
using Orbio.Web.UI.Areas.Admin.Models.Attribute;
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

        public AttributeController(IProductAttributeService attributeService, ISpecificationAttributeService specAttributeService)
        {
            this.productAttributeService = attributeService;
            this.specAttributeService = specAttributeService;
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
            int result = productAttributeService.AddOrUpdateProductAttribute(model.Id, model.Name, model.Description);
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
            var result = specAttributeService.GetSpecificationAttributesById(Id.GetValueOrDefault());
            var model = new SpecificationAttributeModel(result);
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
    }
}