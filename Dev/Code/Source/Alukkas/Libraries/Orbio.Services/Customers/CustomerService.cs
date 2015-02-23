using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Orbio.Core.Domain.Customers;
using Orbio.Services.Security;

namespace Orbio.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbContext context;
        private readonly IEncryptionService encryptionService;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public CustomerService(IDbContext context, IEncryptionService encryptionService)
        {
            this.context = context;
            this.encryptionService = encryptionService;
        }

        /// <summary>
        /// gets the current customer or creates and returns a guest customer
        /// </summary>
        /// <param name="checkBackgroundTask">check if background task</param>
        /// <param name="isSearchEngine">see if search engine</param>
        /// <param name="authenticatedCustomerData">customer data </param>
        /// <param name="customerByCookieGuid">customer guid from cookie</param>
        /// <returns>returns a customer</returns>
        public Customer GetCurrentCustomer(bool checkBackgroundTask, bool isSearchEngine, string authenticatedCustomerData, string customerByCookieGuid, string ipAddress)
        {
            var result = context.ExecuteFunction<Customer>("usp_Customer_CurrentCustomer",
                   new SqlParameter() { ParameterName = "@checkBackgroundCustomer", Value = checkBackgroundTask, DbType = System.Data.DbType.Boolean },
                    new SqlParameter() { ParameterName = "@isSearchEngine", Value = isSearchEngine, DbType = System.Data.DbType.Boolean },
                     new SqlParameter() { ParameterName = "@authenticatedCustomerData", Value = authenticatedCustomerData, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@customerByCookieGuid", Value = customerByCookieGuid, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@ipAddress", Value = ipAddress, DbType = System.Data.DbType.String });
            var customer = result.FirstOrDefault();
            if (customer == null)
            {
                throw new Nop.Core.NopException("Could not load customer");
            }

            return customer;
        }

        /// <summary>
        /// Validate customer
        /// </summary>
        /// <param name="usernameOrEmail">Username or email</param>
        /// <param name="password">Password</param>
        /// <returns>Result</returns>
        public CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password, ref Customer customerOut)
        {
            //customerOut = null;
            var outputSqlParam = new SqlParameter() { ParameterName = "@loginResult", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 };
            var result = context.ExecuteFunction<Customer>("usp_Customer_ValidateAndGetCustomer",
                  new SqlParameter() { ParameterName = "@usernameOrEmail", Value = usernameOrEmail, DbType = System.Data.DbType.String },
                 outputSqlParam);

            var customer = result.FirstOrDefault();

            if (customer == null)
            {
                return (CustomerLoginResults)outputSqlParam.Value;
            }

            //check password and return
            string pwd = "";
            switch (customer.PasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    pwd = encryptionService.EncryptText(password);
                    break;
                case PasswordFormat.Hashed:
                    pwd = encryptionService.CreatePasswordHash(password, customer.PasswordSalt, ConfigurationManager.AppSettings["HashedPasswordFormat"]);
                    break;
                default:
                    pwd = password;
                    break;
            }
            // pwd = "F1EB4080B7307DAFB0BD5F9EB8A6E711C3827760";
            bool isValid = pwd == customer.Password;

            //save last login date
            if (isValid)
            {
                //customer.LastLoginDateUtc = DateTime.UtcNow;
                //_customerService.UpdateCustomer(customer);
                customerOut = customer;
                return CustomerLoginResults.Successful;
            }
            else
                return CustomerLoginResults.WrongPassword;

        }


        /// <summary>
        /// Update customer details
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="id">Id</param>
        /// <param name="firstname">Firstname</param>
        /// <param name="lastname"Llastname</param>
        /// <param name="gender">Gender</param>
        /// <param name="dob">DOB</param>
        /// <param name="email">Email</param>
        /// <param name="mobile">Mobile</param>
        public void GetCustomerDetails(string action, int id, string firstname, string lastname, string gender, string dob, string email, string mobile)
        {
            context.ExecuteFunction<Customer>("usp_Customer_updateCustomer",
                   new SqlParameter() { ParameterName = "@Action", Value = action, DbType = System.Data.DbType.String },
                    new SqlParameter() { ParameterName = "@cust_id", Value = id, DbType = System.Data.DbType.Int32 },
                     new SqlParameter() { ParameterName = "@firstname", Value = firstname, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@lastname", Value = lastname, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@gender", Value = gender, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@dob", Value = dob, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@email", Value = email, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@mobileno", Value = mobile, DbType = System.Data.DbType.String });
        }

        public CustomerLoginResults GetCustomerDetailsByEmail(string email, string password)
        {
            var outputSqlParam = new SqlParameter() { ParameterName = "@loginResult", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 };
            var result = context.ExecuteFunction<Customer>("usp_Customer_ValidateAndGetCustomer",
                  new SqlParameter() { ParameterName = "@usernameOrEmail", Value = email, DbType = System.Data.DbType.String },
                 outputSqlParam);

            var customer = result.FirstOrDefault();

            if (customer == null)
            {
                return (CustomerLoginResults)outputSqlParam.Value;
            }
            //check password and return
            string pwd = "";
            switch (customer.PasswordFormat)
            {
                case PasswordFormat.Encrypted:
                    pwd = encryptionService.EncryptText(password);
                    break;
                case PasswordFormat.Hashed:
                    pwd = encryptionService.CreatePasswordHash(password, customer.PasswordSalt, ConfigurationManager.AppSettings["HashedPasswordFormat"]);
                    break;
                default:
                    pwd = password;
                    break;
            }

            bool isValid = pwd == customer.Password;

            //save last login date
            if (isValid)
            {
                return CustomerLoginResults.Successful;
            }
            else
                return CustomerLoginResults.WrongPassword;

        }

        public ChangePasswordResult ChangePassword(int id, string newpassword, int PasswordFormat)
        {
            string pwd = "";
            pwd = encryptionService.CreatePasswordHash(newpassword, "F8f+q9M=", ConfigurationManager.AppSettings["HashedPasswordFormat"]);
            context.ExecuteFunction<Customer>("usp_Customer_ChangePassword",
                    new SqlParameter() { ParameterName = "@cust_id", Value = id, DbType = System.Data.DbType.Int32 },
                     new SqlParameter() { ParameterName = "@newpwd", Value = pwd, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@passwordformat", Value = PasswordFormat, DbType = System.Data.DbType.Int32 });

            return ChangePasswordResult.Successful;
        }

    }
}
