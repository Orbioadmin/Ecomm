using Orbio.Core.Data;
using Orbio.Services.Admin.Sales;
using Orbio.Web.UI.Areas.Admin.Models.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Configuration;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class SalesController : Controller
    {
        private readonly IGiftCardService _giftCardService;

        public SalesController(IGiftCardService _giftCardService)
        {
            this._giftCardService = _giftCardService;
        }

        // GET: Admin/Sales
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GiftCards()
        {
            return View();
        }

        public ActionResult SearchGiftCards(GiftCardSearchModel model)
        {
            model.Activated.Insert(0, new SelectListItem() { Text = "All", Value = "0" });
            model.Activated.Insert(1, new SelectListItem() { Text = "Activated", Value = "1" });
            model.Activated.Insert(2, new SelectListItem() { Text = "DeActivated", Value = "2" });

            return PartialView(model);
        }

        public ActionResult ListGiftCards(GiftCardSearchModel model,int? page)
        {
            var result = _giftCardService.GetAllGiftCards(model.IsActive,model.GiftCardCode);
            var giftCardModel = new List<GiftCardModel>();
            giftCardModel = (from g in result
                             select new GiftCardModel(g)).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            int pageNumber = (page ?? 1);
            return PartialView(giftCardModel.ToPagedList(pageNumber, pageSize));                
        }

        public ActionResult DeleteGiftCard(int? Id)
        {
            var result = _giftCardService.DeleteGiftCards(Id.GetValueOrDefault());
            return RedirectToAction("GiftCards");
        }

        public ActionResult AddGiftCard()
        {
            var model = new GiftCardModel();
            model.GiftCardTypes.Insert(0, new SelectListItem() { Text = "Virtual", Value = "0" });
            model.GiftCardTypes.Insert(1, new SelectListItem() { Text = "Physical", Value = "1" });
            return View("AddOrEditGiftCards", model);
        }

        public ActionResult EditGiftCard(int? Id)
        {
            var model = new GiftCardModel();
            var result = _giftCardService.GetGiftCardById(Id.GetValueOrDefault());
            if(result!=null)
            {
                model = new GiftCardModel(result);
            }
            model.GiftCardTypes=new List<SelectListItem>();
            model.GiftCardTypes.Add(new SelectListItem() { Text = "Virtual", Value = "0" });
            model.GiftCardTypes.Add(new SelectListItem() { Text = "Physical", Value = "1" });
            return View("AddOrEditGiftCards", model);
        }

        public ActionResult AddOrUpdateGiftCard(GiftCardModel model)
        {
            var giftCard = new GiftCard();
            giftCard =  new GiftCard()
                        {
                            Id=model.Id,
                            GiftCardTypeId=model.GiftCardType,
                            Amount=model.InitialValue,
                            IsGiftCardActivated=model.IsGiftCardActivated,
                            GiftCardCouponCode=model.CouponCode,
                            RecipientName=model.RecipientName,
                            RecipientEmail=model.RecipientEmail,
                            SenderName=model.SenderName,
                            SenderEmail=model.SenderEmail,
                            Message=model.Message,
                            IsRecipientNotified=model.IsRecipientNotified,
                            CreatedOnUtc=DateTime.UtcNow,
                        };
            var result = _giftCardService.AddOrUpdateGiftCards(giftCard);
            return RedirectToAction("GiftCards");
        }
    }
}