using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Admin.Orders;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public class ShoppingCartService : IShoppingCartService
    {
       private readonly IDbContext dbContext;

       public ShoppingCartService(IDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        #region methods

        /// <summary>
        /// get all current shopping cart customer
        /// </summary>
        /// <returns></returns>
        public ShoppingCart GetShoppingCartAllCustomer(ShoppingCartType shoppingCartType,int storeId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter { ParameterName = "@shoppingCartTypeId", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@storeId", Value = storeId, DbType = System.Data.DbType.Int32 });
            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_OrbioAdmin_GetCurrentCartDetails",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var shoppingCart = Serializer.GenericDeSerializer<ShoppingCart>(result.XmlResult);
                return shoppingCart;
            }

            return new ShoppingCart();
        }

        #endregion
    }
}
