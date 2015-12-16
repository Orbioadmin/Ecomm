using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Orbio.Services.Admin.Attributes
{
    public class ProductAttributeService : IProductAttributeService
    {
        #region Fields
        private readonly IDbContext dbContext;
        #endregion

        public ProductAttributeService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<ProductAttribute> GetProductAttributes()
        {
            using (var context = new OrbioAdminContext())
            {
                var model = context.ProductAttributes.ToList();
                return model;
            }
        }

        public ProductAttribute GetProductAttributeById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var model = context.ProductAttributes.Where(m => m.Id == Id).FirstOrDefault();
                return model;
            }
        }

        public void AddOrUpdateProductAttribute(int Id, string Name, string Description)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductAttributes.Where(m => m.Id == Id).FirstOrDefault();
                if (result != null)
                {
                    result.Name = Name;
                    result.Description = Description;
                    context.SaveChanges();
                }
                else
                {
                    var productAttribute = new ProductAttribute();
                    productAttribute.Name = Name;
                    productAttribute.Description = Description;
                    context.ProductAttributes.Add(productAttribute);
                    context.SaveChanges();
                }
            }
        }

        public int DeleteProductAttribute(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.ProductAttributes.Where(m => m.Id == Id).FirstOrDefault();
                    if (result != null)
                    {
                        context.ProductAttributes.Remove(result);
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
        /// GetProductVariantAttributeValueByProductVariantAttributeId
        /// </summary>
        /// <param name="productVariantAttributeId"></param>
        /// <returns></returns>
        public List<ProductVariantAttributeValue> GetProductVariantAttributeValueByProductVariantAttributeId(int productVariantAttributeId)
        {
            using (var context = new OrbioAdminContext())
            {
                context.ProductVariantAttributeValues.Include("Product_ProductAttribute_Mapping.Product").Load();
                context.ProductVariantAttributeValues.Include("Product_ProductAttribute_Mapping.ProductAttribute").Load();
                return context.ProductVariantAttributeValues.Where(pvav => pvav.ProductVariantAttributeId == productVariantAttributeId).ToList();
            }
        }
        /// <summary>
        /// Insert product variant attribute value
        /// </summary>
        /// <param name="pvav"></param>
        public void InsertProductVariantAttributeValue(ProductVariantAttributeValue pvav)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = context.ProductVariantAttributeValues.OrderByDescending(m => m.ProductVariantAttributeId == pvav.ProductVariantAttributeId).First();
                int displayOrder = (query != null) ? query.DisplayOrder : 0;
                pvav.DisplayOrder = displayOrder + 1;
                context.ProductVariantAttributeValues.Add(pvav);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValue"></param>
        /// <returns></returns>
        public int UpdateProductVariantAttributeValue(ProductVariantAttributeValue productVariantAttributeValue)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductVariantAttributeValues.Where(m => m.Id == productVariantAttributeValue.Id).FirstOrDefault();
                result.Name = productVariantAttributeValue.Name;
                result.PriceAdjustment = productVariantAttributeValue.PriceAdjustment;
                result.WeightAdjustment = productVariantAttributeValue.WeightAdjustment;
                result.IsPreSelected = productVariantAttributeValue.IsPreSelected;
                context.SaveChanges();
                return result.ProductVariantAttributeId;
            }
        }

        /// <summary>
        /// Delete product variant attribute value
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void DeleteProductVariantAttributeValue(int Id)
        {
            using (var context = new OrbioAdminContext())
            {

                var query = context.ProductVariantAttributeValues.Where(m => m.Id == Id).FirstOrDefault();
                if (query != null)
                {
                    context.ProductVariantAttributeValues.Remove(query);
                    context.SaveChanges();
                    var productVariantAttributeValue = context.ProductVariantAttributeValues.Where(p => p.Id > Id).ToList();
                    productVariantAttributeValue.ForEach(a => a.DisplayOrder = (a.DisplayOrder - 1));
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Update product variant attribute display order
        /// </summary>
        /// <param name="pvaIds"></param>
        public void UpdateProductVariantAttributeValueDisplayOrder(int[] pvavIds)
        {
            var pvavIdsXml = Serializer.GenericSerializer(pvavIds);
            dbContext.ExecuteFunction<Product_ProductAttribute_Mapping>("usp_UpdateProductVariantAttributeValueDisplayOrder", new SqlParameter() { ParameterName = "@pvavIdsXml", Value = pvavIdsXml, DbType = System.Data.DbType.Xml });
        }
    }
}
