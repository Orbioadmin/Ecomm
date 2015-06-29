using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Utility;

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
        public void AddCartItem(string action, ShoppingCartType shoppingCartType,int curCustomerId, int customerId, int productId, string attributexml, int quantity)
        {
            context.ExecuteFunction<ShoppingCartItem>("usp_Shoppingcart_Items",
                   new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@id", Value = 0, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@shoppingCartTypeId", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@productId", Value = productId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@attributexml", Value = (object)attributexml??DBNull.Value, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 }); 
        }

        /// <summary>
        /// Add wishlist item
        /// </summary>
        /// <param name="action">Action</param>
        public string AddWishlistItem(string action, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, string attributexml, int quantity)
        {
            var result = context.ExecuteFunction<string>("usp_Shoppingcart_Items",
                   new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@id", Value = 0, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@shoppingCartTypeId", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@productId", Value = productId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@attributexml", Value = (object)attributexml ?? DBNull.Value, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 });
            return result[0].ToString();
        }

        /// <summary>
        /// Migrate shopping cart item
        /// </summary>
        /// <param name="action">Action</param>
        public void MigrateShoppingCart(string action, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, string attributexml, int quantity)
        {
            context.ExecuteFunction<ShoppingCartItem>("usp_Shoppingcart_Items",
                   new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@id", Value = 0, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@shoppingCartTypeId", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@productId", Value = productId, DbType = System.Data.DbType.Int32 },
                   new SqlParameter() { ParameterName = "@attributexml", Value = (object)attributexml ?? DBNull.Value, DbType = System.Data.DbType.String },
                   new SqlParameter() { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 });
        }
        /// <summary>
        /// Get shopping cart items
        /// </summary>
        /// <param name="action">Action</param>
        public Cart GetCartItems(string action, int id, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, int quantity, int storeId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@shoppingCartTypeId", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@productId", Value = productId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@attributexml", Value = "", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@storeId", Value = storeId, DbType = System.Data.DbType.Int32 });
            var result = context.ExecuteFunction<XmlResultSet>("usp_Shoppingcart_Items",
                          sqlParamList.ToArray()
                          ).FirstOrDefault();
            if (result != null)
            {
                var cart = Serializer.GenericDeSerializer<Cart>(result.XmlResult);
                return cart;
            }
            return new Cart();
        }


        /// <summary>
        /// Update wishlist items
        /// </summary>
        /// <param name="action">Action</param>
        public string UpdateWishListItems(string action, int id, ShoppingCartType shoppingCartType, int curCustomerId, int customerId, int productId, int quantity)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@id", Value = id, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@shoppingCartTypeId", Value = (int)shoppingCartType, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@customerId", Value = customerId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@productId", Value = productId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@attributexml", Value = "", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@quantity", Value = quantity, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<string>("usp_Shoppingcart_Items",
                          sqlParamList.ToArray()
                          ).FirstOrDefault();

                return result;
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
