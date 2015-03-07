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

        public Address GetCustomerAddress(string email, string billorship)
        {
            if (String.IsNullOrWhiteSpace(email))
                return null;

            var result = context.ExecuteFunction<Address>("usp_Customer_GetCustomerAddressDetails",
                  new SqlParameter() { ParameterName = "@username", Value = email, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@value", Value = billorship, DbType = System.Data.DbType.String });

            var customerAddress = result.FirstOrDefault();
            return customerAddress;
        }

        public void UpdateCustomerAddress(string email, bool sameaddress, string BillFirstName, string BillLastName, string BillPhone, string BillAddress,
                string BillCity, string BillPincode, string BillState, string BillCountry, string ShipFirstName, string ShipLastName,
                string ShipPhone, string ShipAddress, string ShipCity, string ShipPincode, string ShipState, string ShipCountry)
        {
            var result = context.ExecuteFunction<Customer>("usp_Customer_UpdateCustomerAddress",
                new SqlParameter() { ParameterName = "@sameaddress", Value = sameaddress, DbType = System.Data.DbType.Boolean },
                new SqlParameter() { ParameterName = "@email", Value = email, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billfirstname", Value = BillFirstName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billlastname", Value = BillLastName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billphoneno", Value = BillPhone, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billaddress", Value = BillAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billcity", Value = BillCity, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billpincode", Value = BillPincode, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billstate", Value = BillState, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@billcountry", Value = BillCountry, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipfirstname", Value = ShipFirstName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shiplastname", Value = ShipLastName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipphone", Value = ShipPhone, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipaddress", Value = ShipAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipcity", Value = ShipCity, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shippincode", Value = ShipPincode, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipstate", Value = ShipState, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@shipcountry", Value = ShipCountry, DbType = System.Data.DbType.String });
        }

    }
}
