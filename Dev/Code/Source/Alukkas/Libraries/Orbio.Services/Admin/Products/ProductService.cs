using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
using Nop.Data;
using System.Data.SqlClient;
using Nop.Core.Domain;
using Orbio.Core.Utility;
namespace Orbio.Services.Admin.Products
{
    public partial class ProductService:IProductService
    {
        #region Fields
        private readonly IDbContext dbContext;
        #endregion

        public ProductService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Methods

        /// <summary>
        /// Get list of products by search or default
        /// </summary>
        /// <param name="nameOrSku"></param>
        /// <param name="categoryId"></param>
        /// <param name="manufatureId"></param>
        /// <returns></returns>
        public List<Orbio.Core.Domain.Catalog.Product> GetAllProductsSeachOrDefault(string nameOrSku, int? categoryId, int? manufatureId)
        {

            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_OrbioAdmin_GetAllProductList",
            new SqlParameter() { ParameterName = "@nameOrSku", Value = nameOrSku, DbType = System.Data.DbType.String },
            new SqlParameter() { ParameterName = "@categoryId", Value = categoryId, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@manufatureId", Value = manufatureId, DbType = System.Data.DbType.Int32 }
             ).FirstOrDefault();

            if (result != null && result.XmlResult != null)
            {
                var products = Serializer.GenericDeSerializer<List<Orbio.Core.Domain.Catalog.Product>>(result.XmlResult);
                return products;
            }

            return new List<Orbio.Core.Domain.Catalog.Product>();
        }

        /// <summary>
        /// Get product details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Insert new Product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="slug"></param>
        public void InsertNewProduct(Product product, string slug)
        {
            using (var context = new OrbioAdminContext())
            {
                var productData = context.Products.FirstOrDefault();
                productData = product;
                context.Products.Add(productData);
                context.SaveChanges();
                int Id = productData.Id;
                var UrlRecord = context.UrlRecords.Where(m => m.EntityName == "Product").FirstOrDefault();
                if (UrlRecord != null)
                {
                    UrlRecord.EntityId = Id;
                    UrlRecord.EntityName = "Product";
                    UrlRecord.Slug = slug;
                    UrlRecord.IsActive = true;
                    UrlRecord.LanguageId = 0;
                    context.UrlRecords.Add(UrlRecord);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Delete selected products
        /// </summary>
        /// <param name="idList"></param>
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
