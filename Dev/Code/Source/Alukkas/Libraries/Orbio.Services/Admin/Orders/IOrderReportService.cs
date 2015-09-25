using System.Collections.Generic;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Payments;
using Orbio.Core.Domain.Shipping;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Admin.Orders;
using Orbio.Core.Payments;
namespace Orbio.Services.Admin.Orders
{
    public partial interface IOrderReportService
    {

        /// <summary>
        /// Get order average reports from last 12 months
        /// </summary>
        /// <returns></returns>
        List<OrderAverageReport> GetOrderAverageReport();

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
        OrderAverageReportLine GetOrderAverageReportLine(int storeId, int vendorId, OrderStatus? os,
            PaymentStatus? ps, ShippingStatus? ss, DateTime? startTimeUtc,
            DateTime? endTimeUtc, string billingEmail, bool ignoreCancelledOrders = false);

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="os">Order status</param>
        /// <returns>Result</returns>
        OrderAverageReportLineSummary OrderAverageReport(int storeId, OrderStatus os);

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
        decimal ProfitReport(int storeId, int vendorId, OrderStatus? os, PaymentStatus? ps, ShippingStatus? ss,
            DateTime? startTimeUtc, DateTime? endTimeUtc, string billingEmail);

    }
}
