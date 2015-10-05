using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Catalog
{
   public interface ICategoryServices
    {
       List<Orbio.Core.Domain.Catalog.Category> GetTopMenuCategories();

       CategoryDetails GetCategoryDetailsById(int Id);

       CategoryDetails GetCategoryDetails();

       int DeleteCategory(int Id);

       int AddOrUpdateCategory(CategoryList model);

       /// <summary>
       /// Delete Category Product from category product mapping table
       /// </summary>
       /// <param name="categoryId">Category Id</param>
       /// <param name="productId">Product Id</param>
       void DeleteCategoryProduct(int categoryId, int productId);

       List<Product_Category_Mapping> GetCategoryProducts(int Id);
    }
}
