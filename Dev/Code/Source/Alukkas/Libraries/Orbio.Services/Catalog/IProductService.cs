using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog;

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
        int InsertReviews(int id,int productid,bool isapproved,string ReviewTitle,string ReviewText,int Rating,string CustomerName);
    }
}
