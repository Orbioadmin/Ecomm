using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public class CustomerService : ICustomerService
    {
        public List<Customer> GetAllCustomer(string FirstName, string LastName, string Email, List<int> Roles)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = new List<Customer>();
                var query = context.Customers.Include("CustomerRoles").Where(m => !m.Deleted);

                if (FirstName != null)
                    query = query.Where(m => m.FirstName.Contains(FirstName));
                if (LastName != null)
                    query = query.Where(m => m.LastName.Contains(LastName));
                if (Email != null)
                    query = query.Where(m => m.Email.Contains(Email));
                if (Roles!=null)
                  
                query = query.Where(p => p.CustomerRoles.Any(cr=>Roles.Contains(cr.Id)));
                result = query.ToList();

                return result;
            }
        }
    }
}
