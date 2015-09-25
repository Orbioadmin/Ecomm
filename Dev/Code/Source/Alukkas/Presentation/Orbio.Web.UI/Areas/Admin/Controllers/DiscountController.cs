using Orbio.Services.Admin.Discount;
using Orbio.Web.UI.Areas.Admin.Models.Discount;
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

        #endregion
    }
}