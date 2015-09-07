using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Components
{
    public class ProductComponentService:IProductComponentService
    {

        public List<ProductComponent> GetProductComponent()
        {
            using (var context = new OrbioAdminContext())
            {
                var model = context.ProductComponents.Where(m => m.Deleted == false).ToList();
                return model;
            }
        }

        public ProductComponent GetProductComponentById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var model = context.ProductComponents.Where(m => m.Id == Id).FirstOrDefault();
                return model;
            }
        }

        public int AddOrUpdateProductComponent(int Id, string Name, bool IsActive, string Email)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.ProductComponents.Where(m => m.Id == Id).FirstOrDefault();
                    if (result != null)
                    {
                        result.ComponentName = Name;
                        result.IsActive = IsActive;
                        result.Deleted = false;
                        result.ModifiedBy = Email;
                        result.ModifiedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                    else
                    {
                        var prodComp = context.ProductComponents.FirstOrDefault();
                        prodComp.ComponentName = Name;
                        prodComp.IsActive = IsActive;
                        prodComp.Deleted = false;
                        prodComp.CreatedBy = Email;
                        prodComp.CreatedDate = DateTime.Now;
                        prodComp.ModifiedBy = Email;
                        prodComp.ModifiedDate = DateTime.Now;
                        context.ProductComponents.Add(prodComp);
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

        public int DeleteProductComponent(int Id, string Email)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var model = context.ProductComponents.Where(m => m.Id == Id).FirstOrDefault();
                    if (model != null)
                    {
                        model.Deleted = true;
                        model.ModifiedBy = Email;
                        model.ModifiedDate = DateTime.Now;
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
