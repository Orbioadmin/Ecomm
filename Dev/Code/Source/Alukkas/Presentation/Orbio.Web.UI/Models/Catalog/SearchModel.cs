using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Domain.Catalog;
namespace Orbio.Web.UI.Models.Catalog
{
    public partial class SearchModel
    {
        public SearchModel()
        {
            Categories = new List<CategorySimpleModel>();
            this.Products = new List<ProductOverViewModel>();
        }

       
        public SearchModel(Search SearchProduct)
            : this()
        {
            this.CategoryId = SearchProduct.CategoryId;
            this.Totalcount = SearchProduct.Totalcount + "  Items Found";
            if (SearchProduct.Products != null && SearchProduct.Products.Count > 0)
            {
                this.Products = (from p in SearchProduct.Products
                                 select new ProductOverViewModel(p)).ToList();
            }
        }

        public string Totalcount { get; set; }
        public string CategoryId { get; set; }
        public IList<ProductOverViewModel> Products { get; set; }
        public IList<CategorySimpleModel> Categories { get; set; }
        public int[] SelectedSpecificationAttributeIds { get; set; }
        public string SelectedPriceRange { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}