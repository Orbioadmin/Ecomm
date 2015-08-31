using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public interface IProductAttributeService
    {
     List<ProductAttribute> GetProductAttributes();

     ProductAttribute GetProductAttributeById(int Id);

     void AddOrUpdateProductAttribute(int Id, string Name, string Description);

     int DeleteProductAttribute(int Id);
    }
}
