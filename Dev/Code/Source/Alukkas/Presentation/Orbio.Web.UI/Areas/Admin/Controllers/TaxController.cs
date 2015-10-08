using Orbio.Core.Data;
using Orbio.Services.Admin.Tax;
using Orbio.Web.UI.Areas.Admin.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class TaxController : Controller
    {
        private readonly ITaxCategoryService taxCategoryService;

        public TaxController(ITaxCategoryService taxCategoryService)
        {
            this.taxCategoryService = taxCategoryService;
        }

        // GET: Admin/Tax
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaxCategory()
        {
            return View();
        }

        public ActionResult ListTaxCategories()
        {
            var result = taxCategoryService.GetAllTaxCategories();
            var model = (from tc in result
                         select new TaxCategoryModel(tc)).ToList();
            return PartialView(model);
        }

        public ActionResult AddTaxCategory()
        {
            var model = new TaxCategoryModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult UpdateTaxCategory(int Id,FormCollection form)
        {
            if(form!=null)
            {
                var name = form["txtname" + Id];
                var displayOrder = Convert.ToInt32(form["txtdisplayorder" + Id]);
                var taxRate =  Convert.ToDecimal(form["txttaxrate" + Id]);

                taxCategoryService.AddOrUpdate(Id,name,displayOrder,taxRate);
            }
            return RedirectToAction("TaxCategory");
        }

        [HttpPost]
        public ActionResult AddTaxCategory(TaxCategoryModel model)
        {
            taxCategoryService.AddOrUpdate(model.Id, model.Name, model.DisplayOrder, model.TaxRate);
            return RedirectToAction("TaxCategory");
        }

        public ActionResult DeleteTaxCategory(int Id)
        {
            taxCategoryService.Delete(Id);
            return RedirectToAction("TaxCategory");
        }
    }
}