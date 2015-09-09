using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public static class CategoryExtension
    {
        /// <summary>
        /// generating parent categories
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="categoryService"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<CategoryModel> GetFormattedBreadCrumb(this List<CategoryModel> categoryList, string separator = ">>")
        {
            if (categoryList == null)
                throw new ArgumentNullException("category");

            var resultList = new List<CategoryModel>();
            foreach (var item in categoryList)
            {
                int Id = item.Id;
                string result = string.Empty;
                var alreadyProcessedCategoryIds = new List<int>() { };
                var category = new CategoryModel();
                while (item != null && //not null
                   !alreadyProcessedCategoryIds.Contains(item.Id)) //prevent circular references
                {
                    if (String.IsNullOrEmpty(result))
                    {
                        result = item.Name;
                    }
                    else
                    {
                        result = string.Format("{0} {1} {2}", item.Name, separator, result);
                        goto ListCategories;
                    }

                    alreadyProcessedCategoryIds.Add(item.Id);

                    category = (from c in resultList
                                where c.Id == item.ParentCategory
                                select c).FirstOrDefault();
                    if (category != null)
                    {
                        item.Id = category.Id;
                        item.Name = category.Name;
                    }
                }
            ListCategories:
                resultList.Add(new CategoryModel()
                {
                    Id = Id,
                    Name = result,
                });
            }
            return resultList;
        }
    }
}