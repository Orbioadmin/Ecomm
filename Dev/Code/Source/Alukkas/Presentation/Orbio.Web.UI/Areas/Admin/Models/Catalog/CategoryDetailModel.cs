using Orbio.Core.Domain.Admin.Catalog;
using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class CategoryDetailModel
    {
        public CategoryDetailModel()
        {
            ParentCategories = new List<CategoryModel>();
        }

        public CategoryDetailModel(CategoryDetails result)
        {
            Categories = new CategoryModel(result);

            if (result.Products != null)
            {
                Products = (from c in result.Products
                            select new ProductModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                IsFeaturedProduct=c.IsFeaturedProduct,
                                DisplayOrder=c.DisplayOrder,
                            }).ToList();
            }

            if (result.Discount != null)
            {
                Discount = (from c in result.Discount
                            select new DiscountModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                            }).ToList();
            }

        }


        public CategoryModel Categories { get; set; }

        public List<ProductModel> Products { get; set; }

        public List<DiscountModel> Discount { get; set; }

        public IList<CategoryModel> ParentCategories { get; set; }
    }
}