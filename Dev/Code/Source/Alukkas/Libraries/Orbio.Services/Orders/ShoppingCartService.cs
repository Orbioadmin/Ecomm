using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Orbio.Services.Orders
{
   public class ShoppingCartService : IShoppingCartService
    {
        private readonly IDbContext context;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public ShoppingCartService(IDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Add shopping cart item
        /// </summary>
        /// <param name="action">Action</param>
        public void AddCartItem(string action, int ShoppingCartTypeId, int CustomerId, int ProductId, string attributexml, int Quantity)
        {
            context.ExecuteFunction<ShoppingCartItem>("usp_Shoppingcart_Items",
                   new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@id", Value = 0, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@shoppingcarttypeid", Value = ShoppingCartTypeId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@customerid", Value = CustomerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@productid", Value = ProductId, DbType = System.Data.DbType.Int32 },
                    new SqlParameter() { ParameterName = "@attributexml", Value = attributexml, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@quantity", Value = Quantity, DbType = System.Data.DbType.Int32 });
        }
        /// <summary>
        /// Get shopping cart items
        /// </summary>
        /// <param name="action">Action</param>
        public ShoppingCartItems GetCartItems(string action, int ShoppingCartTypeId, int CustomerId, int ProductId, int Quantity)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@id", Value = 0, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@shoppingcarttypeid", Value = ShoppingCartTypeId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@customerid", Value = CustomerId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@productid", Value = ProductId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@attributexml", Value = "", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@quantity", Value = Quantity, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<XmlResultSet>("usp_Shoppingcart_Items",
                          sqlParamList.ToArray()
                          ).FirstOrDefault();
            if (result != null)
            {
                var shoppingCartItem = Serializer.GenericDeSerializer<ShoppingCartItems>(result.XmlResult);
                return shoppingCartItem;
            }
            return new ShoppingCartItems();
        }

        /// <summary>
        /// Update and delete shopping cart item
        /// </summary>
        /// <param name="action">Action</param>
        public void ModifyCartItem(string cartitems)
        {
            context.ExecuteFunction<ShoppingCartItem>("usp_Shoppingcart_Items_Updates",
                   new SqlParameter() { ParameterName = "@list", Value = cartitems,DbType=DbType.Xml });
        }
    }
}
