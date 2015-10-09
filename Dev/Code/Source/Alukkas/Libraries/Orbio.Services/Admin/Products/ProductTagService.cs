using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Products
{
    public class ProductTagService : IProductTagService
    {
        public  List<ProductTag> GetAllProductTags()
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductTags.Include("Products").OrderByDescending(m=>m.Id).ToList();
                return result;
            }
        }

        public ProductTag GetAllProductTagsById(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductTags.Include("Products").Where(m => m.Id == id).FirstOrDefault();
                return result;
            }
        }

        public int UpdateProductTags(int id,string name)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    var result = context.ProductTags.Where(m => m.Id == id).FirstOrDefault();
                    if (result != null)
                    {
                        result.Name = name;
                        context.SaveChanges();
                    }
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int AddProductTags(string name)
        {
            try
            {
             using (var context = new OrbioAdminContext())
                {
                    var result = context.ProductTags.FirstOrDefault();
                    if (result != null)
                    {
                        result.Name = name;
                        context.ProductTags.Add(result);
                        context.SaveChanges();
                    }
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int DeleteProductTags(int id)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    var result = context.ProductTags.Where(m => m.Id == id).FirstOrDefault();
                    if (result != null)
                    {
                        context.ProductTags.Remove(result);
                        context.SaveChanges();
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
}
            
