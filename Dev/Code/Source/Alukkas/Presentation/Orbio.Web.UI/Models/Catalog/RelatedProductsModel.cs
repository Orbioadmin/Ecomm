﻿using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public class RelatedProductsModel
    {
        public RelatedProductsModel()
        {
            this.ProductDetail = new List<ProductDetailModel>();
        }
        public RelatedProductsModel(RelatedProduct relatedProduct)
            : this()
        {
            if (relatedProduct.ProductDetails != null && relatedProduct.ProductDetails.Count > 0)
            {
                this.ProductDetail = (from p in relatedProduct.ProductDetails
                                      select new ProductDetailModel(p)).ToList();
            }
        }
        public List<ProductDetailModel> ProductDetail { get; private set; }
    }
}