using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Components
{
    public class PriceComponentService : IPriceComponentService
    { 
        
        #region Fields
        private readonly OrbioAdminContext context = new OrbioAdminContext();
        #endregion

        public List<PriceComponent> GetPriceComponent()
        {
            var model = context.PriceComponents.Where(m => m.Deleted == false).ToList();
            return model;
        }

        public PriceComponent GetPriceComponentById(int Id)
        {
            var model = context.PriceComponents.Where(m => m.Id == Id).FirstOrDefault();
            return model;
        }

        public int AddOrUpdatePriceComponent(int Id, string Name, bool IsActive, string Email)
        {
            try
            {
                var result = context.PriceComponents.Where(m => m.Id == Id).FirstOrDefault();
                if (result != null)
                {
                    result.Name = Name;
                    result.IsActive = IsActive;
                    result.Deleted = false;
                    result.ModifiedBy = Email;
                    result.ModifiedDate = DateTime.Now;
                    context.SaveChanges();
                }
                else
                {
                    var priceComp = context.PriceComponents.FirstOrDefault();
                    priceComp.Name = Name;
                    priceComp.IsActive = IsActive;
                    priceComp.Deleted = false;
                    priceComp.CreatedBy = Email;
                    priceComp.CreatedDate = DateTime.Now;
                    priceComp.ModifiedBy = Email;
                    priceComp.ModifiedDate = DateTime.Now;
                    context.PriceComponents.Add(priceComp);
                    context.SaveChanges();
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int DeletePriceComponent(int Id,string Email)
        {
            try
            {
                var model = context.PriceComponents.Where(m => m.Id == Id).FirstOrDefault();
                if (model != null)
                {
                    model.Deleted = true;
                    model.ModifiedBy = Email;
                    model.ModifiedDate = DateTime.Now;
                    context.SaveChanges();
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
