﻿using Nop.Core.Infrastructure;
using Orbio.Services.Admin.Components;
using Orbio.Web.UI.Areas.Admin.Models.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class ComponentController : Controller
    {

         private readonly IProductComponentService productComponentService;

         private readonly IPriceComponentService priceComponentService;

         public ComponentController(IProductComponentService productComponentService, IPriceComponentService priceComponentService)
        {
            this.productComponentService = productComponentService;
            this.priceComponentService = priceComponentService;
        }
        // GET: Admin/Component
        public ActionResult Index()
        {
            return View();
        }

        #region Product Components

        public ActionResult ProductComponent()
        {
            var result = productComponentService.GetProductComponent();
            var model = (from PC in result 
                          select new ProductComponentModel(PC)).ToList();
            return View("ListProductComponent", model);
        }

        public ActionResult AddProductComponent()
        {
            var model = new ProductComponentModel();
            return View("AddOrEditProductComponent", model);
        }

        public ActionResult EditProductComponent(int Id)
        {
            var result = productComponentService.GetProductComponentById(Id);
            var model = new ProductComponentModel(result);
            return View("AddOrEditProductComponent", model);
        }

        public ActionResult AddOrUpdateProductComponent(ProductComponentModel model)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            int result = productComponentService.AddOrUpdateProductComponent(model.Id,model.Name,model.IsActive,curCustomer.Email);
            return RedirectToAction("ProductComponent");
        }

        public ActionResult DeleteProductComponent(int Id)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var result = productComponentService.DeleteProductComponent(Id, curCustomer.Email);
            return RedirectToAction("ProductComponent");
        }

        #endregion


        #region Price Components

        public ActionResult PriceComponent()
        {
            var result = priceComponentService.GetPriceComponent();
            var model = (from PC in result
                         select new PriceComponentModel(PC)).ToList();
            return View("ListPriceComponent", model);
        }

        public ActionResult AddPriceComponent()
        {
            var model = new PriceComponentModel();
            return View("AddOrEditPriceComponent", model);
        }

        public ActionResult EditPriceComponent(int Id)
        {
            var result = priceComponentService.GetPriceComponentById(Id);
            var model = new PriceComponentModel(result);
            return View("AddOrEditPriceComponent", model);
        }

        public ActionResult AddOrUpdatePriceComponent(PriceComponentModel model)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            int result = priceComponentService.AddOrUpdatePriceComponent(model.Id, model.Name, model.IsActive, curCustomer.Email);
            return RedirectToAction("PriceComponent");
        }

        public ActionResult DeletePriceComponent(int Id)
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var curCustomer = workContext.CurrentCustomer;
            var result = priceComponentService.DeletePriceComponent(Id,curCustomer.Email);
            return RedirectToAction("PriceComponent");
        }

        #endregion
    }
}