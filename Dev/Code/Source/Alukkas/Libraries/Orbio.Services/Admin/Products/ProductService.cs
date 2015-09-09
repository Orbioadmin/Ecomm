using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
namespace Orbio.Services.Admin.Products
{
    public partial class ProductService:IProductService
    {
        #region Methods

        //Get list of products by search or default
        public List<Product> GetAllProductsSeachOrDefault(string nameOrSku,int? categoryId,int? manufatureId)
        {
            using (var context = new OrbioAdminContext())
            {
                context.Products.Include("Product_Picture_Mapping.Picture").Load();
                context.Products.Include("Product_PriceComponent_Mapping.PriceComponent").Load();
                context.Products.Include("Product_ProductComponent_Mapping.ProductComponent").Load();
                var productList = (from p in context.Products
                                   where p.Deleted != true
                                   select p).ToList();
                if (!string.IsNullOrEmpty(nameOrSku))
                {
                    productList = (from p in productList
                                   where (p.Name==nameOrSku || p.Sku==nameOrSku)
                                   select p).ToList();
                }
                if (categoryId > 0)
                {
                    productList = (from p in productList
                                   join pcm in context.Product_Category_Mapping on p.Id equals pcm.ProductId
                                   where pcm.CategoryId == categoryId
                                   select p).ToList();
                }
                if (manufatureId > 0)
                {
                    productList = (from p in productList
                                   join pmm in context.Product_Manufacturer_Mapping on p.Id equals pmm.ProductId
                                   where pmm.ManufacturerId == manufatureId
                                   select p).ToList();
                }

                return productList;
            }
        }

        //Get product by id
        public Product GetProductById(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                context.Products.Include("Product_Picture_Mapping.Picture").Load();
                context.Products.Include("Product_PriceComponent_Mapping.PriceComponent").Load();
                context.Products.Include("Product_ProductComponent_Mapping.ProductComponent").Load();
                var productList = (from p in context.Products
                                   where p.Deleted != true && p.Id == id
                                   select p).FirstOrDefault();
                return productList;
            }
        }

        //Add new product
        public void AddNewProduct(Product product)
        {
            using (var context = new OrbioAdminContext())
            {
                var productData = context.Products.FirstOrDefault();
                productData = product;
                context.Products.Add(productData);
                context.SaveChanges();
            }
        }

        //Delete selected products
        public void DeleteSelectedProducts(int[] idList)
        {
            using (var context = new OrbioAdminContext())
            {
                var product = context.Products.Where(f => idList.Contains(f.Id)).ToList();
                product.ForEach(a => a.Deleted = true);
                context.SaveChanges();
            }
        }
        #endregion

    }
}
