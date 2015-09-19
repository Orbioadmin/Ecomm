using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Orders
{
    public class BestSellers
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public List<CategoryList> Category { get; set; }

        public List<ManufacturerDetails> Manufacturers { get; set; }
    }
}
