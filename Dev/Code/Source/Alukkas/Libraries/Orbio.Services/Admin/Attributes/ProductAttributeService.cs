using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public class ProductAttributeService : IProductAttributeService
    {
        public List<ProductAttribute> GetProductAttributes()
        {
            using (var context = new OrbioAdminContext())
            {
                var model = context.ProductAttributes.ToList();
                return model;
            }
        }

        public ProductAttribute GetProductAttributeById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var model = context.ProductAttributes.Where(m => m.Id == Id).FirstOrDefault();
                return model;
            }
        }

        public void AddOrUpdateProductAttribute(int Id, string Name, string Description)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductAttributes.Where(m => m.Id == Id).FirstOrDefault();
                if (result != null)
                {
                    result.Name = Name;
                    result.Description = Description;
                    context.SaveChanges();
                }
                else
                {
                    var productAttribute = context.ProductAttributes.FirstOrDefault();
                    productAttribute.Name = Name;
                    productAttribute.Description = Description;
                    context.ProductAttributes.Add(productAttribute);
                    context.SaveChanges();
                }
            }
        }

        public int DeleteProductAttribute(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.ProductAttributes.Where(m => m.Id == Id).FirstOrDefault();
                    if (result != null)
                    {
                        context.ProductAttributes.Remove(result);
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
    }
}
