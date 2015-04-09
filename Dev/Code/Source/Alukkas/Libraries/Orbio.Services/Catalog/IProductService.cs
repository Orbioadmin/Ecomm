using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog;
using System.Data;

namespace Orbio.Services.Catalog
{
    public interface IProductService
    {

        /// <summary>
        /// gets product details by slug
        /// </summary>
        /// <param name="slug">the slug value</param>
        /// <param name="entityName">the entity name</param>
        /// <returns>instance of Product details</returns>
        ProductDetail GetProductsDetailsBySlug(string slug);


         /// <summary>
        /// gets all related products by product id
        /// </summary>
        /// <param name="productid">the product id</param>
        /// <returns>list of products</returns>
        RelatedProduct GetRelatedProductsById(int productId);

         /// <summary>
        /// gets all associated products by product id
        /// </summary>
        /// <param name="productid">the product id</param>
        /// <returns>list of products</returns>
        AssociatedProduct GetAssociatedProductsById(int productId);

        /// <summary>
        /// insert customer reviews
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productid"></param>
        /// <param name="isapproved"></param>
        /// <param name="ReviewTitle"></param>
        /// <param name="ReviewText"></param>
        /// <param name="Rating"></param>
        /// <param name="CustomerName"></param>
        /// <returns></returns>
        int InsertReviews(int id, int productId, bool isApproved, string reviewTitle, string reviewText, int rating, string customerName);

        /// <summary>
        /// getting customer reviews
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        List<ProductReview> GetCustomerReviews(int productId, string value, int pageNumber, int pageSize);
    }
}
