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
        //Get list of products by search or default
        List<Product> GetAllProductsSeachOrDefault(string nameOrSku, int? categoryId, int? manufatureId);

        //Get product by id
        Product GetProductById(int id);

        //Add new product
        void AddNewProduct(Product product);

        //Delete selected products
        void DeleteSelectedProducts(int[] idList);
    }
}
