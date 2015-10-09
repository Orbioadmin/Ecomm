using Orbio.Services.Admin.Attributes;
using Orbio.Web.UI.Areas.Admin.Models.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class SpecificationAttributeController : Controller
    {
        public readonly ISpecificationAttributeService _specificationAttributeService;
        public SpecificationAttributeController(ISpecificationAttributeService specificationAttributeService)
        {
            this._specificationAttributeService = specificationAttributeService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AttributeOptions(int id)
        {
            var attributeOptions = (from sao in _specificationAttributeService.GetSpecificationAttributeOptionBySpecId(id)
                                    select new SpecificationAttributeOptionModel
                                    {
                                        Name = sao.Name,
                                        Id = sao.Id
                                    }).ToList();
            return Json(attributeOptions);
        }
    }
}