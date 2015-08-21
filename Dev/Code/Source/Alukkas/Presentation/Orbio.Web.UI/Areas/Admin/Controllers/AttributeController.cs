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
        private readonly IProductAttributeService attributeService;

        public AttributeController(IProductAttributeService attributeService)
        {
            this.attributeService = attributeService;
        }
        // GET: Admin/Attribute
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductAttribute()
        {
            var result = attributeService.GetProductAttributes();
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
            var result = attributeService.GetProductAttributeById(Id);
            var model= new ProductAttributeModel(result);
            return View("CreateOrEditProductAttribute", model);
        }

        [HttpPost]
        public ActionResult AddOrUpdateProductAttribute(ProductAttributeModel model)
        {
                int result = attributeService.AddOrUpdateProductAttribute(model.Id, model.Name, model.Description);
                return RedirectToAction("ProductAttribute");
        }

        public ActionResult DeleteProductAttribute(int Id)
        {
            int result = attributeService.DeleteProductAttribute(Id);
            return RedirectToAction("ProductAttribute");
        }
    }
}