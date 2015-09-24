using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Orbio.Core;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Utility;
using Orbio.Services.Payments;
using System.Globalization;
using Orbio.Services.Helpers;
using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Orders;
using Orbio.Core.Payments;
using Nop.Data;
using Nop.Core.Domain;

namespace Orbio.Services.Admin.Orders
{
    public class OrderReportService : IOrderReportService
    {
        #region Fields
        private readonly OrbioAdminContext context = new OrbioAdminContext(); 
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDbContext dbContext;
        #endregion
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public OrderReportService(IDateTimeHelper dateTimeHelper, IDbContext dbContext)
        {
            this.dbContext = dbContext;
            //this.context = context;
            this._dateTimeHelper = dateTimeHelper;
        }

        #region Methods

        /// <summary>
        /// Get order average reports from last 12 months
        /// </summary>
        /// <returns></returns>
        public List<OrderAverageReport> GetOrderAverageReport()
        {
            var sqlParamList = new List<SqlParameter>();
            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_OrbioAdmin_GetOrderAverageReport", sqlParamList.ToArray()).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var order = Serializer.GenericDeSerializer<List<OrderAverageReport>>(result.XmlResult);
                return order;
            }

            return new List<OrderAverageReport>();
        }


        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="os">Order status</param>
        /// <param name="ps">Payment status</param>
        /// <param name="ss">Shipping status</param>
        /// <param name="startTimeUtc">Start date</param>
        /// <param name="endTimeUtc">End date</param>
        /// <param name="billingEmail">Billing email. Leave empty to load all records.</param>
        /// <param name="ignoreCancelledOrders">A value indicating whether to ignore cancelled orders</param>
        /// <returns>Result</returns>
        public virtual OrderAverageReportLine GetOrderAverageReportLine(int storeId, int vendorId, OrderStatus? os,
            PaymentStatus? ps, ShippingStatus? ss, DateTime? startTimeUtc, DateTime? endTimeUtc,
            string billingEmail, bool ignoreCancelledOrders = false)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;

            var query = context.Orders.AsQueryable();
            query = query.Where(o => !o.Deleted);
            if (storeId > 0)
                query = query.Where(o => o.StoreId == storeId);

            if (ignoreCancelledOrders)
            {
                int cancelledOrderStatusId = (int)OrderStatus.Cancelled;
                query = query.Where(o => o.OrderStatusId != cancelledOrderStatusId);
            }
            if (orderStatusId.HasValue)
                query = query.Where(o => o.OrderStatusId == orderStatusId.Value);
            if (paymentStatusId.HasValue)
                query = query.Where(o => o.PaymentStatusId == paymentStatusId.Value);
            if (shippingStatusId.HasValue)
                query = query.Where(o => o.ShippingStatusId == shippingStatusId.Value);
            if (startTimeUtc.HasValue)
                query = query.Where(o => startTimeUtc.Value <= o.CreatedOnUtc);
            if (endTimeUtc.HasValue)
                query = query.Where(o => endTimeUtc.Value >= o.CreatedOnUtc);
            //if (!String.IsNullOrEmpty(billingEmail))
            //query = query.Where(o => o.BillingAddress != null && !String.IsNullOrEmpty(o.BillingAddress.Email) && o.BillingAddress.Email.Contains(billingEmail));

            var item = (from oq in query
                        group oq by 1 into result
                        select new
                        {
                            OrderCount = result.Count(),
                            OrderShippingExclTaxSum = result.Sum(o => o.OrderShippingExclTax),
                            OrderTaxSum = result.Sum(o => o.OrderTax),
                            OrderTotalSum = result.Sum(o => o.OrderTotal)
                        }
                       ).Select(r => new OrderAverageReportLine()
                       {
                           CountOrders = r.OrderCount,
                           SumShippingExclTax = r.OrderShippingExclTaxSum,
                           SumTax = r.OrderTaxSum,
                           SumOrders = r.OrderTotalSum
                       })
                       .FirstOrDefault();

