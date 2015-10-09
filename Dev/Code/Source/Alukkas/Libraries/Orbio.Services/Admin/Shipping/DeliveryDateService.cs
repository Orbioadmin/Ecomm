using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipping
{
    public class DeliveryDateService : IDeliveryDateService
    {
       public List<DeliveryDate> GetAllDeliveryDate()
        {
           using(var context=new OrbioAdminContext())
           {
               var result = context.DeliveryDates.OrderBy(m=>m.DisplayOrder).ToList();
               return result;
           }
        }

       public int AddOrUpdate(int id, string name, int displayOrder)
       {
           try
           {
               using (var context = new OrbioAdminContext())
               {
                   var result = context.DeliveryDates.Where(m => m.Id == id).FirstOrDefault();
                   if (result != null)
                   {
                       result.Name = name;
                       result.DisplayOrder = displayOrder;
                       context.SaveChanges();
                   }
                   else
                   {
                       result = context.DeliveryDates.FirstOrDefault();
                       result.Name = name;
                       result.DisplayOrder = displayOrder;
                       context.DeliveryDates.Add(result);
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

       public void Delete(int id)
       {
           using (var context = new OrbioAdminContext())
           {
               var result = context.DeliveryDates.Where(m => m.Id == id).FirstOrDefault();
               if(result!=null)
               {
                   context.DeliveryDates.Remove(result);
                   context.SaveChanges();
               }
           }
       }
    }
}
