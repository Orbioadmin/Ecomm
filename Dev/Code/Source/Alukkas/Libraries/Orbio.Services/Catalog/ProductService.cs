using System.Collections.Generic;
using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Catalog;
using System.Linq;
using Orbio.Services.Utility;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Data;

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

        /// <summary>
        /// gets all related products by product id
        /// </summary>
        /// <param name="productid">the product id</param>
        /// <returns>list of products</returns>
        public RelatedProduct GetRelatedProductsById(int productId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@productid", Value = productId, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<XmlResultSet>("usp_Catalog_RelatedProducts",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if (result != null)
            {
                var relatedProduct = Serializer.GenericDeSerializer<RelatedProduct>(result.XmlResult);
                return relatedProduct;
            }

            return new RelatedProduct();
        }


        /// <summary>
        /// gets all associated products by product id
        /// </summary>
        /// <param name="productid">the product id</param>
        /// <returns>list of products</returns>
        public AssociatedProduct GetAssociatedProductsById(int productId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@productid", Value = productId, DbType = System.Data.DbType.Int32 });

            var result = context.ExecuteFunction<XmlResultSet>("usp_Catalog_AssociatedProducts",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if (result != null)
            {
                var associatedProduct = Serializer.GenericDeSerializer<AssociatedProduct>(result.XmlResult);
                return associatedProduct;
            }

            return new AssociatedProduct();
        }

        public int InsertReviews(int id, int productId, bool isApproved, string reviewTitle, string reviewText, int rating, string customerName)
        {
            var result = context.ExecuteFunction<ProductDetail>("usp_Customer_InsertCustomerReview",
                  new SqlParameter() { ParameterName = "@customerid", Value = id, DbType = System.Data.DbType.Int16 },
                   new SqlParameter() { ParameterName = "@productid", Value = productId, DbType = System.Data.DbType.Int16 },
                    new SqlParameter() { ParameterName = "@isapproved", Value = isApproved, DbType = System.Data.DbType.Boolean },
                     new SqlParameter() { ParameterName = "@reviewtitle", Value = reviewTitle, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@reviewtext", Value = reviewText, DbType = System.Data.DbType.String },
                      new SqlParameter() { ParameterName = "@rating", Value = rating, DbType = System.Data.DbType.Int16},
                       new SqlParameter() { ParameterName = "@name", Value = customerName, DbType = System.Data.DbType.String });

            return 0;
        }

        public List<ProductReview> GetCustomerReviews(int productId, string value)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@Value", Value = value, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter() { ParameterName = "@ProductId", Value = productId, DbType = System.Data.DbType.Int16 });

            var result = context.ExecuteFunction<ProductReview>("usp_Catalog_GetCustomerReviews",
             sqlParamList.ToArray());
           return result;
        }
    }
}