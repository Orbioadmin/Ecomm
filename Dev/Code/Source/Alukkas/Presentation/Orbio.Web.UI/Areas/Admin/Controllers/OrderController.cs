using Orbio.Core.Domain.Admin.Orders;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Payments;
using Orbio.Services.Admin;
using Orbio.Services.Admin.Orders;
using Orbio.Services.Orders;
using Orbio.Services.Payments;
using Orbio.Web.UI.Areas.Admin.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using Orbio.Web.Framework;
using System.Web;
using System.Web.Mvc;
using Orbio.Services.Helpers;
using Orbio.Web.UI.Area.Admin.Models.CheckOut;
using Orbio.Services.Admin.Address;
using Orbio.Services.Directory;
using Orbio.Core.Domain.Directory;
using Orbio.Services.Messages;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        #region Fields
        private readonly Orbio.Services.Admin.Orders.IOrderService _orderService;
        private readonly IOrderReportService _orderReportService;
        private readonly IAddressService _addressService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IMessageService _messageService;
        #endregion

        #region Ctor

        public OrderController(Orbio.Services.Admin.Orders.IOrderService orderService, IOrderReportService orderReportService, IAddressService addressService, IDateTimeHelper dateTimeHelper, ICountryService CountryService, IStateProvinceService stateProvinceService, IOrderProcessingService orderProcessingService,IMessageService messageService)
        {
            this._orderService = orderService;
            this._addressService = addressService;
            this._orderReportService = orderReportService;
            this._dateTimeHelper = dateTimeHelper;
            this._countryService = CountryService;
            this._stateProvinceService = stateProvinceService;
            this._orderProcessingService = orderProcessingService;
            this._messageService = messageService;
        }

        #endregion

        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }

        protected virtual IList<OrderAverageReportLineSummaryModel> GetOrderAverageReportModel()
        {
            var report = new List<OrderAverageReportLineSummary>();
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Pending));
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Processing));
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Complete));
            report.Add(_orderReportService.OrderAverageReport(1, OrderStatus.Cancelled));
            var model = report.Select(x =>
            {
                return new OrderAverageReportLineSummaryModel()
                {
                    OrderStatus = x.OrderStatus.ToString(),
                    CountTodayOrders = x.CountTodayOrders,
                    SumTodayOrders = x.SumTodayOrders.ToString("#,##0.00"), 
                    CountThisWeekOrders = x.CountThisWeekOrders,
                    SumThisWeekOrders = x.SumThisWeekOrders.ToString("#,##0.00"),
                    CountThisMonthOrders = x.CountThisMonthOrders,
                    SumThisMonthOrders = x.SumThisMonthOrders.ToString("#,##0.00"),
                    CountThisYearOrders = x.CountThisYearOrders,
                    SumThisYearOrders = x.SumThisYearOrders.ToString("#,##0.00"),
                    CountAllTimeOrders = x.CountAllTimeOrders,
                    SumAllTimeOrders = x.SumAllTimeOrders.ToString("#,##0.00"),
                };
            }).ToList();

            return model;
        }

        [ChildActionOnly]
        public ActionResult OrderAverageReport()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
            //    return Content("");

            var model = GetOrderAverageReportModel();
            return PartialView(model);
       }

        protected virtual IList<OrderIncompleteReportLineModel> GetOrderIncompleteReportModel()
        {
            var model = new List<OrderIncompleteReportLineModel>();
            //not paid
            var psPending = _orderReportService.GetOrderAverageReportLine(0, 0, null, PaymentStatus.Pending, null, null, null, null, true);
            model.Add(new OrderIncompleteReportLineModel()
            {
                Item = "Total unpaid order (pending payment status)",//_localizationService.GetResource("Admin.SalesReport.Incomplete.TotalUnpaidOrders"),
                Count = psPending.CountOrders,
                Total = psPending.SumOrders.ToString("#,##0.00")
            });
            //not shipped
            var ssPending = _orderReportService.GetOrderAverageReportLine(0, 0, null, null, ShippingStatus.NotYetShipped, null, null, null, true);
            model.Add(new OrderIncompleteReportLineModel()
            {
                Item = "Total not yet shipped orders",//_localizationService.GetResource("Admin.SalesReport.Incomplete.TotalNotShippedOrders"),
                Count = ssPending.CountOrders,
                Total = ssPending.SumOrders.ToString("#,##0.00")
            });
            //pending
            var osPending = _orderReportService.GetOrderAverageReportLine(0, 0, OrderStatus.Pending, null, null, null, null, null, true);
            model.Add(new OrderIncompleteReportLineModel()
            {
                Item = "Total incomplete orders (pending payment status)",//_localizationService.GetResource("Admin.SalesReport.Incomplete.TotalIncompleteOrders"),
                Count = osPending.CountOrders,
                Total = osPending.SumOrders.ToString("#,##0.00")
            });
            return model;
        }

        [ChildActionOnly]
        public ActionResult OrderIncompleteReport()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders))
            //    return Content("");

            var model = GetOrderIncompleteReportModel();
            return PartialView(model);
        }

        #region OrderList

        public ActionResult List()
        {
            //order statuses
            var model = new OrderListModel();
            model.AvailableOrderStatuses = OrderStatus.Pending.ToSelectList(false).ToList();
            model.AvailableOrderStatuses.Insert(0, new SelectListItem() { Text = "ALL", Value = "0" });

            //payment statuses
            model.AvailablePaymentStatuses = PaymentStatus.Pending.ToSelectList(false).ToList();
            model.AvailablePaymentStatuses.Insert(0, new SelectListItem() { Text = "ALL", Value = "0" });

            //shipping statuses
            model.AvailableShippingStatuses = ShippingStatus.NotYetShipped.ToSelectList(false).ToList();
            model.AvailableShippingStatuses.Insert(0, new SelectListItem() { Text = "ALL", Value = "0" });

            return View(model);
        }

        public virtual IList<OrderModel> GetAllOrderDetails(OrderListModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
                          : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            PaymentStatus? paymentStatus = model.PaymentStatusId > 0 ? (PaymentStatus?)(model.PaymentStatusId) : null;
            ShippingStatus? shippingStatus = model.ShippingStatusId > 0 ? (ShippingStatus?)(model.ShippingStatusId) : null;

            var order = _orderService.SearchOrders(0, startDateValue, endDateValue, orderStatus,
                paymentStatus, shippingStatus, model.CustomerEmail, model.GoDirectlyToNumber);
            var orderModel = new List<OrderModel>();
            orderModel = order.Select(x =>
            {
                return new OrderModel()
                {
                    Id = x.OrderId,
                    OrderTotal = x.OrderTotal.ToString("#,##0.00"),
                    OrderStatus = x.OrderStatus.ToString(),
                    PaymentStatus = x.PaymentStatus.ToString(),
                    ShippingStatus = x.ShippingStatus.ToString(),
                    CustomerEmail = x.Customer.Email,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                };
            }).ToList();

            return orderModel;
        }
        [ChildActionOnly]
        public ActionResult OrderList(OrderListModel model)
        {
            var resultModel = GetAllOrderDetails(model);
            return PartialView(resultModel);
        }

        [ChildActionOnly]
        public ActionResult OrderSummary(OrderListModel model)
        {
            DateTime? startDateValue = (model.StartDate == null) ? null
              : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.StartDate.Value, _dateTimeHelper.CurrentTimeZone);

            DateTime? endDateValue = (model.EndDate == null) ? null
                            : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.EndDate.Value, _dateTimeHelper.CurrentTimeZone).AddDays(1);

            OrderStatus? orderStatus = model.OrderStatusId > 0 ? (OrderStatus?)(model.OrderStatusId) : null;
            PaymentStatus? paymentStatus = model.PaymentStatusId > 0 ? (PaymentStatus?)(model.PaymentStatusId) : null;
            ShippingStatus? shippingStatus = model.ShippingStatusId > 0 ? (ShippingStatus?)(model.ShippingStatusId) : null;

            var reportSummary = _orderReportService.GetOrderAverageReportLine(0, 0, orderStatus, paymentStatus, shippingStatus,
                               startDateValue, endDateValue, model.CustomerEmail);
            var profit = _orderReportService.ProfitReport(0, 0, orderStatus, paymentStatus, shippingStatus,
                startDateValue, endDateValue, model.CustomerEmail);
            var aggregator = new OrderModel()
            {
                aggregatorprofit = profit.ToString("#,##0.00"),
                aggregatorshipping = reportSummary.SumShippingExclTax.ToString("#,##0.00"),
                aggregatortax = reportSummary.SumTax.ToString("#,##0.00"),
                aggregatortotal = reportSummary.SumOrders.ToString("#,##0.00")
            };
            return PartialView(aggregator);
        }

        public ActionResult Edit(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null || order.Deleted)
                //No order found with the specified id
                return RedirectToAction("List");

            var model = new OrderModel();
            PrepareOrderDetailsModel(model, order);

            return View(model);
        }

        public ActionResult AddressEdit(int addressId, int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            var address = _addressService.GetAddressDetailsById(addressId);
            if (address == null)
                throw new ArgumentException("No address found with the specified id", "addressId");

            var model = new AddressModel();
            model.OrderId = orderId;
            model = address.ToModel();
            //countries
            model.AvailableCountries.Add(new SelectListItem() { Text = "Select Country", Value = "0" });
            foreach (var c in _countryService.GetAllCountries())
                model.AvailableCountries.Add(new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = (c.Id == address.CountryId) });
            //states
            //var states = address.Country != null ? _stateProvinceService.GetStateProvincesByCountryId(address.CountryId).ToList() : new List<StateProvince>();
            var states = _stateProvinceService.GetStateProvincesByCountryId(address.CountryId).ToList();
            if (states.Count > 0)
            {
                foreach (var s in states)
                    model.AvailableStates.Add(new SelectListItem() { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == address.StateProvinceId) });
            }
            else
                model.AvailableStates.Add(new SelectListItem() { Text = "Others", Value = "0" });

            return View(model);
        }

        [HttpPost]
        public ActionResult AddressEdit(AddressModel model)
        {
            //if (ModelState.IsValid)
            //{
            _addressService.UpdateAddress(model.Id, model.FirstName, model.LastName, model.Address, model.Phone, model.City, model.Pincode,model.StateProvinceId, model.CountryId);
            //}
            return RedirectToAction("Edit", "Order", new { id = model.OrderId });
        }

        protected void PrepareOrderDetailsModel(OrderModel model, Orbio.Core.Domain.Orders.Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (model == null)
                throw new ArgumentNullException("model");

            model.Id = order.OrderId;
            model.OrderStatus = order.OrderStatus.ToString();
            model.OrderStatusId = order.OrderStatusId;
            model.OrderGuid = order.OrderGuid;
            model.CustomerId = order.CustomerId;
            var customer = order.Customer;
            model.CustomerInfo = customer.Email;
            model.CustomerIp = order.CustomerIp;
            model.VatNumber = order.VatNumber;
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc);
            //model.AllowCustomersToSelectTaxDisplayType = _taxSettings.AllowCustomersToSelectTaxDisplayType;
            //model.TaxDisplayType = _taxSettings.TaxDisplayType;
            model.AffiliateId = order.AffiliateId;
            //a vendor should have access only to his products
            //model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            #region Order totals

            //var primaryStoreCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
            //if (primaryStoreCurrency == null)
            //    throw new Exception("Cannot load primary store currency");

            //subtotal
            model.OrderSubtotalInclTax = order.OrderSubtotalInclTax.ToString("#,##0.00");
            model.OrderSubtotalExclTax = order.OrderSubtotalExclTax.ToString("#,##0.00");
            model.OrderSubtotalInclTaxValue = order.OrderSubtotalInclTax;
            model.OrderSubtotalExclTaxValue = order.OrderSubtotalExclTax;
            //discount (applied to order subtotal)
            string orderSubtotalDiscountInclTaxStr = order.OrderSubTotalDiscountInclTax.ToString("#,##0.00");
            string orderSubtotalDiscountExclTaxStr = order.OrderSubTotalDiscountExclTax.ToString("#,##0.00");
            if (order.OrderSubTotalDiscountInclTax > decimal.Zero)
                model.OrderSubTotalDiscountInclTax = orderSubtotalDiscountInclTaxStr;
            if (order.OrderSubTotalDiscountExclTax > decimal.Zero)
                model.OrderSubTotalDiscountExclTax = orderSubtotalDiscountExclTaxStr;
            model.OrderSubTotalDiscountInclTaxValue = order.OrderSubTotalDiscountInclTax;
            model.OrderSubTotalDiscountExclTaxValue = order.OrderSubTotalDiscountExclTax;

            //shipping
            model.OrderShippingInclTax = order.OrderShippingInclTax.ToString("#,##0.00");
            model.OrderShippingExclTax = order.OrderShippingExclTax.ToString("#,##0.00");
            model.OrderShippingInclTaxValue = order.OrderShippingInclTax;
            model.OrderShippingExclTaxValue = order.OrderShippingExclTax;

            //payment method additional fee
            if (order.PaymentMethodAdditionalFeeInclTax > decimal.Zero)
            {
                model.PaymentMethodAdditionalFeeInclTax = order.PaymentMethodAdditionalFeeInclTax.ToString("#,##0.00");
                model.PaymentMethodAdditionalFeeExclTax = order.PaymentMethodAdditionalFeeExclTax.ToString("#,##0.00");
            }
            model.PaymentMethodAdditionalFeeInclTaxValue = order.PaymentMethodAdditionalFeeInclTax;
            model.PaymentMethodAdditionalFeeExclTaxValue = order.PaymentMethodAdditionalFeeExclTax;


            //tax
            model.Tax = order.OrderTax.ToString("#,##0.00");
            //SortedDictionary<decimal, decimal> taxRates = order.TaxRatesDictionary;
            //bool displayTaxRates = _taxSettings.DisplayTaxRates && taxRates.Count > 0;
            //bool displayTax = !displayTaxRates;
            //foreach (var tr in order.TaxRatesDictionary)
            //{
            //    model.TaxRates.Add(new OrderModel.TaxRate()
            //    {
            //        Rate = _priceFormatter.FormatTaxRate(tr.Key),
            //        Value = _priceFormatter.FormatPrice(tr.Value, true, false),
            //    });
            //}
            //model.DisplayTaxRates = displayTaxRates;
            //model.DisplayTax = displayTax;
            model.TaxValue = order.OrderTax;
            model.TaxRatesValue = order.TaxRates;

            //discount
            if (order.OrderDiscount > 0)
                model.OrderTotalDiscount = order.OrderDiscount.ToString("#,##0.00");
            model.OrderTotalDiscountValue = order.OrderDiscount;

            ////gift cards
            //foreach (var gcuh in order.GiftCardUsageHistory)
            //{
            //    model.GiftCards.Add(new OrderModel.GiftCard()
            //    {
            //        CouponCode = gcuh.GiftCard.GiftCardCouponCode,
            //        Amount = _priceFormatter.FormatPrice(-gcuh.UsedValue, true, false),
            //    });
            //}

            ////reward points
            //if (order.RedeemedRewardPointsEntry != null)
            //{
            //    model.RedeemedRewardPoints = -order.RedeemedRewardPointsEntry.Points;
            //    model.RedeemedRewardPointsAmount = _priceFormatter.FormatPrice(-order.RedeemedRewardPointsEntry.UsedAmount, true, false);
            //}

            //total
            model.OrderTotal = order.OrderTotal.ToString("#,##0.00");
            model.OrderTotalValue = order.OrderTotal;

            //refunded amount
            if (order.RefundedAmount > decimal.Zero)
                model.RefundedAmount = order.RefundedAmount.ToString("#,##0.00");

            ////used discounts
            //var duh = _discountService.GetAllDiscountUsageHistory(null, null, order.Id, 0, int.MaxValue);
            //foreach (var d in duh)
            //{
            //    model.UsedDiscounts.Add(new OrderModel.UsedDiscountModel()
            //    {
            //        DiscountId = d.DiscountId,
            //        DiscountName = d.Discount.Name
            //    });
            //}

            //#endregion

            //#region Payment info

            //if (order.AllowStoringCreditCardNumber)
            //{
            //    //card type
            //    model.CardType = _encryptionService.DecryptText(order.CardType);
            //    //cardholder name
            //    model.CardName = _encryptionService.DecryptText(order.CardName);
            //    //card number
            //    model.CardNumber = _encryptionService.DecryptText(order.CardNumber);
            //    //cvv
            //    model.CardCvv2 = _encryptionService.DecryptText(order.CardCvv2);
            //    //expiry date
            //    string cardExpirationMonthDecrypted = _encryptionService.DecryptText(order.CardExpirationMonth);
            //    if (!String.IsNullOrEmpty(cardExpirationMonthDecrypted) && cardExpirationMonthDecrypted != "0")
            //        model.CardExpirationMonth = cardExpirationMonthDecrypted;
            //    string cardExpirationYearDecrypted = _encryptionService.DecryptText(order.CardExpirationYear);
            //    if (!String.IsNullOrEmpty(cardExpirationYearDecrypted) && cardExpirationYearDecrypted != "0")
            //        model.CardExpirationYear = cardExpirationYearDecrypted;

            //    model.AllowStoringCreditCardNumber = true;
            //}
            //else
            //{
            //    string maskedCreditCardNumberDecrypted = _encryptionService.DecryptText(order.MaskedCreditCardNumber);
            //    if (!String.IsNullOrEmpty(maskedCreditCardNumberDecrypted))
            //        model.CardNumber = maskedCreditCardNumberDecrypted;
            //}


            ////purchase order number (we have to find a better to inject this information because it's related to a certain plugin)
            //var pm = _paymentService.LoadPaymentMethodBySystemName(order.PaymentMethodSystemName);
            //if (pm != null && pm.PluginDescriptor.SystemName.Equals("Payments.PurchaseOrder", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    model.DisplayPurchaseOrderNumber = true;
            //    model.PurchaseOrderNumber = order.PurchaseOrderNumber;
            //}

            //payment transaction info
            model.AuthorizationTransactionId = order.AuthorizationTransactionId;
            model.CaptureTransactionId = order.CaptureTransactionId;
            model.SubscriptionTransactionId = order.SubscriptionTransactionId;

            ////payment method info
            model.PaymentMethod = order.PaymentMethodSystemName;
            model.PaymentStatus = order.PaymentStatus.ToString();

            ////payment method buttons
            //model.CanCancelOrder = _orderProcessingService.CanCancelOrder(order);
            //model.CanCapture = _orderProcessingService.CanCapture(order);
            //model.CanMarkOrderAsPaid = _orderProcessingService.CanMarkOrderAsPaid(order);
            //model.CanRefund = _orderProcessingService.CanRefund(order);
            //model.CanRefundOffline = _orderProcessingService.CanRefundOffline(order);
            //model.CanPartiallyRefund = _orderProcessingService.CanPartiallyRefund(order, decimal.Zero);
            //model.CanPartiallyRefundOffline = _orderProcessingService.CanPartiallyRefundOffline(order, decimal.Zero);
            //model.CanVoid = _orderProcessingService.CanVoid(order);
            //model.CanVoidOffline = _orderProcessingService.CanVoidOffline(order);

            //model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            //model.MaxAmountToRefund = order.OrderTotal - order.RefundedAmount;

            ////recurring payment record
            //var recurringPayment = _orderService.SearchRecurringPayments(0, 0, order.Id, null, 0, int.MaxValue, true).FirstOrDefault();
            //if (recurringPayment != null)
            //{
            //    model.RecurringPaymentId = recurringPayment.Id;
            //}
            #endregion

            #region Billing & shipping info

            model.BillingAddress = order.BillingAddress;


            model.ShippingStatus = order.ShippingStatus.ToString();
            if (order.ShippingStatus != ShippingStatus.ShippingNotRequired)
            {
                model.ShippingAddress = order.ShippingAddress;
            }

            #endregion

            #region Products
            model.CheckoutAttributeInfo = order.CheckoutAttributeDescription;
            bool hasDownloadableItems = false;
            var products = order.OrderItems;
            //a vendor should have access only to his products
            //if (_workContext.CurrentVendor != null)
            //{
            //    products = products
            //        .Where(orderItem => orderItem.Product.VendorId == _workContext.CurrentVendor.Id)
            //        .ToList();
            //}
            foreach (var orderItem in products)
            {

                var orderItemModel = new OrderModel.OrderItemModel()
                {
                    //Id = orderItem.Id,
                    ProductId = orderItem.Product.Id,
                    ProductName = orderItem.Product.Name,
                    //Sku = orderItem.Product.FormatSku(orderItem.AttributesXml, _productAttributeParser),
                    Quantity = orderItem.Quantity,
                    //IsDownload = orderItem.Product.IsDownload,
                    DownloadCount = orderItem.DownloadCount,
                    //DownloadActivationType = orderItem.Product.DownloadActivationType,
                    IsDownloadActivated = orderItem.IsDownloadActivated
                };
                //license file
                //if (orderItem.LicenseDownloadId.HasValue)
                //{
                //    var licenseDownload = _downloadService.GetDownloadById(orderItem.LicenseDownloadId.Value);
                //    if (licenseDownload != null)
                //    {
                //        orderItemModel.LicenseDownloadGuid = licenseDownload.DownloadGuid;
                //    }
                //}
                //    //vendor
                //    var vendor = _vendorService.GetVendorById(orderItem.Product.VendorId);
                //    orderItemModel.VendorName = vendor != null ? vendor.Name : "";

                //unit price
                orderItemModel.UnitPriceInclTaxValue = orderItem.UnitPriceInclTax;
                orderItemModel.UnitPriceExclTaxValue = orderItem.UnitPriceExclTax;
                orderItemModel.UnitPriceInclTax = orderItem.UnitPriceInclTax.ToString("#,##0.00");
                orderItemModel.UnitPriceExclTax = orderItem.UnitPriceExclTax.ToString("#,##0.00");
                //discounts
                orderItemModel.DiscountInclTaxValue = orderItem.DiscountAmountInclTax;
                orderItemModel.DiscountExclTaxValue = orderItem.DiscountAmountExclTax;
                orderItemModel.DiscountInclTax = orderItem.DiscountAmountInclTax.ToString("#,##0.00");
                orderItemModel.DiscountExclTax = orderItem.DiscountAmountExclTax.ToString("#,##0.00");
                //subtotal
                orderItemModel.SubTotalInclTaxValue = orderItem.PriceInclTax;
                orderItemModel.SubTotalExclTaxValue = orderItem.PriceExclTax;
                orderItemModel.SubTotalInclTax = orderItem.PriceInclTax.ToString("#,##0.00");
                orderItemModel.SubTotalExclTax = orderItem.PriceExclTax.ToString("#,##0.00");

                    orderItemModel.AttributeInfo = orderItem.AttributeDescription;
                //    if (orderItem.Product.IsRecurring)
                //        orderItemModel.RecurringInfo = string.Format(_localizationService.GetResource("Admin.Orders.Products.RecurringPeriod"), orderItem.Product.RecurringCycleLength, orderItem.Product.RecurringCyclePeriod.GetLocalizedEnum(_localizationService, _workContext));

                //    //return requests
                //    orderItemModel.ReturnRequestIds = _orderService.SearchReturnRequests(0, 0, orderItem.Id, null, 0, int.MaxValue)
                //        .Select(rr => rr.Id).ToList();
                //    //gift cards
                //    orderItemModel.PurchasedGiftCardIds = _giftCardService.GetGiftCardsByPurchasedWithOrderItemId(orderItem.Id)
                    //        .Select(gc => gc.Id).ToList();

                    model.Items.Add(orderItemModel);
            }
            //model.HasDownloadableProducts = hasDownloadableItems;

            #region Order Notes
            foreach (var orderNote in order.OrderNotes)
            {

                var orderNoteModel = new OrderModel.OrderNote()
                {
                    Id = orderNote.Id,
                    OrderId = orderNote.OrderId,
                    Note = orderNote.Note,
                    CreatedOn = orderNote.CreatedOnUtc
                };

                model.OrderNotes.Add(orderNoteModel);
            }
            #endregion

            #endregion
        }


        #endregion

        #region OrderProcessing

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            try
            {
                _orderProcessingService.CancelOrder(order);

                return RedirectToAction("Edit", "Order", new { id = order.OrderId });
            }
            catch (Exception exc)
            {
                //error
                //ErrorNotification(exc, false);
                return RedirectToAction("Edit", "Order", new { id = order.OrderId });
            }
        }

        [HttpPost]
        public ActionResult MarkOrderAsPaid(int id)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            try
            {
                _orderProcessingService.MarkOrderAsPaid(order);

                return RedirectToAction("Edit", "Order", new { id = order.OrderId });
            }
            catch (Exception exc)
            {
                //error
                //ErrorNotification(exc, false);
                return RedirectToAction("Edit", "Order", new { id = order.OrderId }); 
            }
        }

        [HttpPost]
        public ActionResult ChangeOrderStatus(int id, OrderModel model)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            try
            {
                order.OrderStatusId = model.OrderStatusId;
                _orderService.UpdateOrder(order);
                return RedirectToAction("Edit", "Order", new { id = order.OrderId });
            }
            catch (Exception exc)
            {
                //error
                //ErrorNotification(exc, false);
                return RedirectToAction("Edit", "Order", new { id = order.OrderId });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            _orderProcessingService.DeleteOrder(order);
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult EditOrderTotals(int id, OrderModel model)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            order.OrderSubtotalInclTax = model.OrderSubtotalInclTaxValue;
            order.OrderSubtotalExclTax = model.OrderSubtotalExclTaxValue;
            order.OrderSubTotalDiscountInclTax = model.OrderSubTotalDiscountInclTaxValue;
            order.OrderSubTotalDiscountExclTax = model.OrderSubTotalDiscountExclTaxValue;
            order.OrderShippingInclTax = model.OrderShippingInclTaxValue;
            order.OrderShippingExclTax = model.OrderShippingExclTaxValue;
            order.PaymentMethodAdditionalFeeInclTax = model.PaymentMethodAdditionalFeeInclTaxValue;
            order.PaymentMethodAdditionalFeeExclTax = model.PaymentMethodAdditionalFeeExclTaxValue;
            order.TaxRates = model.TaxRatesValue;
            order.OrderTax = model.TaxValue;
            order.OrderDiscount = model.OrderTotalDiscountValue;
            order.OrderTotal = model.OrderTotalValue;
            _orderService.UpdateOrder(order);

            return RedirectToAction("Edit", "Order", new { id = order.OrderId });
        }

        [HttpPost]
        public ActionResult EditOrderItem(int id, FormCollection form)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            //get order item identifier
            int orderItemId = 0;
            foreach (var formValue in form.AllKeys)
                if (formValue.StartsWith("btnSaveOrderItem", StringComparison.InvariantCultureIgnoreCase))
                    orderItemId = Convert.ToInt32(formValue.Substring("btnSaveOrderItem".Length));

            var orderItem = order.OrderItems.FirstOrDefault(x => x.Product.Id == orderItemId);
            if (orderItem == null)
                throw new ArgumentException("No order item found with the specified id");


            decimal unitPriceInclTax, unitPriceExclTax, discountInclTax, discountExclTax, priceInclTax, priceExclTax;
            int quantity;
            if (!decimal.TryParse(form["pvUnitPriceInclTax" + orderItemId], out unitPriceInclTax))
                unitPriceInclTax = orderItem.UnitPriceInclTax;
            if (!decimal.TryParse(form["pvUnitPriceExclTax" + orderItemId], out unitPriceExclTax))
                unitPriceExclTax = orderItem.UnitPriceExclTax;
            if (!int.TryParse(form["pvQuantity" + orderItemId], out quantity))
                quantity = orderItem.Quantity;
            if (!decimal.TryParse(form["pvDiscountInclTax" + orderItemId], out discountInclTax))
                discountInclTax = orderItem.DiscountAmountInclTax;
            if (!decimal.TryParse(form["pvDiscountExclTax" + orderItemId], out discountExclTax))
                discountExclTax = orderItem.DiscountAmountExclTax;
            if (!decimal.TryParse(form["pvPriceInclTax" + orderItemId], out priceInclTax))
                priceInclTax = orderItem.PriceInclTax;
            if (!decimal.TryParse(form["pvPriceExclTax" + orderItemId], out priceExclTax))
                priceExclTax = orderItem.PriceExclTax;

            if (quantity > 0)
            {
                orderItem.UnitPriceInclTax = unitPriceInclTax;
                orderItem.UnitPriceExclTax = unitPriceExclTax;
                orderItem.Quantity = quantity;
                orderItem.DiscountAmountInclTax = discountInclTax;
                orderItem.DiscountAmountExclTax = discountExclTax;
                orderItem.PriceInclTax = priceInclTax;
                orderItem.PriceExclTax = priceExclTax;
                _orderService.UpdateOrder(order);
            }
            else
            {
                _orderService.DeleteOrderItem(orderItem);
            }

            var model = new OrderModel();
            PrepareOrderDetailsModel(model, order);

            //selected tab
            //SaveSelectedTabIndex(persistForTheNextRequest: false);

            return RedirectToAction("Edit", "Order", new { id = order.OrderId });
        }

        [HttpPost]
        public ActionResult DeleteOrderItem(int id, FormCollection form)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            //get order item identifier
            int orderItemId = 0;
            foreach (var formValue in form.AllKeys)
                if (formValue.StartsWith("btnDeleteOrderItem", StringComparison.InvariantCultureIgnoreCase))
                    orderItemId = Convert.ToInt32(formValue.Substring("btnDeleteOrderItem".Length));

            var orderItem = order.OrderItems.FirstOrDefault(x => x.Product.Id == orderItemId);
            if (orderItem == null)
                throw new ArgumentException("No order item found with the specified id");

            _orderService.DeleteOrderItem(orderItem);

            return RedirectToAction("Edit", "Order", new { id = order.OrderId });
        }

        #endregion

        #region OrderNoteProcessing
        public ActionResult OrderNoteAdd(int orderId, OrderModel model)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                return RedirectToAction("List");

            var orderNote = new OrderNote()
            {
                DisplayToCustomer = model.AddOrderNoteDisplayToCustomer,
                Note = model.AddOrderNoteMessage,
                CreatedOnUtc = DateTime.UtcNow,
            };
            order.OrderNotes.Add(orderNote);
            _orderService.UpdateOrderNotes(order);

            //new order notification
            if (model.AddOrderNoteDisplayToCustomer)
            {
                //email
                _messageService.SendNewOrderNoteAddedCustomerNotification(
                    orderNote, 0);

            }

            return RedirectToAction("Edit", "Order", new { id = order.OrderId });
        }
        [HttpPost]
        public ActionResult DeleteOrderNote(int id, FormCollection form)
        {

            var order = _orderService.GetOrderById(id);
            if (order == null)
                //No order found with the specified id
                return RedirectToAction("List");

            //get order item identifier
            int orderNoteId = 0;
            foreach (var formValue in form.AllKeys)
                if (formValue.StartsWith("btnDeleteOrderNote", StringComparison.InvariantCultureIgnoreCase))
                    orderNoteId = Convert.ToInt32(formValue.Substring("btnDeleteOrderItem".Length));

            var orderNote = order.OrderNotes.FirstOrDefault(on => on.Id == orderNoteId);
            if (orderNote == null)
                throw new ArgumentException("No order note found with the specified id");
            _orderService.DeleteOrderNote(orderNote);

            return RedirectToAction("Edit", "Order", new { id = order.OrderId });
        }
        #endregion
    }
}