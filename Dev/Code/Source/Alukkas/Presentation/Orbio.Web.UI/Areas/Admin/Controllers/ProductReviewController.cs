using Orbio.Services.Admin.Products;
using Orbio.Web.UI.Areas.Admin.Models.Product;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Orbio.Services.Helpers;
using Orbio.Web.UI.Filters;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ProductReviewController : Controller
    {
        private readonly IProductReviewService productReviewService;
        private readonly IDateTimeHelper _dateTimeHelper;

        public ProductReviewController(IProductReviewService productReviewService, IDateTimeHelper _dateTimeHelper)
        {
            this.productReviewService = productReviewService;
            this._dateTimeHelper = _dateTimeHelper;
        }
        // GET: Admin/ProductReview
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Search(ProductReviewSearchModel model)
        {
            return PartialView(model);
        }

        public ActionResult ProductReviewList(ProductReviewSearchModel model, int? page)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                        : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var prodReviewModel = new List<ProductReviewModel>();
            var result = productReviewService.GetAllProductReviews(startDateValue, endDateValue, model.Message);
            prodReviewModel = (from pr in result
                     select new ProductReviewModel(pr)).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            return PartialView(prodReviewModel.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Edit(int? id)
        {
            var result = productReviewService.EditProductReview(id.GetValueOrDefault());
            if(result==null)
                 return View("List");
            var model = new ProductReviewModel(result);
            return View(model);
        }

        public ActionResult Update(ProductReviewModel model)
        {
            var result = productReviewService.UpdateProductReview(model.Id,model.Title,model.ReviewText,model.IsApproved);
            return RedirectToAction("List");
        }

        public ActionResult DeleteProductReview(int? Id)
        {
            var result = productReviewService.DeleteProductReview(Id.GetValueOrDefault());
            return RedirectToAction("List");
        }

         [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Approve(int[] Review)
        {
            var result = productReviewService.GetAllProductReviews(null, null, null);
            result = result.Where(c => Review.Contains(c.Id)).ToList();
            var prodReview = productReviewService.Approve(result);
            return RedirectToAction("List");
        }

         [HttpParamAction]
         [AcceptVerbs(HttpVerbs.Post)]
         public ActionResult Disapprove(int[] Review)
         {
             var result = productReviewService.GetAllProductReviews(null, null, null);
             result = result.Where(c => Review.Contains(c.Id)).ToList();
             var prodReview = productReviewService.Disapprove(result);
             return RedirectToAction("List");
         }
    }
}