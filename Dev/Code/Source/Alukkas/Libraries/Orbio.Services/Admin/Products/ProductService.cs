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
using Orbio.Core.Domain.Admin.Catalog;

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
                context.Products.Include("Product_Category_Mapping.Category").Load();
                context.Products.Include("Product_Manufacturer_Mapping.Manufacturer").Load();
                context.Products.Include("Product_PriceComponent_Mapping.PriceComponent").Load();
                context.Products.Include("Product_ProductComponent_Mapping.ProductComponent").Load();
                context.Products.Include("Product_SpecificationAttribute_Mapping.SpecificationAttributeOption.SpecificationAttribute").Load();
                context.Products.Include("Product_ProductAttribute_Mapping.ProductAttribute").Load();
                context.Products.Include("Product_ProductAttribute_Mapping.ProductVariantAttributeValues").Load();
                context.Products.Include("ProductTags").Load();
                var productList = (from p in context.Products
                                   join u in context.UrlRecords on p.Id equals u.EntityId
                                   where p.Deleted != true && p.Id == id
                                   select p).FirstOrDefault();
                return productList;
            }
        }

        /// <summary>
        /// Insert new Product
        /// </summary>
        /// <param name="productDetail"></param>
        public int InsertNewProduct(Orbio.Core.Domain.Admin.Product.ProductDetail productDetail)
        {
            var productXml = Serializer.GenericSerializer<Orbio.Core.Domain.Admin.Product.ProductDetail>(productDetail);
            //dbContext.ExecuteFunction<Product>("usp_Product_InsertProduct",
            //new SqlParameter() { ParameterName = "@productXml", Value = productXml, DbType = System.Data.DbType.Xml });

            var sqlParams = new SqlParameter[] {new SqlParameter{ ParameterName="@productXml", 
            Value=productXml, DbType= System.Data.DbType.Xml}, new SqlParameter{ ParameterName="@productId", 
            Value=0, DbType= System.Data.DbType.Int32, Direction= System.Data.ParameterDirection.Output} };
            var result = dbContext.ExecuteSqlCommand("EXEC [usp_Product_InsertProduct] @productXml, @productId OUTPUT", false, null, sqlParams);
            return Convert.ToInt32(sqlParams[1].Value);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productDetail"></param>
        public void UpdateProduct(Orbio.Core.Domain.Admin.Product.ProductDetail productDetail)
        {
            var productXml = Serializer.GenericSerializer<Orbio.Core.Domain.Admin.Product.ProductDetail>(productDetail);
            dbContext.ExecuteFunction<Product>("usp_Product_UpdateProduct",
            new SqlParameter() { ParameterName = "@productXml", Value = productXml, DbType = System.Data.DbType.Xml });
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

        /// <summary>
        /// Get all product tags
        /// </summary>
        /// <returns></returns>
        public List<ProductTag> GetAllProductTags()
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductTags.ToList();
                return result;
            }
        }

        #region Product Picture Mapping
        /// <summary>
        /// Insert to product picture mapping
        /// </summary>
        /// <param name="productPicture"></param>
        public void InsertToProductPictureMapping(Product_Picture_Mapping productPicture)
        {
            using (var context = new OrbioAdminContext())
            {
                context.Product_Picture_Mapping.Add(productPicture);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// Update product picture display order
        /// </summary>
        /// <param name="pictureIds"></param>
        public void UpdatePictureDisplayOrder(int[] pictureIds)
        {
            var pictureIdsXml = Serializer.GenericSerializer(pictureIds);
            dbContext.ExecuteFunction<Product_Picture_Mapping>("usp_UpdateProductPicture", new SqlParameter() { ParameterName = "@pictureIdsXml", Value = pictureIdsXml, DbType = System.Data.DbType.Xml });
        }

        /// <summary>
        /// Delete produc picture
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteProductPicture(int Id,int productId)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var query = context.Product_Picture_Mapping.Where(m => m.Id == Id).FirstOrDefault();
                    if (query != null)
                    {
                        context.Product_Picture_Mapping.Remove(query);
                        context.SaveChanges();
                        var productPicture = context.Product_Picture_Mapping.Where(p => p.Id > Id && p.ProductId==productId).ToList();
                        productPicture.ForEach(a => a.DisplayOrder = (a.DisplayOrder - 1));
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        #endregion

        #region Mapping Product specification attribute
        /// <summary>
        /// Insert value to product specification mapping
        /// </summary>
        /// <param name="specification"></param>
        public void InsertProductSpecificationAttribute(Product_SpecificationAttribute_Mapping specification)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = context.Product_SpecificationAttribute_Mapping.OrderByDescending(m => m.ProductId == specification.ProductId).First();
                int displayOrder = (query != null) ? query.DisplayOrder : 0;
                specification.DisplayOrder = displayOrder + 1;
                context.Product_SpecificationAttribute_Mapping.Add(specification);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update product specification attribute
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public int UpdateProductSpecificationAttribute(Product_SpecificationAttribute_Mapping specification)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Product_SpecificationAttribute_Mapping.Where(m => m.Id == specification.Id).FirstOrDefault();
                result.AllowFiltering = specification.AllowFiltering;
                result.ShowOnProductPage = specification.ShowOnProductPage;
                context.SaveChanges();
                return result.ProductId;
            }
        }

        /// <summary>
        /// Update product specification display order
        /// </summary>
        /// <param name="specificationIds"></param>
        public void UpdateSpecificationDisplayOrder(int[] specificationIds)
        {
            var specificationIdsXml = Serializer.GenericSerializer(specificationIds);
            dbContext.ExecuteFunction<Product_SpecificationAttribute_Mapping>("usp_UpdateProductSpecificationAttribute", new SqlParameter() { ParameterName = "@specificationIdsXml", Value = specificationIdsXml, DbType = System.Data.DbType.Xml });
        }

        /// <summary>
        /// Delete produc specification attribute
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void DeleteProductSpecificationAttribute(int Id)
        {
            using (var context = new OrbioAdminContext())
            {

                 var query = context.Product_SpecificationAttribute_Mapping.Where(m => m.Id == Id).FirstOrDefault();
                    if (query != null)
                    {
                        context.Product_SpecificationAttribute_Mapping.Remove(query);
                        context.SaveChanges();
                        var productSpecification = context.Product_SpecificationAttribute_Mapping.Where(p => p.Id > Id).ToList();
                        productSpecification.ForEach(a => a.DisplayOrder = (a.DisplayOrder - 1));
                        context.SaveChanges();
                    }
            }
        }
        #endregion

        #region Mapping Product variant attribute
        /// <summary>
        /// Insert value to product variant attribute mapping
        /// </summary>
        /// <param name="productVariantAttribute"></param>
        public void InsertProductVariantAttribute(Product_ProductAttribute_Mapping productVariantAttribute)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = context.Product_ProductAttribute_Mapping.OrderByDescending(m => m.ProductId == productVariantAttribute.ProductId).First();
                int displayOrder = (query != null) ? query.DisplayOrder : 0;
                productVariantAttribute.DisplayOrder = displayOrder + 1;
                context.Product_ProductAttribute_Mapping.Add(productVariantAttribute);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update product variant attribute
        /// </summary>
        /// <param name="productVariantAttribute"></param>
        /// <returns></returns>
        public int UpdateProductVariantAttribute(Product_ProductAttribute_Mapping productVariantAttribute)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Product_ProductAttribute_Mapping.Where(m => m.Id == productVariantAttribute.Id).FirstOrDefault();
                result.ProductAttributeId = productVariantAttribute.ProductAttributeId;
                result.IsRequired = productVariantAttribute.IsRequired;
                result.TextPrompt = productVariantAttribute.TextPrompt;
                result.AttributeControlTypeId = productVariantAttribute.AttributeControlTypeId;
                context.SaveChanges();
                return result.ProductId;
            }
        }

        /// <summary>
        /// Add or update product variant attribute size guide url
        /// </summary>
        /// <param name="productVariantAttribute"></param>
        public void AddOrUpdateProductVariantAttributeSizeGuide(Product_ProductAttribute_Mapping productVariantAttribute)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Product_ProductAttribute_Mapping.Where(m => m.Id == productVariantAttribute.Id).FirstOrDefault();
                result.SizeGuideUrl = productVariantAttribute.SizeGuideUrl;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete SizeGuideUrl
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteSizeGuideUrl(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.Product_ProductAttribute_Mapping.Where(m => m.Id == Id).FirstOrDefault();
                    if (result != null)
                    {
                        result.SizeGuideUrl = null;
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Delete product variant attribute
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void DeleteProductVariantAttribute(int Id)
        {
            using (var context = new OrbioAdminContext())
            {

                var query = context.Product_ProductAttribute_Mapping.Where(m => m.Id == Id).FirstOrDefault();
                if (query != null)
                {
                    context.Product_ProductAttribute_Mapping.Remove(query);
                    context.SaveChanges();
                    var productVariantAttribute = context.Product_ProductAttribute_Mapping.Where(p => p.Id > Id).ToList();
                    productVariantAttribute.ForEach(a => a.DisplayOrder = (a.DisplayOrder - 1));
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update product variant attribute display order
        /// </summary>
        /// <param name="pvaIds"></param>
        public void UpdateProductVariantAttributeDisplayOrder(int[] pvaIds)
        {
            var pvaIdsXml = Serializer.GenericSerializer(pvaIds);
            dbContext.ExecuteFunction<Product_ProductAttribute_Mapping>("usp_UpdateProductVariantAttributeDisplayOrder", new SqlParameter() { ParameterName = "@pvaIdsXml", Value = pvaIdsXml, DbType = System.Data.DbType.Xml });
        }

        /// <summary>
        /// GetProductVariantAttributeByProductVariantAttributeId
        /// </summary>
        /// <param name="productVariantAttributeId"></param>
        /// <returns></returns>
        public List<Product_ProductAttribute_Mapping> GetProductVariantAttributeProductVariantAttributeId(int productVariantAttributeId)
        {
            using (var context = new OrbioAdminContext())
            {
                context.Product_ProductAttribute_Mapping.Include("Product").Load();
                return context.Product_ProductAttribute_Mapping.Where(pvav => pvav.Id == productVariantAttributeId).ToList();
            }
        }


        #endregion

        #region Discounts

        /// <summary>
        /// Get all discounts for products by Id
        /// </summary>
        public List<Orbio.Core.Domain.Discounts.Discount> GetAllDiscountsForProductsById(int id)
        {
            var sqlParamList = new List<SqlParameter>();
            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_GetAllProductDiscounts", new SqlParameter() { ParameterName = "@productid", Value = id, DbType = System.Data.DbType.Int32 }).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var order = Serializer.GenericDeSerializer<List<Orbio.Core.Domain.Discounts.Discount>>(result.XmlResult);
                return order;
            }

            return new List<Orbio.Core.Domain.Discounts.Discount>();
        }

        #endregion

        #endregion

    }
}
