using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
namespace Orbio.Services.Checkout
{
    public class TransientCartService : ITransientCartService
    {
        private readonly IDbContext context;

        public TransientCartService(IDbContext context)
        {
            this.context = context;
        }

        public TransientCart GetTransientCart(int id, int customerId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@transientCartId", Value = id, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 });
            var result = context.ExecuteFunction<XmlResultSet>("usp_GetTransientCart",
                        sqlParamList.ToArray()
                        ).FirstOrDefault();
            if (result != null)
            {
                var cart = Serializer.GenericDeSerializer<TransientCart>(result.XmlResult);
                return cart;
            }
            return new TransientCart();  
        }

        public int UpdateTransientCart(int id, int customerId, TransientCart cart)
        {
            var sqlParams = new SqlParameter[] { new SqlParameter { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 },
                 new SqlParameter { ParameterName = "@transientCartXml", Value = Serializer.GenericSerializer<TransientCart>(cart), DbType = System.Data.DbType.Xml },
                  new SqlParameter() { ParameterName = "@transientCartId", Value = id, DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.InputOutput }
             };
             context.ExecuteSqlCommand("dbo.usp_UpdateTransientCart @customerId, @transientCartXml, @transientCartId OUTPUT  ", false, null, sqlParams);
             return Convert.ToInt32(sqlParams[2].Value);
        }
    }
}
