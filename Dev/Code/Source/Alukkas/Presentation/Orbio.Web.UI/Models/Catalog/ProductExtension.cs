using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public static class ProductExtension
    {
        public static string GetProductVariantErrors(this List<ProductVariantAttributeModel> productVariants)
        {
            var errorString = string.Empty;
            foreach (var pva in productVariants)
            {
                var attrSelected = false;
                foreach (var pvav in pva.Values)
                {
                    if (pvav.Id > 0)
                    {
                        attrSelected = true;
                        break;
                    }
                }

                if (!attrSelected)
                {
                    errorString += "Select a " + pva.TextPrompt + ";";
                }
            }

            return errorString;
        }
    }
}