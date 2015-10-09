using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Catalog
{
    public class CategoryServices : ICategoryServices
    {

        private readonly IDbContext dbContext;

        /// <summary>
        /// instantiates Category service type
        /// </summary>
        /// <param name="context">db context</param>
        public CategoryServices(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// get all top menu categories
        /// </summary>
        /// <returns></returns>
        public List<Orbio.Core.Domain.Catalog.Category> GetTopMenuCategories()
        {
            using (var context = new OrbioAdminContext())
            {
                //var category = new List<Orbio.Core.Domain.Catalog.Category>
                var category = (from c in context.Categories
                                join url in context.UrlRecords on c.Id equals url.EntityId
                                where url.EntityName == "Category" && !c.Deleted
                                && c.ParentCategoryId == 0 && url.LanguageId==0 && url.IsActive
                                select new Orbio.Core.Domain.Catalog.Category()
                                {
                                    Id=c.Id,
                                    Name=c.Name,
                                    Description=c.Description,
                                    ParentCategoryId=c.ParentCategoryId,
                                    SeName=url.Slug,
                                }).ToList();
                if (category != null && category.Count > 0)
                {
                    foreach (var item in category)
                    {
                        item.SubCategories = SubCategories(item.Id);
                    }
                }
                return category;
            }
        }

        public List<Orbio.Core.Domain.Catalog.Category> SubCategories(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var category = (from c in context.Categories
                                join url in context.UrlRecords on c.Id equals url.EntityId
                                where url.EntityName == "Category" && !c.Deleted
                                && c.ParentCategoryId == id
                                && url.LanguageId == 0 && url.IsActive
                                select new Orbio.Core.Domain.Catalog.Category()
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Description = c.Description,
                                    ParentCategoryId = c.ParentCategoryId,
                                    SeName = url.Slug,
                                }).ToList();
                if (category != null && category.Count > 0)
                {
                    foreach (var item in category)
                    {
                        item.SubCategories = SubCategories(item.Id);
                    }
                }
                return category;
            }
        }

        /// <summary>
        /// getting category details
        /// </summary>
        /// <returns></returns>
        public CategoryDetails GetCategoryDetails()
        {
            using (var context = new OrbioAdminContext())
            {
                var model = new CategoryDetails();
                var categoryModel = new CategoryList();

                categoryModel.ParentCategoryList = (from c in context.Categories.AsQueryable()
                                                    join url in context.UrlRecords on c.Id equals url.EntityId
                                                    where url.EntityName == "Category" && !c.Deleted
                                                    && url.LanguageId == 0 && url.IsActive
                                                    select new CategoryList()
                                                    {
                                                        Id = c.Id,
                                                        Name = c.Name,
                                                        ParentCategoryId = c.ParentCategoryId,
                                                    }).ToList();

                categoryModel.CategoryTemplateList = (from t in context.CategoryTemplates.AsQueryable()
                                                      select new Templates()
                                                      {
                                                          Id = t.Id,
                                                          Name = t.Name,
                                                      }).ToList();
                model.Categories = categoryModel;

                return model;
            }
        }

        /// <summary>
        /// getting category details by passing id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CategoryDetails GetCategoryDetailsById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
               var result = dbContext.ExecuteFunction<XmlResultSet>("usp_Catalog_GetCategoryDetails",
               new SqlParameter() { ParameterName = "@id", Value = Id, DbType = System.Data.DbType.Int32 }).FirstOrDefault();

                var categoryDetails = Serializer.GenericDeSerializer<CategoryDetails>(result.XmlResult);
                return categoryDetails;
            }
        }

        /// <summary>
        /// adding and updating new category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddOrUpdateCategory(CategoryList model)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    if (model.Id != 0)
                    {
                        var category = context.Categories.Where(m => m.Id == model.Id).FirstOrDefault();
                        if (category != null)
                        {
                            category.Name = model.Name;
                            category.Description = model.Description;
                            category.CategoryTemplateId = model.CategoryTemplateId;
                            category.MetaKeywords = model.MetaKeywords;
                            category.MetaDescription = model.MetaDescription;
                            category.MetaTitle = model.MetaTitle;
                            category.ParentCategoryId = model.ParentCategoryId;
                            category.PictureId = (model.PictureId != null) ? Convert.ToInt32(model.PictureId) : 0;
                            category.ShowOnHomePage = model.ShowOnHomePage;
                            category.IncludeInTopMenu = model.ShowOnHomePage;
                            category.SubjectToAcl = model.SubjectToACL;
                            category.Published = model.Published;
                            category.Deleted = false;
                            category.DisplayOrder = model.DisplayOrder;
                            category.UpdatedOnUtc = DateTime.Now;
                            context.SaveChanges();
                            var UrlRecord = context.UrlRecords.Where(m => m.EntityId == model.Id && m.EntityName == "Category").FirstOrDefault();
                            if (UrlRecord != null)
                            {
                                UrlRecord.Slug = model.Slug;
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var category = context.Categories.FirstOrDefault();
                        category.Name = model.Name;
                        category.Description = model.Description;
                        category.CategoryTemplateId = model.CategoryTemplateId;
                        category.MetaKeywords = model.MetaKeywords;
                        category.MetaDescription = model.MetaDescription;
                        category.MetaTitle = model.MetaTitle;
                        category.ParentCategoryId = model.ParentCategoryId;
                        category.PictureId = (model.PictureId != null) ? Convert.ToInt32(model.PictureId) : 0;
                        category.ShowOnHomePage = model.ShowOnHomePage;
                        category.IncludeInTopMenu = model.ShowOnHomePage;
                        category.SubjectToAcl = model.SubjectToACL;
                        category.Published = model.Published;
                        category.Deleted = false;
                        category.DisplayOrder = model.DisplayOrder;
                        category.CreatedOnUtc = DateTime.Now;
                        category.UpdatedOnUtc = DateTime.Now;

                        context.Categories.Add(category);
                        context.SaveChanges();
                        int Id = category.Id;
                        var UrlRecord = context.UrlRecords.Where(m => m.EntityName == "Category").FirstOrDefault();
                        if (UrlRecord != null)
                        {

                            UrlRecord.EntityId = Id;
                            UrlRecord.EntityName = "Category";
                            UrlRecord.Slug = model.Slug;
                            UrlRecord.IsActive = true;
                            UrlRecord.LanguageId = 0;
                            context.UrlRecords.Add(UrlRecord);
                            context.SaveChanges();
                        }
                        return Id;
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }

        /// <summary>
        /// deleting a category
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteCategory(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var model = new CategoryList();
                    var category = context.Categories.Where(m => m.Id == Id).FirstOrDefault();
                    if (category != null)
                    {
                        category.Deleted = true;
                        context.SaveChanges();

                        var subCategories = SubCategories(Id);
                        if(subCategories!=null && subCategories.Count>0)
                        {
                           DeleteSubCategories(subCategories);
                        }

                        DeleteCategoryMapping(Id);
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Delete Category Product From Category Product Mapping Table
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="productId"></param>
        public void DeleteCategoryProduct(int categoryId, int productId)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var category = context.Product_Category_Mapping.Where(m => m.CategoryId == categoryId && m.ProductId == productId).FirstOrDefault();
                    if (category != null)
                    {
                        context.Product_Category_Mapping.Remove(category);
                        context.SaveChanges();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public List<Product_Category_Mapping> GetCategoryProducts(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Product_Category_Mapping.Include("Product").Where(m => m.CategoryId == Id && !m.Product.Deleted).ToList();
                return result;
            }
        }

        /// <summary>
        /// deleting mapping products to category
        /// </summary>
        /// <param name="Id"></param>
        public void DeleteCategoryMapping(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var categoryMap = context.Product_Category_Mapping.Where(m => m.CategoryId == Id).ToList();
                if (categoryMap != null && categoryMap.Count > 0)
                {
                    foreach (var prod in categoryMap)
                    {
                        context.Product_Category_Mapping.Remove(prod);
                        context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// deleting subcategories
        /// </summary>
        /// <param name="subCategories"></param>
        public void DeleteSubCategories(List<Orbio.Core.Domain.Catalog.Category> subCategories)
        {
            using (var context = new OrbioAdminContext())
            {
                foreach (var item in subCategories)
                {
                    var category = context.Categories.Where(m => m.Id == item.Id).FirstOrDefault();
                    if (category != null)
                    {
                        category.Deleted = true;
                        context.SaveChanges();

                        DeleteCategoryMapping(item.Id);
                    }
                }
            }
        }

    }
}
