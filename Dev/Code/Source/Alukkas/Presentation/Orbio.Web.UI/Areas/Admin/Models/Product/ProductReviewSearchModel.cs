using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Product
{
    public class ProductReviewSearchModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Message { get; set; }
    }
}