using Orbio.Services.Admin.Products;
using Orbio.Web.UI.Areas.Admin.Models.Product;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ProductTagController : Controller
    {
        private readonly IProductTagService productTagService;

        public ProductTagController(IProductTagService productTagService)
        {
            this.productTagService = productTagService;
        }

        // GET: Admin/ProductTag
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListProductTags(int? page)
        {
            var result = productTagService.GetAllProductTags();
            var model = (from pt in result
                         select new ProductTagModel()
                         {
                             Id = pt.Id,
                             Name = pt.Name,
                             ProductCount = pt.Products.Count(),
                         }).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            return PartialView(model.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Add()
        {
            var model = new ProductTagModel();
            return PartialView(model);
        }

        //public ActionResult Edit(int? Id)
        //{
        //    var result = productTagService.GetAllProductTagsById(Id.GetValueOrDefault());
        //    var model = new ProductTagModel()
        //                 {
        //                     Id = result.Id,
        //                     Name = result.Name,
        //                     ProductCount = result.Products.Count(),
        //                 };
        //    return View(model);
        //}

        [HttpPost]
        public ActionResult Update(int Id, FormCollection form)
        {
            if (form != null)
            {
                var name = form["txtname" + Id];
                var result = productTagService.UpdateProductTags(Id,name);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult AddProductTag(ProductTagModel model)
        {
            var result = productTagService.AddProductTags(model.Name);
            return RedirectToAction("List");
        }

        public ActionResult Delete(int? Id)
        {
            var result = productTagService.DeleteProductTags(Id.GetValueOrDefault());
            return RedirectToAction("List");
        }
    }
}