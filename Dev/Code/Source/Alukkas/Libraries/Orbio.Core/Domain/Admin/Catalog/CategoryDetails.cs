using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Catalog
{
    public class CategoryDetails
    {
        public CategoryList Categories { get; set; }

        public List<ProductDetails> Products { get; set; }

        public List<DiscountDetails> Discount { get; set; }
    }
}
