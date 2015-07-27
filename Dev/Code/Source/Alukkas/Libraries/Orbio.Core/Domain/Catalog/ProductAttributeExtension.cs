using Orbio.Core.Domain.Catalog.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Orbio.Core.Domain.Catalog
{
    public static class ProductAttributeExtension
    {
        public static string FormatAttribute(this IEnumerable<IProductAttribute> productAttr)
        {
            var sb = new StringBuilder();
            foreach (var pva in productAttr)
            {
                sb.Append(HttpUtility.HtmlEncode(pva.AttributeName)).Append(":"); 
                sb.Append((from pvav in pva.ProductAttributeValues
                           select string.Format(" {0} , {1}(RS)",  HttpUtility.HtmlEncode(pvav.AttributeValue),pvav.PriceAdjustment.ToString("#,##0.00"))
                                       ).Aggregate((p1, p2) => p1 + "," + p2));
                if (productAttr.Count() > 1)
                {
                    sb.Append("<br/>");
                }
            }

            return sb.ToString();
        }
    }
}
