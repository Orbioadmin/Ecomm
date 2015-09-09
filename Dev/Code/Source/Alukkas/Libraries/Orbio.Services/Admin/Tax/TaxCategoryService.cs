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
                var result = context.TaxCategories.ToList();
                return result;
            }
        }
    }
}
