using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Orbio.Core.Domain.Customers;

namespace Orbio.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbContext context;

          /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public CustomerService(IDbContext context)
        {
            this.context = context;           
        }

        /// <summary>
        /// gets the current customer or creates and returns a guest customer
        /// </summary>
        /// <param name="checkBackgroundTask">check if background task</param>
        /// <param name="isSearchEngine">see if search engine</param>
        /// <param name="authenticatedCustomerData">customer data </param>
        /// <param name="customerByCookieGuid">customer guid from cookie</param>
        /// <returns>returns a customer</returns>
        public  Customer GetCurrentCustomer(bool checkBackgroundTask, bool isSearchEngine, string authenticatedCustomerData, string customerByCookieGuid, string ipAddress)
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
    }
}
