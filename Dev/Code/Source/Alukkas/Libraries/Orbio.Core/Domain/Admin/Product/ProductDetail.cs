using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Data;
namespace Orbio.Core.Domain.Admin.Product
{
    public partial class ProductDetail
    {
        public Orbio.Core.Domain.Admin.Product.Product product { get; set; }

        public string seName { get; set; }

        public int[] productTags { get; set; }

        public int[] catgoryIds { get; set; }

        public int[] manufactureIds { get; set; }

        public int[] relatedProductIds { get; set; }

        public int[] similarProductIds { get; set; }

        public int[] discountIds { get; set; }
    }
}
