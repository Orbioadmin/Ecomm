using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Data;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class ProductModel
    {
        public ProductModel()
        {

        }
        public ProductModel(Product_Category_Mapping prod)
        {
            Id = prod.ProductId;
            Name = prod.Product.Name;
            IsFeaturedProduct = prod.IsFeaturedProduct;
            DisplayOrder = prod.DisplayOrder;
            CategoryId = prod.CategoryId;
        }

        public ProductModel(Product_Manufacturer_Mapping prod)
        {
            Id = prod.ProductId;
            Name = prod.Product.Name;
            IsFeaturedProduct = prod.IsFeaturedProduct;
            DisplayOrder = prod.DisplayOrder;
            ManufacturerId = prod.ManufacturerId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsFeaturedProduct { get; set; }

        public int DisplayOrder { get; set; }

        public int CategoryId { get; set; }

        public int ManufacturerId { get; set; }
    }
}