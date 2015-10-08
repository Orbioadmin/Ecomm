using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipping
{
    public class ShippingMethodService : IShippingMethodService
    {
        public  List<ShippingMethod> GetAllShippingMethods()
        {
            using(var context= new OrbioAdminContext())
            {
                var result = context.ShippingMethods.OrderBy(m=>m.DisplayOrder).ToList();
                return result;
            }
        }

        public void AddOrUpdate(int id, string name, string description, int displayOrder)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ShippingMethods.Where(m => m.Id == id).FirstOrDefault();
                if(result!=null)
                {
                    result.Name = name;
                    result.Description = description;
                    result.DisplayOrder = displayOrder;
                    context.SaveChanges();
                }
                else
                {
                    result = new ShippingMethod();
                    result.Name = name;
                    result.Description = description;
                    result.DisplayOrder = displayOrder;
                    context.ShippingMethods.Add(result);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ShippingMethods.Where(m => m.Id == id).FirstOrDefault();
                  if (result != null)
                  {
                      context.ShippingMethods.Remove(result);
                      context.SaveChanges();
                  }
            }
        }
    }
}
                      
                  
