using Orbio.Core.Data;
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

        #endregion

    }
}
