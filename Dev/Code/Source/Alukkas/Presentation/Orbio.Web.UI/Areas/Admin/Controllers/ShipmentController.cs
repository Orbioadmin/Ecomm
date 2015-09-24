using Orbio.Services.Admin.Shipments;
using Orbio.Services.Common;
using Orbio.Services.Helpers;
using Orbio.Web.UI.Areas.Admin.Models.Shipments;
using Orbio.Web.UI.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ShipmentController : Controller
    {
        #region Fields
        private readonly IShipmentService _shipmentService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPdfService _pdfService;
        #endregion

        #region Ctor
        public ShipmentController(IShipmentService _shipmentService, IDateTimeHelper _dateTimeHelper, IPdfService _pdfService)
        {
            this._shipmentService = _shipmentService;
            this._dateTimeHelper = _dateTimeHelper;
            this._pdfService = _pdfService;
        }
        #endregion

        // GET: Admin/Shipment
        public ActionResult Index()
        {
            var model = new List<ShipmentModel>();
            return View(model);
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Search()
        {
            var model = new ShipmentListModel();
            var result = _shipmentService.GetAllCountries();
            model.AvailableCountries = (from c in result
                                        select new SelectListItem()
                                        {
                                            Text = c.Name,
                                            Value = c.Id.ToString(),
                                        }).ToList();
            model.AvailableStates=(from c in result
                                       from s in c.StateProvinces
                                   select new SelectListItem()
                                   {
                                       Text = s.Name,
                                       Value = s.Id.ToString(),
                                   }).ToList();

            model.AvailableCountries.Insert(0, new SelectListItem() { Text = "Country", Value = "0" });
            model.AvailableStates.Insert(0, new SelectListItem() { Text = "State", Value = "0" });
            return PartialView(model);
        }

        public ActionResult ShipmentList(ShipmentListModel model)
        {
            var shipmentModel = new List<ShipmentModel>();
            DateTime? startDateValue = (model.StartDate == null) ? null
            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            var result = _shipmentService.GetAllShipmentDetails(startDateValue,endDateValue,model.TrackingNumber,model.CountryId,model.StateProvinceId);
            shipmentModel = (from s in result
                             select new ShipmentModel()
                             {
                                 Id = s.Id,
                                 OrderId = s.OrderId,
                                 TrackingNumber = s.TrackingNumber,
                                 TotalWeight = s.TotalWeight,
                                 ShippedDateUtc = s.ShippedDateUtc,
                                 DeliveryDateUtc = s.DeliveryDateUtc,
                                 ShipmentMethod=s.Order.ShippingMethod,
                                 ShipmentItems = (from oi in s.Order.OrderItems
                                                  select new ShipmentItemModel()
                                                  {
                                                  ProductName=oi.Product.Name,
                                                  ProductId=oi.ProductId,
                                                  QuantityInThisShipment=oi.Quantity,
                                                  ItemWeight=oi.ItemWeight,
                                                  ItemDimensions = string.Format("{0:F2} x {1:F2} x {2:F2} [{3}]", oi.Product.Length, oi.Product.Width, oi.Product.Height, "inch(es)"),
                                                  ShipmentId=s.Id,
                                                  }).ToList(),
                             }).ToList();
            return PartialView(shipmentModel);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrintPackagingSlips(int[] ShipmentIds)
        {
            var result = _shipmentService.GetAllShipmentDetails(null, null, null, 0, 0);
            result = result.Where(c => ShipmentIds.Contains(c.Id)).ToList();
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintPackagingSlipsToPdf(stream, result);
                bytes = stream.ToArray();
            }
            return File(bytes, "application/pdf", "packagingslips.pdf");
            //return RedirectToAction("List");
        }

        public ActionResult DeleteShipment(int? Id)
        {
            var result = _shipmentService.DeleteShipment(Id.GetValueOrDefault());
            return RedirectToAction("List");
        }
    }
}