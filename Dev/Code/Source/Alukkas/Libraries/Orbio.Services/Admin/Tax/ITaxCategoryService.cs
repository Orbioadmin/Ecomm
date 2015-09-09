using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Tax
{
    public partial interface ITaxCategoryService
    {
        //Get all tax categories
        List<TaxCategory> GetAllTaxCategories();
    }
}
