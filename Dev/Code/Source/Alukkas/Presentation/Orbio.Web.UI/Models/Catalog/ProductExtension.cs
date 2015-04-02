using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public static class ProductExtension
    {
        public static string GetProductVariantErrors(this List<ProductVariantAttributeModel> productVariants, List<ProductVariantAttributeModel> orginalProductVariants)
        {
            var errorString = string.Empty;
            foreach (var pva in productVariants)
            {
                var attrSelected = false;
                foreach (var pvav in pva.Values)
                {
                    if (pvav.Id > 0)
                    {
                        var attrForProduct = (from opva in orginalProductVariants 
                                              from opvav in pva.Values
                                              where opva.Id == pva.Id && opvav.Id==pvav.Id
                                                  select opvav).FirstOrDefault();
                        attrSelected = true && (attrForProduct!=null);
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