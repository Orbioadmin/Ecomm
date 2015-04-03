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
                var textPrompt = string.Empty;
                var attrSelected = false;
                foreach (var pvav in pva.Values)
                {
                    if (pvav.Id > 0)
                    {
                        var attrForProduct = (from opva in orginalProductVariants
                                              from opvav in opva.Values
                                              where opva.Id == pva.Id && opvav.Id==pvav.Id
                                                  select opva).FirstOrDefault();
                        attrSelected = true && (attrForProduct!=null);
                        textPrompt = attrForProduct.TextPrompt;
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

        public static void ValidateProductVariantAttributes(this List<ProductVariantAttributeModel> orginalProductVariants, List<ProductVariantAttributeModel> selectedProductVariants)
        {
            foreach (var pva in orginalProductVariants)
            {                 
                foreach (var pvav in pva.Values)
                {
                    var attrForProduct = (from spva in selectedProductVariants
                                          from spvav in spva.Values
                                          where spva.Id == pva.Id && spvav.Id == pvav.Id
                                          select spva).FirstOrDefault();
                    if (attrForProduct != null)
                    {
                        pvav.IsPreSelected = true;
                    }
                }
            }
        }
    }


}