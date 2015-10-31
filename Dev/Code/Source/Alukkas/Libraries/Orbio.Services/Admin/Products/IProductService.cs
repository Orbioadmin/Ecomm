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
        int InsertNewProduct(Orbio.Core.Domain.Admin.Product.ProductDetail productDetail);

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="productDetail"></param>
        void UpdateProduct(Orbio.Core.Domain.Admin.Product.ProductDetail productDetail);

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

        /// <summary>
        /// Insert to product picture mapping
        /// </summary>
        /// <param name="productPicture"></param>
        void InsertToProductPictureMapping(Product_Picture_Mapping productPicture);

        /// <summary>
        /// Update product picture display order
        /// </summary>
        /// <param name="pictureIds"></param>
        void UpdatePictureDisplayOrder(int[] pictureIds);

        /// <summary>
        /// Delete produc picture
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int DeleteProductPicture(int Id, int productId);

        /// <summary>
        /// Insert value to product specification mapping
        /// </summary>
        /// <param name="specification"></param>
        void InsertProductSpecificationAttribute(Product_SpecificationAttribute_Mapping specification);

        /// <summary>
        /// Update product specification attribute
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        int UpdateProductSpecificationAttribute(Product_SpecificationAttribute_Mapping specification);

        /// <summary>
        /// Update product specification display order
        /// </summary>
        /// <param name="pictureIds"></param>
        void UpdateSpecificationDisplayOrder(int[] specificationIds);

        /// <summary>
        /// Delete produc specification attribute
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        void DeleteProductSpecificationAttribute(int Id);

        /// <summary>
        /// Insert value to product variant attribute mapping
        /// </summary>
        /// <param name="productVariantAttribute"></param>
        void InsertProductVariantAttribute(Product_ProductAttribute_Mapping productVariantAttribute);

        /// <summary>
        /// Update product variant attribute
        /// </summary>
        /// <param name="productVariantAttribute"></param>
        /// <returns></returns>
        int UpdateProductVariantAttribute(Product_ProductAttribute_Mapping productVariantAttribute);

        /// <summary>
        /// Add or update product variant attribute size guide url
        /// </summary>
        /// <param name="productVariantAttribute"></param>
        void AddOrUpdateProductVariantAttributeSizeGuide(Product_ProductAttribute_Mapping productVariantAttribute);

        /// <summary>
        /// Delete SizeGuideUrl
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int DeleteSizeGuideUrl(int Id);

        /// <summary>
        /// Delete product variant attribute
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        void DeleteProductVariantAttribute(int Id);

        /// <summary>
        /// Update product variant attribute display order
        /// </summary>
        /// <param name="pvaIds"></param>
        void UpdateProductVariantAttributeDisplayOrder(int[] pvaIds);

        /// <summary>
        /// Get all discounts for products by Id
        /// </summary>
        List<Orbio.Core.Domain.Discounts.Discount> GetAllDiscountsForProductsById(int id);

        /// <summary>
        /// GetProductVariantAttributeByProductVariantAttributeId
        /// </summary>
        /// <param name="productVariantAttributeId"></param>
        /// <returns></returns>
        List<Product_ProductAttribute_Mapping> GetProductVariantAttributeProductVariantAttributeId(int productVariantAttributeId);
    }
}
