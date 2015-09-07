using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Customers;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Payments;
using Orbio.Services.Customers;
using Orbio.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    /// <summary>
    /// Customer report service
    /// </summary>
    public partial class CustomerReportService : ICustomerReportService
    {
        #region Fields

        private readonly OrbioAdminContext context = new OrbioAdminContext(); 
        private readonly ICustomerService _customerService;
        private readonly IDateTimeHelper _dateTimeHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerService">Customer service</param>
        /// <param name="dateTimeHelper">Date time helper</param>
        public CustomerReportService(ICustomerService customerService,
            IDateTimeHelper dateTimeHelper)
        {
            this._customerService = customerService;
            this._dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a report of customers registered in the last days
        /// </summary>
        /// <param name="days">Customers registered in the last days</param>
        /// <returns>Number of registered customers</returns>
        public virtual int GetRegisteredCustomersReport(int days)
        {
            DateTime date = _dateTimeHelper.ConvertToUserTime(DateTime.Now).AddDays(-days);

            //var registeredCustomerRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            //if (registeredCustomerRole == null)
                //return 0;

            var query = from c in context.Customers.AsQueryable()
                        from cr in c.CustomerRoles
                        where !c.Deleted &&
                        cr.Id == 3 &&
                        c.CreatedOnUtc >= date
                        //&& c.CreatedOnUtc <= DateTime.UtcNow
                        select c;
            int count = query.Count();
            return count;
        }


        public List<BestCustomerReportLine> GetTopCustomerReport()
        {
            var query = (from c in context.Customers
                         join o in context.Orders on c.Id equals o.CustomerId
                         where (!o.Deleted && !c.Deleted)
                         group o by o.CustomerId into temp
                         select new BestCustomerReportLine
                         {
                             CustomerId = temp.Key,
                             Email = temp.Select(c=>c.Customer.Email).FirstOrDefault(),
                             OrderTotal = temp.Sum(o => o.OrderTotal),
                             OrderCount = temp.Count(),
                         }).Take(20).ToList();

            return query;
        }

        public List<BestCustomerReportLine> SearchCustomerReport(DateTime? startDateValue, DateTime? endDateValue, OrderStatus? orderStatus,
               PaymentStatus? paymentStatus, ShippingStatus? shippingStatus)
        {
            int? orderStatusId = null;
            if (orderStatus.HasValue)
                orderStatusId = (int)orderStatus.Value;

            int? paymentStatusId = null;
            if (paymentStatus.HasValue)
                paymentStatusId = (int)paymentStatus.Value;

            int? shippingStatusId = null;
            if (shippingStatus.HasValue)
                shippingStatusId = (int)shippingStatus.Value;

            var query = context.Orders.Where(o=>!o.Deleted);
            if (orderStatusId.HasValue)
                query = query.Where(o => o.OrderStatusId == orderStatusId.Value);
            if (paymentStatusId.HasValue)
                query = query.Where(o => o.PaymentStatusId == paymentStatusId.Value);
            if (shippingStatusId.HasValue)
                query = query.Where(o => o.ShippingStatusId == shippingStatusId.Value);
            if (startDateValue.HasValue)
                query = query.Where(o => startDateValue.Value <= o.CreatedOnUtc);
            if (endDateValue.HasValue)
                query = query.Where(o => endDateValue.Value >= o.CreatedOnUtc);

            var result = (from c in context.Customers
                          join o in query on c.Id equals o.CustomerId
                         where (!c.Deleted)
                         group o by o.CustomerId into temp
                         select new BestCustomerReportLine
                         {
                             CustomerId = temp.Key,
                             Email = temp.Select(c => c.Customer.Email).FirstOrDefault(),
                             OrderTotal = temp.Sum(o => o.OrderTotal),
                             OrderCount = temp.Count(),
                         }).Take(20).ToList();

            return result;
        }

        #endregion

    }
}
