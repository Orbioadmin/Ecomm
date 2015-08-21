using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public class ProductAttributeService : IProductAttributeService
    {
        #region Fields
        private readonly OrbioAdminContext context = new OrbioAdminContext();
        #endregion

        public List<ProductAttribute> GetProductAttributes()
        {
            var model = context.ProductAttributes.ToList();
            return model;               
        }
    }
}
