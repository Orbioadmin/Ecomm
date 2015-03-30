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
        public void AddCartItem(string action, ShoppingCartType shoppingCartType, int CustomerId, int ProductId, string attributexml, int quantity)
        {
            context.ExecuteFunction<ShoppingCartItem>("usp_Shoppingcart_Items",
                   new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@id", Value = 0, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@shoppingCartTypeid", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@customerId", Value = CustomerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@productId", Value = ProductId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@attributexml", Value = (object)attributexml??DBNull.Value, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 }); 
        }
        /// <summary>
        /// Get shopping cart items
        /// </summary>
        /// <param name="action">Action</param>
        public ShoppingCartItems GetCartItems(string action, int id, ShoppingCartType shoppingCartType, int CustomerId, int ProductId, int quantity)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@shoppingCartTypeid", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@customerId", Value = CustomerId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@productId", Value = ProductId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@attributexml", Value = "", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 });

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
        /// <param name="cartItems">cartItems</param>
        public void ModifyCartItem(List<ShoppingCartItem> cartItems)
        {
            string cartItemsXml = Serializer.GenericSerializer(cartItems);
            context.ExecuteFunction<ShoppingCartItem>("usp_Shoppingcart_Items_Updates",
                   new SqlParameter() { ParameterName = "@list", Value = cartItemsXml, DbType = DbType.Xml });
        }
    }
}
