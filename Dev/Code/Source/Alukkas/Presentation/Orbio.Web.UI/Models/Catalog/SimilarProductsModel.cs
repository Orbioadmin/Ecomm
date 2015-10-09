using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Web.UI.Models.Catalog
{
    public class SimilarProductsModel
    {
        public SimilarProductsModel()
        {
            this.ProductDetail = new List<ProductDetailModel>();
        }
        public SimilarProductsModel(SimilarProduct similarProduct)
            : this()
        {
            if (similarProduct.ProductDetails != null && similarProduct.ProductDetails.Count > 0)
            {
                this.ProductDetail = (from p in similarProduct.ProductDetails
                                      select new ProductDetailModel(p)).ToList();
            }
        }
        public List<ProductDetailModel> ProductDetail { get; private set; }
    }
}