using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Catalog
{
    public class ManufacturerService : IManufacturerService
    {

        /// <summary>
        /// getting all manufacturer list
        /// </summary>
        /// <returns></returns>
        public Manufacturers GetAllManufacturers()
        {
            using (var context = new OrbioAdminContext())
            {
                var model = new Manufacturers();
                model.ManufacturerList = (from m in context.Manufacturers.AsQueryable()
                                          join url in context.UrlRecords.AsQueryable()
                                          on m.Id equals url.EntityId
                                          where url.EntityName == "Manufacturer" && m.Deleted == false
                                          select new ManufacturerDetails()
                                          {
                                              Id = m.Id,
                                              Name = m.Name,
                                              Published = m.Published,
                                              DisplayOrder = m.DisplayOrder,
                                          }).ToList();
                return model;
            }
        }


        /// <summary>
        /// searching manufacturer by name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public Manufacturers SearchManufacturerByName(string Name)
        {
            using (var context = new OrbioAdminContext())
            {
                var model = new Manufacturers();
                model.ManufacturerList = (from m in context.Manufacturers.AsQueryable()
                                          join url in context.UrlRecords.AsQueryable()
                                          on m.Id equals url.EntityId
                                          where url.EntityName == "Manufacturer" && m.Deleted == false && m.Name.Contains(Name)
                                          select new ManufacturerDetails()
                                          {
                                              Id = m.Id,
                                              Name = m.Name,
                                              Published = m.Published,
                                              DisplayOrder = m.DisplayOrder,
                                          }).ToList();
                return model;
            }
        }

        /// <summary>
        /// adding or updating manufacturer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddOrUpdateManufacturer(ManufacturerDetails model)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    if (model.Id != 0)
                    {
                        var manufacturers = context.Manufacturers.Where(m => m.Id == model.Id).FirstOrDefault();
                        if (manufacturers != null)
                        {
                            manufacturers.Name = model.Name;
                            manufacturers.Description = model.Description;
                            manufacturers.ManufacturerTemplateId = model.ManufacturerTemplate;
                            manufacturers.MetaKeywords = model.MetaKeyWords;
                            manufacturers.MetaDescription = model.MetaDescription;
                            manufacturers.MetaTitle = model.MetaTitle;
                            // category.PictureId=model.Picture;
                            manufacturers.SubjectToAcl = model.SubjectToACL;
                            manufacturers.Published = model.Published;
                            manufacturers.Deleted = false;
                            manufacturers.DisplayOrder = model.DisplayOrder;
                            manufacturers.UpdatedOnUtc = DateTime.Now;
                            context.SaveChanges();


                            var UrlRecord = context.UrlRecords.Where(m => m.EntityId == model.Id && m.EntityName == "Manufacturer").FirstOrDefault();
                            if (UrlRecord != null)
                            {
                                UrlRecord.Slug = model.SearchEngine;
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var manufacturers = context.Manufacturers.FirstOrDefault();
                        manufacturers.Name = model.Name;
                        manufacturers.Description = model.Description;
                        manufacturers.ManufacturerTemplateId = model.ManufacturerTemplate;
                        manufacturers.MetaKeywords = model.MetaKeyWords;
                        manufacturers.MetaDescription = model.MetaDescription;
                        manufacturers.MetaTitle = model.MetaTitle;
                        // category.PictureId=model.Picture;
                        manufacturers.SubjectToAcl = model.SubjectToACL;
                        manufacturers.Published = model.Published;
                        manufacturers.Deleted = false;
                        manufacturers.DisplayOrder = model.DisplayOrder;
                        manufacturers.CreatedOnUtc = DateTime.Now;
                        manufacturers.UpdatedOnUtc = DateTime.Now;

                        context.Manufacturers.Add(manufacturers);
                        context.SaveChanges();
                        int Id = manufacturers.Id;

                        var UrlRecord = context.UrlRecords.Where(m => m.EntityName == "Manufacturer").FirstOrDefault();
                        if (UrlRecord != null)
                        {

                            UrlRecord.EntityId = Id;
                            UrlRecord.EntityName = "Manufacturer";
                            UrlRecord.Slug = model.SearchEngine;
                            UrlRecord.IsActive = true;
                            UrlRecord.LanguageId = 0;
                            context.UrlRecords.Add(UrlRecord);
                            context.SaveChanges();
                        }
                        //UrlRecord.EntityId = Id;
                        //UrlRecord.EntityName = "Manufacturer";
                        //UrlRecord.Slug = model.SearchEngine;
                        //UrlRecord.IsActive = true;
                        //UrlRecord.LanguageId = 0;

                        //if (UrlRecord != null)
                        //{ 
                        //    context.SaveChanges(); 
                        //}
                        //else
                        //{
                        //    context.UrlRecords.Add(UrlRecord);
                        //    context.SaveChanges();
                        //}
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
        /// deleting manufacturer
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteManufacturer(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var model = new ManufacturerDetails();
                    var manufacturer = context.Manufacturers.Where(m => m.Id == Id).FirstOrDefault();
                    if (manufacturer != null)
                    {
                        manufacturer.Deleted = true;
                        context.SaveChanges();

                        var manuMap = context.Product_Manufacturer_Mapping.Where(m => m.ManufacturerId == Id).ToList();
                        if (manuMap != null && manuMap.Count > 0)
                        {
                            foreach (var prod in manuMap)
                            {
                                context.Product_Manufacturer_Mapping.Remove(prod);
                                context.SaveChanges();
                            }
                        }
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
        /// getting manufacturer by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Manufacturers GetManufacturerDetailsById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var model = new Manufacturers();
                var manufacturerModel = new ManufacturerDetails();

                manufacturerModel = (from m in context.Manufacturers.AsQueryable()
                                     join pic in context.Pictures.AsQueryable() on m.PictureId equals pic.Id into temp
                                     from j in temp.DefaultIfEmpty()
                                     join url in context.UrlRecords.AsQueryable() on m.Id equals url.EntityId into tempurl
                                     from u in tempurl.DefaultIfEmpty()
                                     where m.Id == Id && u.EntityName == "Manufacturer"
                                     select new ManufacturerDetails()
                                     {
                                         Id = m.Id,
                                         Name = m.Name,
                                         Description = m.Description,
                                         MetaKeyWords = m.MetaDescription,
                                         MetaDescription = m.MetaDescription,
                                         ManufacturerTemplate = m.ManufacturerTemplateId,
                                         MetaTitle = m.MetaTitle,
                                         Picture = j.RelativeUrl,
                                         Published = m.Published,
                                         DisplayOrder = m.DisplayOrder,
                                         SubjectToACL = m.SubjectToAcl,
                                         SearchEngine = u.Slug,
                                     }).FirstOrDefault();

                manufacturerModel.ManufacturerTemplates = (from t in context.ManufacturerTemplates.AsQueryable()
                                                           select new Templates()
                                                           {
                                                               Id = t.Id,
                                                               Name = t.Name,
                                                           }).ToList();

                model.Manufacturer = manufacturerModel;

                model.Products = (from p in context.Products.AsQueryable()
                                  join pc in context.Product_Manufacturer_Mapping.AsQueryable() on p.Id equals pc.ProductId
                                  where pc.ManufacturerId == Id && p.Deleted == false
                                  select new ProductDetails()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      IsFeaturedProduct = pc.IsFeaturedProduct,
                                      DisplayOrder = p.DisplayOrder,
                                  }).ToList();

                return model;
            }
        }

        /// <summary>
        /// getting manufacturer template
        /// </summary>
        /// <returns></returns>
        public Manufacturers GetManufacturerDetails()
        {
            using (var context = new OrbioAdminContext())
            {
                var model = new ManufacturerDetails();
                var result = new Manufacturers();
                model.ManufacturerTemplates = (from t in context.ManufacturerTemplates.AsQueryable()
                                               select new Templates()
                                               {
                                                   Id = t.Id,
                                                   Name = t.Name,
                                               }).ToList();
                result.Manufacturer = model;
                return result;
            }
        }


        /// <summary>
        /// Deleteing manufacture product
        /// </summary>
        /// <param name="manufacturerId">Manufacture Id</param>
        /// <param name="productId">Product Id</param>
        public void DeleteManufacturerProduct(int manufacturerId, int productId)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var product = context.Product_Manufacturer_Mapping.Where(m => m.ManufacturerId == manufacturerId && m.ProductId == productId).FirstOrDefault();
                    if (product != null)
                    {
                        context.Product_Manufacturer_Mapping.Remove(product);
                        context.SaveChanges();
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        public List<Product_Manufacturer_Mapping> GetManufacturerProducts(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Product_Manufacturer_Mapping.Include("Product").Where(m => m.ManufacturerId == Id && !m.Product.Deleted).ToList();
                return result;
            }
        }
    }
}
