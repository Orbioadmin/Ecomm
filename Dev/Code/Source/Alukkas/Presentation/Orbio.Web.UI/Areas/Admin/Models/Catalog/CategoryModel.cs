using Orbio.Core.Domain.Admin.Catalog;
using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class CategoryModel
    {
        public CategoryModel()
        {
            CategoryList = new List<CategoryModel>();
            CategoryTemplateList = new List<TemplateModel>();
        }

        public CategoryModel(Category category)
            : this()
        {
            this.Id = category.Id;
            this.Name = category.Name;
            // this.SeName = category.SeName;

            if (category.SubCategories != null && category.SubCategories.Count > 0)
            {
                CategoryList.AddRange((from c in category.SubCategories
                                             select new CategoryModel(c)).ToList());
            }
        }
        public CategoryModel(CategoryDetails result)
        {
            Id = result.Categories.Id;
            Name = result.Categories.Name;
            Description = result.Categories.Description;
            ShowOnHomePage = result.Categories.ShowOnHomePage;
            Published = result.Categories.Published;
            DisplayOrder = result.Categories.DisplayOrder;
            PictureId = result.Categories.PictureId;
            PictureUrl = result.Categories.PictureUrl;
            MetaKeyWords = result.Categories.MetaKeywords;
            MetaDescription = result.Categories.MetaDescription;
            MetaTitle = result.Categories.MetaTitle;
            SearchEngine = result.Categories.Slug;
            SubjectToACL = result.Categories.SubjectToACL;
            ParentCategory = result.Categories.ParentCategoryId;
            CategoryTemplate = result.Categories.CategoryTemplateId;

            CategoryList = (from c in result.Categories.ParentCategoryList
                                  select new CategoryModel
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                      ParentCategory = c.ParentCategoryId,
                                  }).ToList();

            CategoryTemplateList = (from c in result.Categories.CategoryTemplateList
                                    select new TemplateModel
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                    }).ToList();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public int CategoryTemplate { get; set; }

        public string ParentCategoryName { get; set; }

        public int PictureId { get; set; }

        public string PictureUrl { get; set; }

        public bool ShowOnHomePage { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public string MetaKeyWords { get; set; }

        public int ParentCategory { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public string SearchEngine { get; set; }

        public bool AllowCustomer { get; set; }

        public bool SubjectToACL { get; set; }

        public List<CategoryModel> CategoryList { get; set; } 

        public List<TemplateModel> CategoryTemplateList { get; set; }
    }
}