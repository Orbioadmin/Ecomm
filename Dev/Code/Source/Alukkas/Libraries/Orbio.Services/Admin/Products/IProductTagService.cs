using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Products
{
    public interface IProductTagService
    {
        List<ProductTag> GetAllProductTags();

        ProductTag GetAllProductTagsById(int id);

        int UpdateProductTags(int id,string name);

        int AddProductTags(string name);

        int DeleteProductTags(int id);
    }
}
