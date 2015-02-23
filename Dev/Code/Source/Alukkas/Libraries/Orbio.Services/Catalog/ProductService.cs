using System.Collections.Generic;
using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Catalog;
using System.Linq;
using Orbio.Services.Utility;
using System.Data.SqlClient;

namespace Orbio.Services.Catalog
{
    public class ProductService : IProductService
    {

         private readonly IDbContext context;
        private const string entityName = "Product";

        /// <summary>
        /// instantiates Category service type
        /// </summary>
        /// <param name="context">db context</param>
        public ProductService(IDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// gets all productsdetails by slug
        /// </summary>
        /// <param name="slug">the slug value</param>
        /// <param name="entityName">the entity name</param>
        /// <returns>list of products</returns>

        public ProductDetail GetProductsDetailsBySlug(string slug)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@slug", Value = slug, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@entityName", Value = entityName, DbType = System.Data.DbType.String });

            var result = context.ExecuteFunction<XmlResultSet>("usp_Catalog_GetProductsDetailBySlug",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if (result != null)
            {
                var productdetails = Serializer.GenericDeSerializer<ProductDetail>(result.XmlResult);
                return productdetails;
            }

            return new ProductDetail();
        }
    }
}