using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Catalog
{
    public class ProductDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsFeaturedProduct { get; set; }

        public int DisplayOrder { get; set; }
    }
}
