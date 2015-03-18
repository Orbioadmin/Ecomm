using System.Collections.Generic;
using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Catalog;
using System.Linq;
using Orbio.Services.Utility;
using System.Data.SqlClient;

namespace Orbio.Services.Catalog
{
    /// <summary>
    /// category service implementations
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IDbContext context;
        private const string entityName = "Category";

        /// <summary>
        /// instantiates Category service type
        /// </summary>
        /// <param name="context">db context</param>
        public CategoryService(IDbContext context)
        {
            this.context = context;
        }
         /// <summary>
        /// gets topmenu categories
        /// </summary>
        /// <returns>list of category</returns>
        public List<Category> GetTopMenuCategories()
        {
            var result = context.ExecuteFunction<XmlResultSet>("usp_Catalog_GetTopMenu", null).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var categories = Serializer.GenericDataContractDeSerializer<List<Category>>(result.XmlResult);
                return categories;
            }

            return new List<Category>();
        }


        /// <summary>
        /// gets all products by slug
        /// </summary>
        /// <param name="slug">the slug value</param>
        /// <param name="entityName">the entity name</param>
        /// <returns>list of products</returns>
        public CategoryProduct GetProductsBySlug(string slug, string filterIds, decimal? minPrice, decimal? maxPrice, string keyword)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@slug", Value = slug, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@entityName", Value = entityName, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@keyword", Value = keyword, DbType = System.Data.DbType.String });

            if (!string.IsNullOrEmpty(filterIds))
            {
                sqlParamList.Add(new SqlParameter { ParameterName = "@specificationFilterIds", Value = filterIds, DbType = System.Data.DbType.String });
            }

            if (minPrice.HasValue)
            {
                sqlParamList.Add(new SqlParameter { ParameterName = "@minPrice", Value = minPrice, DbType = System.Data.DbType.Decimal });
            }

             if (maxPrice.HasValue)
            {
                sqlParamList.Add(new SqlParameter { ParameterName = "@maxPrice", Value = maxPrice, DbType = System.Data.DbType.Decimal });
            }

            var result = context.ExecuteFunction<XmlResultSet>("usp_Catalog_GetProductsBySlug",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if(result!=null)
            {
                var categoryProduct = Serializer.GenericDeSerializer<CategoryProduct>(result.XmlResult); 
            return categoryProduct;
            }

            return new CategoryProduct();
        }


        /// <summary>
        /// gets all products by Search
        /// </summary>
        /// <param name="slug">the slug value</param>
        /// <param name="entityName">the entity name</param>
        /// <returns>list of products</returns>
        public Search GetProductsBySearch(string slug, string filterIds, decimal? minPrice, decimal? maxPrice, string keyword)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@slug", Value = slug, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@entityName", Value = entityName, DbType = System.Data.DbType.String });
            sqlParamList.Add(new SqlParameter { ParameterName = "@keyword", Value = keyword, DbType = System.Data.DbType.String });

            if (!string.IsNullOrEmpty(filterIds))
            {
                sqlParamList.Add(new SqlParameter { ParameterName = "@specificationFilterIds", Value = filterIds, DbType = System.Data.DbType.String });
            }

            if (minPrice.HasValue)
            {
                sqlParamList.Add(new SqlParameter { ParameterName = "@minPrice", Value = minPrice, DbType = System.Data.DbType.Decimal });
            }

            if (maxPrice.HasValue)
            {
                sqlParamList.Add(new SqlParameter { ParameterName = "@maxPrice", Value = maxPrice, DbType = System.Data.DbType.Decimal });
            }

            var result = context.ExecuteFunction<XmlResultSet>("usp_Search",
                sqlParamList.ToArray()
                ).FirstOrDefault();
            if (result != null)
            {
                var categoryProduct = Serializer.GenericDeSerializer<Search>(result.XmlResult);
                return categoryProduct;
            }

            return new Search();
        }


        /// <summary>
        /// gets specification filters by category id
        /// </summary>
        /// <param name="categoryId">the category id</param>
        /// <returns>list of specification filter model</returns>
        public List<SpecificationFilterModel> GetSpecificationFiltersByCategoryId(int categoryId,string keyword)
        {
            var result = context.ExecuteFunction<SpecificationFilterModel>("usp_Catalog_GetFiltersByCategoryId",
              new SqlParameter() { ParameterName = "@categoryId", Value = categoryId, DbType = System.Data.DbType.Int32 },
              new SqlParameter() { ParameterName = "@keyword", Value = keyword, DbType = System.Data.DbType.String });
           
            return result!=null?result : new List<SpecificationFilterModel>();
        }

        /// <summary>
        /// gets specification filters by Search
        /// </summary>
        /// <param name="categoryId">the category id</param>
        ///   /// <param name="categoryId">the keyword</param>
        /// <returns>list of specification filter model</returns>
        public List<SpecificationFilterModel> GetSpecificationFiltersByCategory(string categoryId,string keyword)
        {
            var result = context.ExecuteFunction<SpecificationFilterModel>("usp_Catalog_GetFiltersByCategory",
              new SqlParameter() { ParameterName = "@categoryId", Value = categoryId, DbType = System.Data.DbType.String },
              new SqlParameter() { ParameterName = "@keyword", Value = keyword, DbType = System.Data.DbType.String });

            return result != null ? result : new List<SpecificationFilterModel>();
        }
    }
}
