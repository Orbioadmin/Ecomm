﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orbio.Web.UI.Filters;
using Nop.Core.Infrastructure;
using Orbio.Services.Customers;
using Orbio.Services.Checkout;
using Orbio.Web.UI.Models.CheckOut;
using Orbio.Core.Domain.Customers;
using System.Globalization;

namespace Orbio.Web.UI.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ICheckoutService checkoutService;
        //
        // GET: /CheckOut/
        public CheckOutController(ICheckoutService checkoutService)
        {
            this.checkoutService = checkoutService;
        }

        [LoginRequired]
        public ActionResult Index()
        {
            return View();

        }

         [LoginRequired]
        public ActionResult Address()
        {
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var customer = workContext.CurrentCustomer;
            if (customer.Email == null)
            {
                return RedirectToAction("MyAccount", "Login");
            }
            else
            {
                var customerBillAddress = checkoutService.GetCustomerAddress(customer.Email, "Billing");
                var customerShipAddress = checkoutService.GetCustomerAddress(customer.Email, "Shipping");
                if (customerBillAddress == null && customerShipAddress==null)
                {
                    return RedirectToAction("Index", "CheckOut");
                }
                else
                {
                    var address = new AddressModel();
                    if (customerBillAddress.BillingAddress_Id == customerBillAddress.ShippingAddress_Id)
                    {
                        address = new AddressModel(customerBillAddress);
                    }
                    else
                    {
                        if(customerShipAddress==null)
                        {
                            address = new AddressModel(customerBillAddress, "Billing");
                          
                        }
                        else if(customerBillAddress==null)
                        {
                           address = new AddressModel(customerBillAddress, "Shipping");
                        }
                        else
                        {
                            address = new AddressModel(customerBillAddress, customerShipAddress);
                        }
                    }
                    return View("~/Views/CheckOut/Index.cshtml", address);
                }
            }
        }
       
        [HttpPost]
        public ActionResult CheckboxDetails(AddressModel model)
        {
           if(model.SameAddress)
           {
               model.BillFirstName = model.ShipFirstName;
               model.BillLastName = model.ShipLastName;
               model.BillPhone = model.ShipPhone;
               model.BillAddress = model.ShipAddress;
               model.BillCity = model.ShipCity;
               model.BillPincode = model.ShipPincode;
               model.BillState = model.ShipState;
               model.BillCountry = model.ShipCountry;
               return Json(new { model });
           }
           else
           {
               var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
               var customer = workContext.CurrentCustomer;
               var customerBillAddress = checkoutService.GetCustomerAddress(customer.Email, "Billing");
               if (customerBillAddress == null)
               {
                   model.BillFirstName = null;
                   model.BillLastName = null;
                   model.BillPhone = null;
                   model.BillAddress = null;
                   model.BillCity = null;
                   model.BillPincode = null;
                   model.BillState = null;
                   model.BillCountry = null;
               }
               else
               {
                   var address = new AddressModel(customerBillAddress, "Billing");
                   model.BillFirstName = address.BillFirstName;
                   model.BillLastName = address.BillLastName;
                   model.BillPhone = address.BillPhone;
                   model.BillAddress = address.BillAddress;
                   model.BillCity = address.BillCity;
                   model.BillPincode = address.BillPincode;
                   model.BillState = address.BillState;
                   model.BillCountry = address.BillCountry;
               }

               return Json(new { model });
           }
        }

        [HttpPost]
        [LoginRequired]
        public ActionResult UpdateCustomer(AddressModel model)
        {
            bool sameaddress=false;
            var workContext = EngineContext.Current.Resolve<Orbio.Core.IWorkContext>();
            var customer = workContext.CurrentCustomer;
            if (model.SameAddress)
            {
                sameaddress = true;
            }
            checkoutService.UpdateCustomerAddress(customer.Email, sameaddress, model.BillFirstName, model.BillLastName, model.BillPhone, model.BillAddress,
            model.BillCity,model.BillPincode, model.BillState, model.BillCountry, model.ShipFirstName, model.ShipLastName,
            model.ShipPhone, model.ShipAddress, model.ShipCity, model.ShipPincode,model.ShipState, model.ShipCountry);

            return Json("Success");
        }
    }
}