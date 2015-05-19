using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Orders
{
    public class OrderService : IOrderService
    {
         private readonly IDbContext context;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
         public OrderService(IDbContext context)
        {
            this.context = context;
        }

         /// <summary>
         /// Get shopping cart items
         /// </summary>
         /// <param name="action">Action</param>
         public OrderDetails GetOrderDetails(int curCustomerId)
         {
             var sqlParamList = new List<SqlParameter>();
             sqlParamList.Add(new SqlParameter { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 });

             var result = context.ExecuteFunction<XmlResultSet>("usp_Customer_Order_Details",
                           sqlParamList.ToArray()
                           ).FirstOrDefault();
             if (result != null)
             {
                 var orderDetails = Serializer.GenericDeSerializer<OrderDetails>(result.XmlResult);
                 return orderDetails;
             }
             return new OrderDetails();
         }
    }
}
