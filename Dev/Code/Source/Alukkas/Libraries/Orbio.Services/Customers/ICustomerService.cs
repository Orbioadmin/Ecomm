using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Customers;

namespace Orbio.Services.Customers
{
     /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerService
    {
        /// <summary>
        /// gets the current customer or creates and returns a guest customer
        /// </summary>
        /// <param name="checkBackgroundTask">check if background task</param>
        /// <param name="isSearchEngine">see if search engine</param>
        /// <param name="authenticatedCustomerData">customer data </param>
        /// <param name="customerByCookieGuid">customer guid from cookie</param>
        /// <returns>returns a customer</returns>
        Customer GetCurrentCustomer(bool checkBackgroundTask, bool isSearchEngine, string @authenticatedCustomerData, string customerByCookieGuid, string ipAddress);

         /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password, ref Customer customerOut);
        
        /// <summary>
        /// Get customer details
        /// </summary>
        /// <param name="action">action</param>
        /// <param name="id">id</param>
        /// <param name="firstname">firstname</param>
        /// <param name="lastname">lastname</param>
        /// <param name="gender">gender</param>
        /// <param name="dob">dob</param>
        /// <param name="email">email</param>
        /// <param name="mobile">mobile</param>
        /// <returns>void</returns>
        void GetCustomerDetails(string action, int id, string firstName, string lastName, string gender, string dob, string email, string mobile);

        /// <summary>
        /// Get customer details by email and password
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>returns a customer</returns>
        CustomerLoginResults GetCustomerDetailsByEmail(string email, string password);

        /// <summary>
        /// Get customer details by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>returns a customer</returns>
        CustomerLoginResults GetCustomerDetailsByEmail(string email, ref Customer customerOut);

        /// <summary>
        /// Update customer password
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="newpassword">newpassword</param>
        /// <param name="PasswordFormat">PasswordFormat</param>
        /// <returns>Update Password</returns>
        ChangePasswordResult ChangePassword(int id, string newPassword, int passwordFormat);

        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request,List<int> roles,List<int> discounts);

        CustomerRegistrationResult ValidateNewCustomer(string userName);
    }
}
