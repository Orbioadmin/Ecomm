using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Nop.Data;
using Orbio.Core.Domain.Common;
using Nop.Core.Infrastructure;

namespace Orbio.Services.Common
{
    /// <summary>
    /// GenericAttribute service implementations
    /// </summary>
    public class GenericAttributeService : IGenericAttributeService
    {
        private readonly IDbContext context;

        public GenericAttributeService()
        {
            this.context = EngineContext.Current.Resolve<IDbContext>();
        }

        /// <summary>
        /// instantiates GenericAttribute service type
        /// </summary>
        /// <param name="context">db context</param>
        public GenericAttributeService(IDbContext context)
        {
            this.context = context;
        }

        ///<summary>
        ///get all generic attributes
        ///</summary>
        ///<param name="action">the action value</param>
        ///<param name="entityId">the entityId(customerId) value</param>
        ///<param name="keyGroup">the keyGroup value</param>
        ///<param name="key">the key value</param>
        ///<param name="value">the keyValue value</param>
        ///<param name="storeId">the storeId value</param>
        public GenericAttribute GetGenericAttributes(int entityId, string keyGroup, string key, string value, int storeId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter { ParameterName = "@action", Value = "select", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@entityId", Value = entityId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@keyGroup", Value = keyGroup, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@key", Value = key, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@value", Value = value, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@storeId", Value = storeId, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<GenericAttribute>("usp_Customer_Generic_Attributes",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if (result != null)
            {
                return result;
            }

            return new GenericAttribute();
        }

        ///<summary>
        ///insert all customer generic attributes to table
        ///</summary>
        ///<param name="action">the action value</param>
        ///<param name="entityId">the entityId(customerId) value</param>
        ///<param name="keyGroup">the keyGroup value</param>
        ///<param name="key">the key value</param>
        ///<param name="value">the keyValue value</param>
        ///<param name="storeId">the storeId value</param>
        public void SaveGenericAttributes(int entityId, string keyGroup, string key, string value, int storeId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter { ParameterName = "@action", Value = "save", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@entityId", Value = entityId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@keyGroup", Value = keyGroup, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@key", Value = key, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@value", Value = value, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@storeId", Value = storeId, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<GenericAttribute>("usp_Customer_Generic_Attributes",
                sqlParamList.ToArray()
                );
        }



        public void DeleteGenericAttribute(int entityId, string keyGroup, string key, string value, int storeId)
        {
            var sqlParamList = new List<SqlParameter>();
          
            sqlParamList.Add(new SqlParameter { ParameterName = "@action", Value = "del", DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@entityId", Value = entityId, DbType = System.Data.DbType.Int32 });
            sqlParamList.Add(new SqlParameter { ParameterName = "@keyGroup", Value = keyGroup, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@key", Value = key, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@value", Value = value, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@storeId", Value = storeId, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<GenericAttribute>("usp_Customer_Generic_Attributes",
                sqlParamList.ToArray()
                );
        }
    }
}
