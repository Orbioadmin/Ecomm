using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Catalog
{
    public class CategoryList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SeName { get; set; }

        public string Description { get; set; }

        public int CategoryTemplate { get; set; }

        public int ParentCategory { get; set; }

        public string ParentCategoryName { get; set; }

        public string Picture { get; set; }

        public bool ShowOnHomePage { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public string MetaKeyWords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public string SearchEngine { get; set; }

        public bool SubjectToACL { get; set; }

        public List<string> SelectedProducts { get; set; }

        public List<CategoryList> ParentCategoryList { get; set; }

        public List<Templates> CategoryTemplateList { get; set; }
    }
}
