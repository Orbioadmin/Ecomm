using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class CheckoutAttributeValueModel
    {
        public int Id { get; set; }

        public int CheckoutAttributeId { get; set; }

        public string Name { get; set; }

        public string ColorSquares { get; set; }

        public string PriceAdjustment { get; set; }

        public string WeightAdjustment { get; set; }

        public bool IsPreSelected { get; set; }

        public int DisplayOrder { get; set; }
    }
}