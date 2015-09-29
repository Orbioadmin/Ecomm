using Orbio.Services.Admin.Discount;
using Orbio.Web.UI.Areas.Admin.Models.Discount;
using Orbio.Web.UI.Filters;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        #region Fields

        public readonly IDiscountService _discountService;

        #endregion

        #region Constructors
        public DiscountController(IDiscountService discountService)
        {
            this._discountService = discountService;
        }
        #endregion

        #region Methods

        // GET: Admin/Discount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var model = GetDiscountList();
            return View(model);
        }

        public List<DiscountModel> GetDiscountList()
        {
            return (from d in _discountService.GetAllDiscounts()
                    select new DiscountModel(d)).ToList();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(DiscountModel model)
        {
                var discount = model.ToEntity();
                _discountService.CreateOrUpdateDiscount("insert",discount);

                //activity log
                //_customerActivityService.InsertActivity("AddNewDiscount", _localizationService.GetResource("ActivityLog.AddNewDiscount"), discount.Name);

                //SuccessNotification(_localizationService.GetResource("Admin.Promotions.Discounts.Added"));
                //return continueEditing ? RedirectToAction("Edit", new { id = discount.Id }) : RedirectToAction("List");

                return RedirectToAction("List");
        }
        public ActionResult Edit(int id)
        {
            var discount = new Orbio.Core.Domain.Discounts.Discount
            {
                Id = id
            };
            var discountDetail = _discountService.GetDiscountById("selectById", discount);
            if (discountDetail == null)
                //No product found with the specified id
                return RedirectToAction("List");
            var model = new DiscountModel();
            PrepareDiscounttModel(model, discountDetail);
            return View(model);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(DiscountModel model)
        {
            var discount = model.ToEntity();
            _discountService.CreateOrUpdateDiscount("update", discount);
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            var discount = new Orbio.Core.Domain.Discounts.Discount
            {
                Id = id
            };
            _discountService.CreateOrUpdateDiscount("delete", discount);
             return RedirectToAction("List");
        }

        [ChildActionOnly]
        public ActionResult UsageHistoryInfo(int id)
        {
            var resultModel = _discountService.GetAllUsageHistoryByDiscountId(id);

            return PartialView(resultModel);
        }
        protected void PrepareDiscounttModel(DiscountModel model, Orbio.Core.Domain.Discounts.Discount discount)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            if (discount != null)
            {
                model.Id = discount.Id;
                model.Name = discount.Name;
                model.DiscountTypeId = discount.DiscountTypeId;
                model.UsePercentage = discount.UsePercentage;
                model.DiscountPercentage = discount.DiscountPercentage;
                model.DiscountAmount = discount.DiscountAmount;
                model.RequiresCouponCode = discount.RequiresCouponCode;
                model.CouponCode = discount.CouponCode;
                model.DiscountLimitationId = discount.DiscountLimitationId;
                model.LimitationTimes = discount.LimitationTimes;
                model.StartDateUtc = discount.StartDateUtc;
                model.EndDateUtc = discount.EndDateUtc;
                if (discount.Categories != null && discount.Categories.Count > 0)
                {
                    model.Categories = (from c in discount.Categories
                                       select new CategorySimpleModel(c)).ToList();
                }
                if (discount.Products != null && discount.Products.Count > 0)
                {
                    model.Products = (from p in discount.Products
                                     select new ProductOverViewModel(p)).ToList();
                }
            }
        }


        public ActionResult DeleteUsageHistory(int id,int discountId)
        {
            _discountService.DeleteUsageHistory(id);
            return RedirectToAction("Edit", "Discount", new { id = discountId });
        }
        #endregion
    }
}