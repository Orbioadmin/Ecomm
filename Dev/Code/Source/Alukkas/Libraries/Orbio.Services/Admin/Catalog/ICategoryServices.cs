using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Catalog
{
   public interface ICategoryServices
    {
       CategoryDetails GetCategoryDetailsById(int Id);

       CategoryDetails GetCategoryDetails();

       int DeleteCategory(int Id);

       int AddOrUpdateCategory(CategoryList model);

       //int DeleteProductMapping(int CategoryId, int ProductId);
    }
}
