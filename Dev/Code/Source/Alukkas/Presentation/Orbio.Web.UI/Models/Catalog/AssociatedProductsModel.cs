using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public class AssociatedProductsModel
    {
        public AssociatedProductsModel()
        {
            this.ProductDetail = new List<ProductDetailModel>();
        }
        public AssociatedProductsModel(AssociatedProduct associatedProduct)
            : this()
        {
            if (associatedProduct.ProductDetails != null && associatedProduct.ProductDetails.Count > 0)
            {
                this.ProductDetail = (from p in associatedProduct.ProductDetails
                                      select new ProductDetailModel(p)).ToList();
            }
        }
        public List<ProductDetailModel> ProductDetail { get; private set; }
    }
}