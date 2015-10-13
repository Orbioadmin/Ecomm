using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public interface IProductAttributeService
    {
     List<ProductAttribute> GetProductAttributes();

     ProductAttribute GetProductAttributeById(int Id);

     void AddOrUpdateProductAttribute(int Id, string Name, string Description);

     int DeleteProductAttribute(int Id);

         /// <summary>
        /// GetProductVariantAttributeValueByProductVariantAttributeId
        /// </summary>
        /// <param name="productVariantAttributeId"></param>
        /// <returns></returns>
        List<ProductVariantAttributeValue> GetProductVariantAttributeValueByProductVariantAttributeId(int productVariantAttributeId);

        /// <summary>
        /// Insert product variant attribute value
        /// </summary>
        /// <param name="pvav"></param>
        void InsertProductVariantAttributeValue(ProductVariantAttributeValue pvav);

        /// <summary>
        /// Update product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValue"></param>
        /// <returns></returns>
        int UpdateProductVariantAttributeValue(ProductVariantAttributeValue productVariantAttributeValue);

         /// <summary>
        /// Delete product variant attribute value
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        void DeleteProductVariantAttributeValue(int Id);

        /// <summary>
        /// Update product variant attribute display order
        /// </summary>
        /// <param name="pvaIds"></param>
        void UpdateProductVariantAttributeValueDisplayOrder(int[] pvavIds);
    }
}
