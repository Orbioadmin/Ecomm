using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Catalog
{
    public class Manufacturers
    {
        public ManufacturerDetails Manufacturer { get; set; }

        public List<ManufacturerDetails> ManufacturerList { get; set; }

        public List<ProductDetails> Products { get; set; }
    }
}