            item = item ?? new OrderAverageReportLine()
            {
                CountOrders = 0,
                SumShippingExclTax = decimal.Zero,
                SumTax = decimal.Zero,
                SumOrders = decimal.Zero,
            };
            return item;
        }

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="os">Order status</param>
        /// <returns>Result</returns>
        public virtual OrderAverageReportLineSummary OrderAverageReport(int storeId, OrderStatus os)
        {
            var item = new OrderAverageReportLineSummary();
            item.OrderStatus = os;

            DateTime nowDt = _dateTimeHelper.ConvertToUserTime(DateTime.Now);
            TimeZoneInfo timeZone = _dateTimeHelper.CurrentTimeZone;

            //today
            DateTime t1 = new DateTime(nowDt.Year, nowDt.Month, nowDt.Day);
            if (!timeZone.IsInvalidTime(t1))
            {
                DateTime? startTime1 = _dateTimeHelper.ConvertToUtcTime(t1, timeZone);
                DateTime? endTime1 = null;
                var todayResult = GetOrderAverageReportLine(storeId, 0, os, null, null, startTime1, endTime1, null);
                item.SumTodayOrders = todayResult.SumOrders;
                item.CountTodayOrders = todayResult.CountOrders;
            }
            //week
            DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime today = new DateTime(nowDt.Year, nowDt.Month, nowDt.Day);
            DateTime t2 = today.AddDays(-(today.DayOfWeek - fdow));
            if (!timeZone.IsInvalidTime(t2))
            {
                DateTime? startTime2 = _dateTimeHelper.ConvertToUtcTime(t2, timeZone);
                DateTime? endTime2 = null;
                var weekResult = GetOrderAverageReportLine(storeId, 0, os, null, null, startTime2, endTime2, null);
                item.SumThisWeekOrders = weekResult.SumOrders;
                item.CountThisWeekOrders = weekResult.CountOrders;
            }
            //month
            DateTime t3 = new DateTime(nowDt.Year, nowDt.Month, 1);
            if (!timeZone.IsInvalidTime(t3))
            {
                DateTime? startTime3 = _dateTimeHelper.ConvertToUtcTime(t3, timeZone);
                DateTime? endTime3 = null;
                var monthResult = GetOrderAverageReportLine(storeId, 0, os, null, null, startTime3, endTime3, null);
                item.SumThisMonthOrders = monthResult.SumOrders;
                item.CountThisMonthOrders = monthResult.CountOrders;
            }
            //year
            DateTime t4 = new DateTime(nowDt.Year, 1, 1);
            if (!timeZone.IsInvalidTime(t4))
            {
                DateTime? startTime4 = _dateTimeHelper.ConvertToUtcTime(t4, timeZone);
                DateTime? endTime4 = null;
                var yearResult = GetOrderAverageReportLine(storeId, 0, os, null, null, startTime4, endTime4, null);
                item.SumThisYearOrders = yearResult.SumOrders;
                item.CountThisYearOrders = yearResult.CountOrders;
            }
            //all time
            DateTime? startTime5 = null;
            DateTime? endTime5 = null;
            var allTimeResult = GetOrderAverageReportLine(storeId, 0, os, null, null, startTime5, endTime5, null);
            item.SumAllTimeOrders = allTimeResult.SumOrders;
            item.CountAllTimeOrders = allTimeResult.CountOrders;

            return item;
        }

        /// <summary>
        /// Get profit report
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="startTimeUtc">Start date</param>
        /// <param name="endTimeUtc">End date</param>
        /// <param name="os">Order status; null to load all records</param>
        /// <param name="ps">Order payment status; null to load all records</param>
        /// <param name="ss">Shipping status; null to load all records</param>
        /// <param name="billingEmail">Billing email. Leave empty to load all records.</param>
        /// <returns>Result</returns>
        public virtual decimal ProfitReport(int storeId, int vendorId,
            OrderStatus? os, PaymentStatus? ps, ShippingStatus? ss,
            DateTime? startTimeUtc, DateTime? endTimeUtc,
            string billingEmail)
        {
            int? orderStatusId = null;
            if (os.HasValue)
                orderStatusId = (int)os.Value;

            int? paymentStatusId = null;
            if (ps.HasValue)
                paymentStatusId = (int)ps.Value;

            int? shippingStatusId = null;
            if (ss.HasValue)
                shippingStatusId = (int)ss.Value;
            //We cannot use String.IsNullOrEmpty(billingEmail) in SQL Compact
            bool dontSearchEmail = String.IsNullOrEmpty(billingEmail);
            var query = from orderItem in context.OrderItems.AsQueryable()
                        join o in context.Orders on orderItem.OrderId equals o.Id
                        where (!startTimeUtc.HasValue || startTimeUtc.Value <= o.CreatedOnUtc) &&
                              (!endTimeUtc.HasValue || endTimeUtc.Value >= o.CreatedOnUtc) &&
                              (!orderStatusId.HasValue || orderStatusId == o.OrderStatusId) &&
                              (!paymentStatusId.HasValue || paymentStatusId == o.PaymentStatusId) &&
                              (!shippingStatusId.HasValue || shippingStatusId == o.ShippingStatusId) &&
                              (!o.Deleted)
                        //we do not ignore deleted products when calculating order reports
                        //(!p.Deleted) &&
                        //(!pv.Deleted) &&

                        select orderItem;

            var productCost = Convert.ToDecimal(query.Sum(orderItem => (decimal?)orderItem.OriginalProductCost * orderItem.Quantity));

            var reportSummary = GetOrderAverageReportLine(storeId, vendorId, os, ps, ss, startTimeUtc, endTimeUtc, billingEmail);
            var profit = reportSummary.SumOrders - reportSummary.SumShippingExclTax - reportSummary.SumTax - productCost;
            return profit;
        }

        #endregion
    }
}
