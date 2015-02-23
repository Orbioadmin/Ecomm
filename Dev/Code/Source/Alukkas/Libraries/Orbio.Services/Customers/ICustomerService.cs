﻿using System;
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

        void GetCustomerDetails(string action, int id, string firstname, string lastname, string gender, string dob, string email, string mobile);

        CustomerLoginResults GetCustomerDetailsByEmail(string email, string password);

        ChangePasswordResult ChangePassword(int id, string newpassword, int PasswordFormat);

        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request);

        CustomerRegistrationResult ValidateNewCustomer(string username);
    }
}
