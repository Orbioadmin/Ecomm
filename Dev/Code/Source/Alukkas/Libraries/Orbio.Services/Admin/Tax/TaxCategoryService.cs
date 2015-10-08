using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Tax
{
    public partial class TaxCategoryService : ITaxCategoryService
    {
        //Get all tax categories
        public List<TaxCategory> GetAllTaxCategories()
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.TaxCategories.Include("TaxRates").OrderBy(m=>m.DisplayOrder).ToList();
                return result;
            }
        }

        public void AddOrUpdate(int id, string name, int displayOrder, decimal taxRate)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.TaxCategories.Where(m => m.Id == id).FirstOrDefault();
                if(result!=null)
                {
                    result.Name = name;
                    result.DisplayOrder = displayOrder;
                    context.SaveChanges();

                    var rate = context.TaxRates.Where(m => m.TaxCategoryId == id).FirstOrDefault();
                    if(rate!=null)
                    {
                        rate.Percentage = taxRate;
                        context.SaveChanges();
                    }
                    else
                    {
                        TaxRate taxrate = new TaxRate();
                        taxrate.Percentage = taxRate;
                        taxrate.TaxCategoryId = id;
                        context.TaxRates.Add(taxrate);
                        context.SaveChanges();
                    }
                }
                else
                {
                    TaxCategory catog = new TaxCategory();
                    catog.Name = name;
                    catog.DisplayOrder = displayOrder;
                    context.TaxCategories.Add(catog);
                    context.SaveChanges();

                    TaxRate rate = new TaxRate();
                    rate.TaxCategoryId = catog.Id;
                    rate.Percentage = taxRate;
                    context.TaxRates.Add(rate);
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.TaxCategories.Where(m=>m.Id==id).FirstOrDefault();
                if(result!=null)
                {
                    var taxRate = context.TaxRates.Where(m => m.TaxCategoryId == id).FirstOrDefault();
                    if (taxRate != null)
                    {
                        context.TaxRates.Remove(taxRate);
                        context.SaveChanges();
                    }
                    context.TaxCategories.Remove(result);
                    context.SaveChanges();
                }
            }
        }
    }

    
}
