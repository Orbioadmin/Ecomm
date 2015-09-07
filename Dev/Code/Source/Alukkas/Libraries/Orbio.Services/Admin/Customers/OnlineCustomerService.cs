using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public class OnlineCustomerService : IOnlineCustomerService
    {
        public List<Customer> GetOnlineCustomers()
        {
            using (var context = new OrbioAdminContext())
            {
                DateTime curdate=DateTime.UtcNow.AddMinutes(-20);
                var result = context.Customers.Include("CustomerRoles")
                    .Where(m => m.LastActivityDateUtc >= curdate && !m.Deleted).OrderByDescending(m => m.LastActivityDateUtc).ToList();
                return result;
            }
        }
    }
}
