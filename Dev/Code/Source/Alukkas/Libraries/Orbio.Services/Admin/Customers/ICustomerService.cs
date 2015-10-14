using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomer(string FirstName, string LastName, string Email, List<int> Roles);

        Customer GetCustomerById(int Id);

        Customer GetOrderDetails(int Id);

        Customer GetCustomerAddressDetails(int Id);

        int AddOrUpdateCustomerInfo(Customer customer, List<int> Roles);

        int DeleteCustomer(int Id);

        string GetCustomerName(string email);
    }
}
