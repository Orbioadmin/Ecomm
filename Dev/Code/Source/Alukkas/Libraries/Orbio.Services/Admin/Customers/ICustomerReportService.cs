
using Orbio.Core.Domain.Admin.Customers;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public partial interface ICustomerReportService
    {
        /// <summary>
        /// Gets a report of customers registered in the last days
        /// </summary>
        /// <param name="days">Customers registered in the last days</param>
        /// <returns>Number of registered customers</returns>
        int GetRegisteredCustomersReport(int days);

        /// <summary>
        /// Gets a report of top 20 customers with order no and order total
        /// </summary>
        List<BestCustomerReportLine> GetTopCustomerReport();


        List<BestCustomerReportLine> SearchCustomerReport(DateTime? startDateValue, DateTime? endDateValue, OrderStatus? orderStatus,
               PaymentStatus? paymentStatus, ShippingStatus? shippingStatus);
    }
}
