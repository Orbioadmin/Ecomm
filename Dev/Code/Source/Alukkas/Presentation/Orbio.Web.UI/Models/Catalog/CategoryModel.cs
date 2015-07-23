using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Web.UI.Models.Catalog
{
    public class CategoryModel : BaseCatalogModel
    {
       
        public CategoryModel()
        {           
            this.Products = new List<ProductOverViewModel>();
            this.BreadCrumbs = new List<CategoryModel>();           
        }

        public CategoryModel(CategoryProduct categoryProduct):this()
        {
            this.Id = categoryProduct.CategoryId;
            this.Name = categoryProduct.Name;         
            this.MetaDescription = categoryProduct.MetaDescription;
            this.MetaKeywords = categoryProduct.MetaKeywords;
            this.MetaTitle = categoryProduct.MetaTitle;
            this.SeName = categoryProduct.SeName;
            this.ViewPath = categoryProduct.TemplateViewPath;
            this.PageSize = categoryProduct.PageSize;
            if (categoryProduct.Products != null && categoryProduct.Products.Count > 0)
            {
                this.Products = (from p in categoryProduct.Products
                                   select new ProductOverViewModel(p)).ToList();
            }

            if (categoryProduct.BreadCrumbs != null && categoryProduct.BreadCrumbs.Count > 0)
            {
                this.BreadCrumbs = (from c in categoryProduct.BreadCrumbs
                                    select new CategoryModel { Id= c.Id, Name = c.Name, SeName = c.SeName }).ToList();
            }

            this.TotalProductCount = categoryProduct.TotalProductCount;
        }

       
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public int PageSize { get; set; }
        
        public IList<ProductOverViewModel> Products { get; set; }
        public IList<CategoryModel> BreadCrumbs { get; set; }
        public int[] SelectedSpecificationAttributeIds { get; set; }

        public string SelectedPriceRange { get; set; }

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int TotalProductCount { get; set; }
    }
}