using Nop.Data;
using Orbio.Core.Domain.Checkout;
using Orbio.Core.Domain.Customers;
using Orbio.Services.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Checkout
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IDbContext context;
        private readonly IEncryptionService encryptionService;

         /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public CheckoutService(IDbContext context, IEncryptionService encryptionService)
        {
            this.context = context;
            this.encryptionService = encryptionService;
        }

        public Address GetCustomerAddress(string email, string billorShip)
        {
            if (String.IsNullOrWhiteSpace(email))
                return null;

            var result = context.ExecuteFunction<Address>("usp_Customer_GetCustomerAddressDetails",
                  new SqlParameter() { ParameterName = "@userName", Value = email, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@value", Value = billorShip, DbType = System.Data.DbType.String });

            var customerAddress = result.FirstOrDefault();
            return customerAddress;
        }

        public void UpdateCustomerAddress(string email, bool sameAddress, string billFirstName, string billLastName, string billPhone, string billAddress,
                string billCity, string billPincode, string billState, string billCountry, string shipFirstName, string shipLastName,
                string shipPhone, string shipAddress, string shipCity, string shipPincode, string shipState, string shipCountry)
        {
            var result = context.ExecuteFunction<Customer>("usp_Customer_UpdateCustomerAddress",
                new SqlParameter() { ParameterName = "@sameAddress", Value = sameAddress, DbType = System.Data.DbType.Boolean },
                new SqlParameter() { ParameterName = "@email", Value = email, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billFirstName", Value = billFirstName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billLastName", Value = billLastName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billPhoneNo", Value = billPhone, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billAddress", Value = billAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billCity", Value = billCity, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billPincode", Value = billPincode, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billState", Value = billState, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billCountry", Value = billCountry, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipFirstName", Value = shipFirstName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipLastName", Value = shipLastName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipPhone", Value = shipPhone, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipAddress", Value = shipAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipCity", Value = shipCity, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipPincode", Value = shipPincode, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipState", Value = shipState, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipCountry", Value = shipCountry, DbType = System.Data.DbType.String });
        }

    }
}
