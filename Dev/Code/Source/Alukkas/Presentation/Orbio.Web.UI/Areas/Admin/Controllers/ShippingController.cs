using Orbio.Services.Admin.Shipments;
using Orbio.Services.Admin.Shipping;
using Orbio.Web.UI.Areas.Admin.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ShippingController : Controller
    {
        private readonly IWarehouseService warehouseService;
        private readonly IShipmentService _shipmentService;
        private readonly IDeliveryDateService deliveryService;
        private readonly IShippingMethodService shippingMethodService;

        public ShippingController(IWarehouseService warehouseService, IShipmentService _shipmentService, IDeliveryDateService deliveryService,
            IShippingMethodService shippingMethodService)
        {
            this.warehouseService = warehouseService;
            this._shipmentService = _shipmentService;
            this.deliveryService = deliveryService;
            this.shippingMethodService = shippingMethodService;
        }
        // GET: Admin/Shipping
        public ActionResult Index()
        {
            return View();
        }

        #region Warehouses
        public ActionResult Warehouses()
        {
            return View();
        }

        public ActionResult ListWarehouse()
        {
            var result = warehouseService.GetAllWarehouseDetails();
            var model = (from w in result
                         select new WarehouseModel(w)).ToList();
            return PartialView(model);
        }

        public ActionResult EditWarehouse(int? Id)
        {
            var result = warehouseService.Edit(Id.GetValueOrDefault());
            var model = new WarehouseModel(result);
            model = GetAllCountiesAndStates(model);
            return View("AddOrEditWarehouse", model);
        }

        public ActionResult AddWarehouse()
        {
            var model = new WarehouseModel();
            model = GetAllCountiesAndStates(model);
            return View("AddOrEditWarehouse", model);
        }

        public WarehouseModel GetAllCountiesAndStates(WarehouseModel model)
        {
            var country = _shipmentService.GetAllCountries();
            model.WarehouseDetails.AvailableCountries = (from c in country
                                                         select new SelectListItem()
                                                         {
                                                             Text = c.Name,
                                                             Value = c.Id.ToString(),
                                                         }).ToList();
            model.WarehouseDetails.AvailableStates = (from c in country
                                                      from s in c.StateProvinces
                                                      select new SelectListItem()
                                                      {
                                                          Text = s.Name,
                                                          Value = s.Id.ToString(),
                                                      }).Distinct().ToList();

            model.WarehouseDetails.AvailableCountries.Insert(0, new SelectListItem() { Text = "Country", Value = "0" });
            model.WarehouseDetails.AvailableStates.Insert(0, new SelectListItem() { Text = "State", Value = "0" });
            return model;
        }

        [HttpPost]
        public ActionResult AddOrUpdateWarehouse(WarehouseModel model)
        {
            var result = warehouseService.AddOrUpdate(model.Id, model.Name, model.AddressId, model.WarehouseDetails.CountryId, model.WarehouseDetails.StateProvinceId, model.WarehouseDetails.City, model.WarehouseDetails.Address, model.WarehouseDetails.Pincode);
            return RedirectToAction("Warehouses");
        }

        public ActionResult DeleteWarehouse(int? Id)
        {
            var result = warehouseService.Delete(Id.GetValueOrDefault());
            return RedirectToAction("Warehouses");
        }
        #endregion

        #region Delivery Date

        public ActionResult DeliveryDate()
        {
            return View();
        }

        public ActionResult ListDeliveryDate()
        {
            var result = deliveryService.GetAllDeliveryDate();
            var model = (from dd in result
                         select new DeliveryDateModel()
                         {
                             Id=dd.Id,
                             Name=dd.Name,
                             DisplayOrder=dd.DisplayOrder,
                         }).ToList();

            return PartialView(model);
        }

        public ActionResult AddDeliveryDate()
        {
            var model = new DeliveryDateModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AddDeliveryDate(DeliveryDateModel model)
        {
            var result = deliveryService.AddOrUpdate(model.Id,model.Name,model.DisplayOrder);
            return RedirectToAction("DeliveryDate");
        }

        [HttpPost]
        public ActionResult UpdateDeliveryDate(int Id,FormCollection form)
        {
            if(form!=null)
            {
                var name = form["txtname" + Id];
                int displayOrder = (form["txtdisplayorder" + Id] != null) ? Convert.ToInt32(form["txtdisplayorder" + Id]) : 0;
                var result = deliveryService.AddOrUpdate(Id, name, displayOrder);
            }
            return RedirectToAction("DeliveryDate");
        }

        public ActionResult DeleteDeliveryDate(int? Id)
        {
           deliveryService.Delete(Id.GetValueOrDefault());
           return RedirectToAction("DeliveryDate");
        }

        #endregion


        #region Shipping Methods

        public ActionResult ShippingMethods()
        {
            return View();
        }

        /// <summary>
        /// getting all shipping methods and passing to view
        /// </summary>
        /// <returns></returns>
        public ActionResult ListShippingMethods()
        {
            var result = shippingMethodService.GetAllShippingMethods();
            var model = (from sm in result
                         select new ShippingMethodModel(sm)).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// adding a new shipping method
        /// </summary>
        /// <returns></returns>
        public ActionResult AddShippingMethod()
        {
            var model = new ShippingMethodModel();
            return PartialView(model);
        }

        /// <summary>
        /// updating shipping method
        /// </summary>
        /// <param name="Id">shipping method id</param>
        /// <param name="form">form collection</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateShippingMethod(int Id, FormCollection form)
        {
            if(form!=null)
            {
                var name = form["txtname" + Id];
                var description = form["txtdescription" + Id];
                var displayOrder = Convert.ToInt32(form["txtdisplayOrder" + Id]);

                shippingMethodService.AddOrUpdate(Id, name, description, displayOrder);
            }
            return RedirectToAction("ShippingMethods");
        }

        /// <summary>
        /// adding new shipping method
        /// </summary>
        /// <param name="model">shipping method model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddShippingMethod(ShippingMethodModel model)
        {
            shippingMethodService.AddOrUpdate(model.Id, model.Name, model.Description,model.DisplayOrder);
            return RedirectToAction("ShippingMethods");
        }

        public ActionResult DeleteShippingMethod(int Id)
        {
            shippingMethodService.Delete(Id);
            return RedirectToAction("ShippingMethods");
        }

        #endregion
    }
}