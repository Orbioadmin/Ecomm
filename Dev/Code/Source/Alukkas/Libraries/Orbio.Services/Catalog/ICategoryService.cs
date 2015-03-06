using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog;
 

namespace Orbio.Services.Catalog
{
    /// <summary>
    /// interface for category service
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// gets topmenu categories
        /// </summary>
        /// <returns>list of category</returns>
        List<Category> GetTopMenuCategories();

        /// <summary>
        /// gets all products by slug
        /// </summary>
        /// <param name="slug">the slug value</param>
        /// <param name="entityName">the entity name</param>
        /// <returns>instance of CategoryProduct</returns>
        CategoryProduct GetProductsBySlug(string slug, string filterIds, decimal? minPrice, decimal? maxPrice, string keyword);

        /// <summary>
        /// gets all products by search
        /// </summary>
        /// <param name="slug">the slug value</param>
        /// <param name="entityName">the entity name</param>
        /// <returns>instance of CategoryProduct</returns>
        Search GetProductsBySearch(string slug, string filterIds, decimal? minPrice, decimal? maxPrice, string keyword);

        /// <summary>
        /// gets specification filters by category id
        /// </summary>
        /// <param name="categoryId">the category id</param>
        /// <returns>list of specification filter model</returns>
        List<SpecificationFilterModel> GetSpecificationFiltersByCategoryId(int categoryId, string keyword);

        /// <summary>
        /// gets specification filters by Search
        /// </summary>
        /// <param name="categoryId">the category id</param>
        /// <param name="categoryId">the keyword</param>
        /// <returns>list of specification filter model</returns>
        List<SpecificationFilterModel> GetSpecificationFiltersByCategory(string categoryId,string keyword);
        
    }


}
