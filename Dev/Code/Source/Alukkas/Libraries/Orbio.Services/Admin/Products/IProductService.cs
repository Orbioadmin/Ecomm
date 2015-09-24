using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Products
{
    public partial interface IProductService
    {
        /// <summary>
        /// Get list of products by search or default
        /// </summary>
        /// <param name="nameOrSku"></param>
        /// <param name="categoryId"></param>
        /// <param name="manufatureId"></param>
        /// <returns></returns>
        List<Orbio.Core.Domain.Catalog.Product> GetAllProductsSeachOrDefault(string nameOrSku, int? categoryId, int? manufatureId);

        /// <summary>
        /// Get product details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product GetProductById(int id);

        /// <summary>
        /// Insert new Product
        /// </summary>
        /// <param name="productDetail"></param>
        void InsertNewProduct(Orbio.Core.Domain.Admin.Product.ProductDetail productDetail);

        /// <summary>
        /// Delete selected products
        /// </summary>
        /// <param name="idList"></param>
        void DeleteSelectedProducts(int[] idList);

        /// <summary>
        /// Get all product tags
        /// </summary>
        /// <returns></returns>
        List<ProductTag> GetAllProductTags();
    }
}
