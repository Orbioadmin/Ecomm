using System.Collections.Generic;
using System.Linq;
using Nop.Web.Framework.Mvc;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Web.UI.Models.Catalog
{
    public class CategorySimpleModel : BaseCatalogModel
    {
        public CategorySimpleModel()
        {
            SubCategories = new List<CategorySimpleModel>();
        }

        public CategorySimpleModel(Category category)
            : this()
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.SeName = category.SeName;
          
            if (category.SubCategories != null && category.SubCategories.Count > 0)
            {
                this.SubCategories.AddRange( (from c in category.SubCategories
                                     select new CategorySimpleModel(c)).ToList());
            }

        }

      
        public int? NumberOfProducts { get; set; }

        public List<CategorySimpleModel> SubCategories { get; set; }

       
    }
}