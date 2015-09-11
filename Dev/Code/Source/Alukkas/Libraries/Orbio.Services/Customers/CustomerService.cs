using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Checkout;
using Orbio.Services.Customers;
using Orbio.Services.Security;
using System.Text.RegularExpressions;
using Orbio.Core.Utility;


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
        public void GetCustomerDetails(string action, int id, string firstName, string lastName, string gender, string dob, string email, string mobile)
        {
            context.ExecuteFunction<Customer>("usp_Customer_updateCustomer",
                   new SqlParameter() { ParameterName = "@Action", Value = action, DbType = System.Data.DbType.String },
                    new SqlParameter() { ParameterName = "@cust_id", Value = id, DbType = System.Data.DbType.Int32 },
                     new SqlParameter() { ParameterName = "@firstName", Value = firstName, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@lastName", Value = lastName, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@gender", Value = gender, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@dob", Value = dob, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@email", Value = email, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@mobileNo", Value = mobile, DbType = System.Data.DbType.String });
        }

        /// <summary>
        /// Get customer details by email and password
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="password">password</param>
        /// <returns>returns a customer</returns>
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

        /// <summary>
        /// Get customer details by email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>returns a customer</returns>
        public CustomerLoginResults GetCustomerDetailsByEmail(string email, ref Customer customerOut)
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
            else
            {
                customerOut = customer;
                return CustomerLoginResults.Successful;
            }
        }
        /// <summary>
        /// Update customer password
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="newpassword">newpassword</param>
        /// <param name="PasswordFormat">PasswordFormat</param>
        /// <returns>Update Password</returns>
        public ChangePasswordResult ChangePassword(int id, string newPassword, int passwordformat)
        {
            string pwd = "";
            string saltKey = encryptionService.CreateSaltKey(5);
            pwd = encryptionService.CreatePasswordHash(newPassword, saltKey, ConfigurationManager.AppSettings["HashedPasswordFormat"]);
            context.ExecuteFunction<Customer>("usp_Customer_ChangePassword",
                    new SqlParameter() { ParameterName = "@cust_id", Value = id, DbType = System.Data.DbType.Int32 },
                     new SqlParameter() { ParameterName = "@newpwd", Value = pwd, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@passwordSalt", Value = saltKey, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@passwordFormat", Value = passwordformat, DbType = System.Data.DbType.Int32 });

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
        public virtual CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request,List<int> roles)
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
           
            if (String.IsNullOrWhiteSpace(request.password))
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
            request.customer.Username = request.userName;
            request.customer.Email = request.email;
            request.customer.PasswordFormat = request.passwordFormat;
            request.customer.Gender = request.gender;
            request.customer.MobileNo = request.mobileNo;
            request.customer.IsApproved = request.isApproved;

            switch (request.passwordFormat)
            {
                case PasswordFormat.Clear:
                    {
                        request.customer.Password = request.password;
                    }
                    break;
                case PasswordFormat.Encrypted:
                    {
                        request.customer.Password = encryptionService.EncryptText(request.password);
                    }
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = encryptionService.CreateSaltKey(5);
                        request.customer.PasswordSalt = saltKey;
                        request.customer.Password = encryptionService.CreatePasswordHash(request.password, saltKey, ConfigurationManager.AppSettings["HashedPasswordFormat"]);
                    }
                    break;
                default:
                    break;
            }

            request.customer.Active = request.isApproved;

            //add to 'Registered' role
            //var registeredRole = GetCustomerRoleBySystemName(SystemCustomerNames.Registered);
            //if (registeredRole == null)
            //{
            //    throw new Nop.Core.NopException("Registered' role could not be loaded");
            //}
            //request.Customer.IsTaxExempt = registeredRole.TaxExempt;
            //request.Customer.CustomerRole.Add(registeredRole);
            ////remove from 'Guests' role
            //var guestRole = request.Customer.CustomerRole.FirstOrDefault(cr => cr.SystemName == SystemCustomerNames.Guests);
            //if (guestRole != null)
            //    request.Customer.CustomerRole.Remove(guestRole);

            ////Add reward points for customer registration (if enabled)
            //if (_rewardPointsSettings.Enabled &&
            //    _rewardPointsSettings.PointsForRegistration > 0)
            //    request.Customer.AddRewardPointsHistoryEntry(_rewardPointsSettings.PointsForRegistration, _localizationService.GetResource("RewardPoints.Message.EarnedForRegistration"));

            var updatedresult = UpdateCustomer(request.customer,roles);

            return (CustomerRegistrationResult)updatedresult;

        }

        public virtual Customer GetCustomerRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;
            //   var outputSqlParam = new SqlParameter() { ParameterName = "@CustomerRole", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 };
            var result = context.ExecuteFunction<Customer>("usp_CustomerRole_GetCustomerRoleBySystemName",
                  new SqlParameter() { ParameterName = "@systemname", Value = systemName, DbType = System.Data.DbType.String });
            var customerRole = result.FirstOrDefault();
            return customerRole;
        }

        public virtual CustomerRegistrationResult UpdateCustomer(Customer customer,List<int> roles)
        {
            //if (String.IsNullOrWhiteSpace(customer.ToString()))
            //    return CustomerRegistrationResult.;
            string action = "";
            if (!customer.IsRegistered)
            {
                action = "Insert";
            }
            else
            {
                action = "Update";
            }

            var roleXml = (roles != null) ? Serializer.GenericSerializer(roles) : null;

            var outputSqlParam = new SqlParameter() { ParameterName = "@insertresult", Direction = System.Data.ParameterDirection.Output, DbType = System.Data.DbType.Int32 }; 
            var result = context.ExecuteFunction<Customer>("usp_Customer_InsertCustomer",

                 new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@id", Value = customer.Id, DbType = System.Data.DbType.Int32 },
                 new SqlParameter() { ParameterName = "@email", Value = customer.Email, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@password", Value = customer.Password, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@passwordformatid", Value = customer.PasswordFormatId, DbType = System.Data.DbType.Int32 },
                 new SqlParameter() { ParameterName = "@passwordsalt", Value = customer.PasswordSalt, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@admincomment", Value = customer.AdminComment, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@istaxexempt", Value = customer.IsTaxExempt, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@affiliateid", Value = customer.AffiliateId, DbType = System.Data.DbType.Int32 },
                 new SqlParameter() { ParameterName = "@vendorid", Value = customer.VendorId, DbType = System.Data.DbType.Int32 },
                 new SqlParameter() { ParameterName = "@active", Value = customer.Active, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@deleted", Value = customer.Deleted, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@issystemaccount", Value = customer.IsSystemAccount, DbType = System.Data.DbType.Boolean },
                 new SqlParameter() { ParameterName = "@systemname", Value = customer.SystemName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@lastipaddress", Value = customer.LastIpAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@createdonutc", Value = customer.CreatedOnUtc, DbType = System.Data.DbType.DateTime },
                 new SqlParameter() { ParameterName = "@lastlogindateutc", Value = customer.LastLoginDateUtc, DbType = System.Data.DbType.DateTime },
                 new SqlParameter() { ParameterName = "@lastactivitydateutc", Value = customer.LastActivityDateUtc, DbType = System.Data.DbType.DateTime },
                 new SqlParameter() { ParameterName = "@billingaddress_id", Value = customer.BillingAddress_Id, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shippingaddress_id", Value = customer.ShippingAddress_Id, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@firstname", Value = customer.FirstName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@lastname", Value = customer.LastName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@gender", Value = customer.Gender, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@dob", Value = customer.DOB, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@companyname", Value = customer.CompanyName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@mobileno", Value = customer.MobileNo, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@customerroles", Value = roleXml, DbType = System.Data.DbType.Xml },
                outputSqlParam);

            //var updateresult = result.FirstOrDefault();
            return (CustomerRegistrationResult)outputSqlParam.Value;

        }
    }
}
