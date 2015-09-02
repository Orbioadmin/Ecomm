using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public interface ICustomerRoleService
    {
        List<CustomerRole> GetAllCustomerRole();

        CustomerRole GetCustomerRoleById(int Id);

        int DeleteCustomerRole(int Id);

        int AddOrUpdateCustomerRole(CustomerRole customerRole);
    }
}
