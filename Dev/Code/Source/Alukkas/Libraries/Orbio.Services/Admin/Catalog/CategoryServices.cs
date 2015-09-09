using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
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
                                                    where c.Deleted == false
                                                    select new CategoryList()
                                                    {
                                                        Id = c.Id,
                                                        Name = c.Name,
                                                        ParentCategory = c.ParentCategoryId,
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
                var model = new CategoryDetails();
                var categoryModel = new CategoryList();

                categoryModel = (from c in context.Categories.AsQueryable()
                                 join pic in context.Pictures.AsQueryable() on c.PictureId equals pic.Id into temp
                                 from j in temp.DefaultIfEmpty()
                                 join url in context.UrlRecords.AsQueryable() on c.Id equals url.EntityId into tempurl
                                 from u in tempurl.DefaultIfEmpty()
                                 where c.Id == Id && u.EntityName == "Category"
                                 select new CategoryList()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     Description = c.Description,
                                     MetaKeyWords = c.MetaDescription,
                                     MetaDescription = c.MetaDescription,
                                     ParentCategory = c.ParentCategoryId,
                                     CategoryTemplate = c.CategoryTemplateId,
                                     MetaTitle = c.MetaTitle,
                                     Picture = j.RelativeUrl,
                                     ShowOnHomePage = c.ShowOnHomePage,
                                     Published = c.Published,
                                     DisplayOrder = c.DisplayOrder,
                                     SearchEngine = u.Slug,
                                 }).FirstOrDefault();

                categoryModel.ParentCategoryList = (from c in context.Categories.AsQueryable()
                                                    where c.Deleted == false
                                                    select new CategoryList()
                                                    {
                                                        Id = c.Id,
                                                        Name = c.Name,
                                                        ParentCategory = c.ParentCategoryId,
                                                    }).ToList();

                categoryModel.CategoryTemplateList = (from t in context.CategoryTemplates.AsQueryable()
                                                      select new Templates()
                                                      {
                                                          Id = t.Id,
                                                          Name = t.Name,
                                                      }).ToList();

                model.Categories = categoryModel;

                model.Products = (from p in context.Products.AsQueryable()
                                  join pc in context.Product_Category_Mapping.AsQueryable() on p.Id equals pc.ProductId
                                  where pc.CategoryId == Id && p.Deleted == false
                                  select new ProductDetails()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      IsFeaturedProduct = pc.IsFeaturedProduct,
                                      DisplayOrder = p.DisplayOrder,
                                  }).ToList();

                //model.Discount = (from d in context.Discounts.AsQueryable()
                //                  join dc in context.Discount_AppliedToCategories.AsQueryable() on d.Id equals dc.Discount_Id
                //                  where dc.Category_Id == Id
                //                  select new DiscountDetails()
                //                  {
                //                      Id = d.Id,
                //                      Name = d.Name,
                //                  }).ToList();
                return model;
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
                            category.CategoryTemplateId = model.CategoryTemplate;
                            category.MetaKeywords = model.MetaKeyWords;
                            category.MetaDescription = model.MetaDescription;
                            category.MetaTitle = model.MetaTitle;
                            category.ParentCategoryId = model.ParentCategory;
                            // category.PictureId=model.Picture;
                            category.ShowOnHomePage = model.ShowOnHomePage;
                            category.SubjectToAcl = model.SubjectToACL;
                            category.Published = model.Published;
                            category.Deleted = false;
                            category.DisplayOrder = model.DisplayOrder;
                            category.UpdatedOnUtc = DateTime.Now;
                            context.SaveChanges();
                            var UrlRecord = context.UrlRecords.Where(m => m.EntityId == model.Id && m.EntityName == "Category").FirstOrDefault();
                            if (UrlRecord != null)
                            {
                                UrlRecord.Slug = model.SearchEngine;
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var category = context.Categories.FirstOrDefault();
                        category.Name = model.Name;
                        category.Description = model.Description;
                        category.CategoryTemplateId = model.CategoryTemplate;
                        category.MetaKeywords = model.MetaKeyWords;
                        category.MetaDescription = model.MetaDescription;
                        category.MetaTitle = model.MetaTitle;
                        category.ParentCategoryId = model.ParentCategory;
                        // category.PictureId=model.Picture;
                        category.ShowOnHomePage = model.ShowOnHomePage;
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
                            UrlRecord.Slug = model.SearchEngine;
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
    }
}
