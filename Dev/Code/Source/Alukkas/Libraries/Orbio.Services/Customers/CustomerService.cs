using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Orbio.Core.Domain.Customers;
using Orbio.Services.Customers;
using Orbio.Services.Security;
using System.Text.RegularExpressions;

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

        public ChangePasswordResult ChangePassword(int id, string newpassword, int passwordformat)
        {
            string pwd = "";
            string saltKey = encryptionService.CreateSaltKey(5);
            pwd = encryptionService.CreatePasswordHash(newpassword, saltKey, ConfigurationManager.AppSettings["HashedPasswordFormat"]);
            context.ExecuteFunction<Customer>("usp_Customer_ChangePassword",
                    new SqlParameter() { ParameterName = "@cust_id", Value = id, DbType = System.Data.DbType.Int32 },
                     new SqlParameter() { ParameterName = "@newpwd", Value = pwd, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@passwordformat", Value = passwordformat, DbType = System.Data.DbType.Int32 },
                        new SqlParameter() { ParameterName = "@passwordsalt", Value = saltKey, DbType = System.Data.DbType.String });
            return ChangePasswordResult.Successful;
        }

        /// <summary>
        /// validating new user
        /// </summary>
        /// <param name="usernameOrEmail"></param>
        /// <returns></returns>
        public CustomerRegistrationResult ValidateNewCustomer(string usernameOrEmail)
        {
            //customerOut = null;
            var outputSqlParam = new SqlParameter() { ParameterName = "@loginResult", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 };
            var result = context.ExecuteFunction<Customer>("usp_Customer_ValidateAndGetCustomer",
                  new SqlParameter() { ParameterName = "@usernameOrEmail", Value = usernameOrEmail, DbType = System.Data.DbType.String },
                 outputSqlParam);

            var customer = result.FirstOrDefault();

            if (customer == null)
            {
                return CustomerRegistrationResult.NewUser;
            }
            else
                return CustomerRegistrationResult.ExistingUser;

        }

        /// <summary>
        /// Register customer
        /// </summary>
        /// <param name="request">Request</param>
        /// <returns>Result</returns>
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {

            //if (request == null)
            //    throw new ArgumentNullException("request");

            //if (request.Customer == null)
            //    throw new ArgumentException("Can't load current customer");
            //if (request.Customer.IsSearchEngineAccount())
            //{
            //    result.AddError("Search engine can't be registered");
            //    return result;
            //}
            //if (request.Customer.IsBackgroundTaskAccount())
            //{
            //    result.AddError("Background task account can't be registered");
            //    return result;
            //}
            //if (request.Customer.IsRegistered())
            //{
            //    result.AddError("Current customer is already registered");
            //    return result;
            //}
            //if (String.IsNullOrEmpty(request.Email))
            //{
            //    result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailIsNotProvided"));
            //    return result;
            //}
            if (!IsValidEmail(request.Email))
            {
                return CustomerRegistrationResult.InvalidEmail;
            }
            if (String.IsNullOrWhiteSpace(request.Password))
            {
                return CustomerRegistrationResult.ProvidePassword;
            }
            //if (_customerSettings.UsernamesEnabled)
            //{
            //    if (String.IsNullOrEmpty(request.Username))
            //    {
            //        result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameIsNotProvided"));
            //        return result;
            //    }
            //}

            ////validate unique user
            //if (_customerService.GetCustomerByEmail(request.Email) != null)
            //{
            //    result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailAlreadyExists"));
            //    return result;
            //}
            //if (_customerSettings.UsernamesEnabled)
            //{
            //    if (_customerService.GetCustomerByUsername(request.Username) != null)
            //    {
            //        result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameAlreadyExists"));
            //        return result;
            //    }
            //}

            //at this point request is valid
            request.Customer.Username = request.Username;
            request.Customer.Email = request.Email;
            request.Customer.PasswordFormat = request.PasswordFormat;
            request.Customer.Gender = request.Gender;
            request.Customer.MobileNo = request.MobileNo;
            request.Customer.IsApproved = request.IsApproved;

            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        request.Customer.Password = request.Password;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        request.Customer.Password = encryptionService.EncryptText(request.Password);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = encryptionService.CreateSaltKey(5);
                        request.Customer.PasswordSalt = saltKey;
                        request.Customer.Password = encryptionService.CreatePasswordHash(request.Password, saltKey, ConfigurationManager.AppSettings["HashedPasswordFormat"]);
                    }
                    break;
                default:
                    break;
            }

            request.Customer.Active = request.IsApproved;

            //add to 'Registered' role
            var registeredRole = GetCustomerRoleBySystemName(SystemCustomerNames.Registered);
            if (registeredRole == null)
            {
                throw new Nop.Core.NopException("Registered' role could not be loaded");
            }
            request.Customer.IsTaxExempt = registeredRole.TaxExempt;
            request.Customer.CustomerRole.Add(registeredRole);
            //remove from 'Guests' role
            var guestRole = request.Customer.CustomerRole.FirstOrDefault(cr => cr.SystemName == SystemCustomerNames.Guests);
            if (guestRole != null)
                request.Customer.CustomerRole.Remove(guestRole);

            ////Add reward points for customer registration (if enabled)
            //if (_rewardPointsSettings.Enabled &&
            //    _rewardPointsSettings.PointsForRegistration > 0)
            //    request.Customer.AddRewardPointsHistoryEntry(_rewardPointsSettings.PointsForRegistration, _localizationService.GetResource("RewardPoints.Message.EarnedForRegistration"));

            var updatedresult = UpdateCustomer(request.Customer);

            return (CustomerRegistrationResult)updatedresult;

        }

        public virtual CustomerRole GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;
            //   var outputSqlParam = new SqlParameter() { ParameterName = "@CustomerRole", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 };
            var result = context.ExecuteFunction<CustomerRole>("usp_CustomerRole_GetCustomerRoleBySystemName",
                  new SqlParameter() { ParameterName = "@systemname", Value = systemName, DbType = System.Data.DbType.String });
            var customerRole = result.FirstOrDefault();
            return customerRole;
        }

        public virtual CustomerRegistrationResult UpdateCustomer(Customer customer)
        {
            //if (String.IsNullOrWhiteSpace(customer.ToString()))
            //    return CustomerRegistrationResult.;

            var outputSqlParam = new SqlParameter() { ParameterName = "@insertResult", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 };
            var result = context.ExecuteFunction<Customer>("usp_Customer_InsertCustomer",
                 new SqlParameter() { ParameterName = "@Action", Value = "Insert", DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@email", Value = customer.Email, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@Password", Value = customer.Password, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@PasswordFormatId", Value = customer.PasswordFormatId, DbType = System.Data.DbType.Int16 },
                 new SqlParameter() { ParameterName = "@PasswordSalt", Value = customer.PasswordSalt, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@AdminComment", Value = customer.AdminComment, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@IsTaxExempt", Value = customer.IsTaxExempt, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@AffiliateId", Value = customer.AffiliateId, DbType = System.Data.DbType.Int16 },
                 new SqlParameter() { ParameterName = "@VendorId", Value = customer.VendorId, DbType = System.Data.DbType.Int16 },
                 new SqlParameter() { ParameterName = "@Active", Value = customer.Active, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@Deleted", Value = customer.Deleted, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@IsSystemAccount", Value = customer.IsSystemAccount, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@SystemName", Value = customer.SystemName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@LastIpAddress", Value = customer.LastIpAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@CreatedOnUtc", Value = customer.CreatedOnUtc, DbType = System.Data.DbType.DateTime },
                 new SqlParameter() { ParameterName = "@LastLoginDateUtc", Value = customer.LastLoginDateUtc, DbType = System.Data.DbType.DateTime },
                 new SqlParameter() { ParameterName = "@LastActivityDateUtc", Value = customer.LastActivityDateUtc, DbType = System.Data.DbType.DateTime },
                 new SqlParameter() { ParameterName = "@BillingAddress_Id", Value = customer.BillingAddressId, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@ShippingAddress_Id", Value = customer.ShippingAddressId, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@firstname", Value = customer.FirstName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@lastname", Value = customer.LastName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@gender", Value = customer.Gender, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@dob", Value = customer.DOB, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@companyname", Value = customer.CompanyName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@mobileno", Value = customer.MobileNo, DbType = System.Data.DbType.String },
                outputSqlParam);

            //var updateresult = result.FirstOrDefault();
            return (CustomerRegistrationResult)outputSqlParam.Value;

        }
        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase);
            return result;
        }
    }
}
