using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public class HomePageProductsModel
    {
        public HomePageProductsModel()
        {
            this.ProductDetail = new List<ProductDetailModel>();
        }
        public HomePageProductsModel(HomePageProducts homepageProduct)
            : this()
        {
            if (homepageProduct.ProductDetails != null && homepageProduct.ProductDetails.Count > 0)
            {
                this.ProductDetail = (from p in homepageProduct.ProductDetails
                                      select new ProductDetailModel(p)).ToList();
            }
        }
        public List<ProductDetailModel> ProductDetail { get; private set; }
    }
}