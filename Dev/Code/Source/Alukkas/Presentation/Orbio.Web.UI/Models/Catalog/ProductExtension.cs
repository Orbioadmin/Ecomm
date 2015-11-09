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
                                              from opvav in opva.Values
                                              where opva.Id == pva.Id && opvav.Id==pvav.Id
                                                  select opva).FirstOrDefault();
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

        public static void ValidateProductVariantAttributes(this List<ProductVariantAttributeModel> orginalProductVariants, List<ProductVariantAttributeModel> selectedProductVariants, string productVariantId)
        {
            ////store to temp and make it null on match so that other product variant selection are mapped as it is
            if (!string.IsNullOrEmpty(productVariantId))
            {
                var selectedPva = (from pva in orginalProductVariants
                                   from pvav in pva.Values
                                   where string.Format("pvav_{0}_{1}", pva.Id, pvav.Id) == productVariantId
                                   select new { pva = pva, pvav = pvav }).FirstOrDefault();
                if (selectedPva != null)
                {
                    selectedPva.pva.ResetPreSelected();
                    selectedPva.pvav.IsPreSelected = true;
                }

                var otherPvas = (from pva in orginalProductVariants                                
                                 where  pva.Id != selectedPva.pva.Id
                                 select pva).ToList();
                SetProductVariantValues(otherPvas, selectedProductVariants);
            }
            else
            {
                SetProductVariantValues(orginalProductVariants, selectedProductVariants);
            }
        }

        private static void SetProductVariantValues(List<ProductVariantAttributeModel> orginalProductVariants, List<ProductVariantAttributeModel> selectedProductVariants)
        {
            foreach (var pva in orginalProductVariants)
            {
                foreach (var pvav in pva.Values)
                {

                    //match with ids from db and posted back from browser
                    var attrForProduct = (from sopva in selectedProductVariants
                                          from sopvav in sopva.Values
                                          where sopva.Id == pva.Id && sopvav.Id == pvav.Id
                                          select sopvav).FirstOrDefault();
                    pvav.IsPreSelected = (attrForProduct != null);

                }
            }
        }

        public static void SetIsPreSelected(this List<ProductVariantAttributeModel> productVariants)
        {
            foreach (var pva in productVariants)
            {
                var preSelectedSet = (from pvav in pva.Values
                                           where pvav.IsPreSelected == true
                                           select pvav);
                if (preSelectedSet.Count() > 1)
                {

                    var maxDisplayOrder = preSelectedSet.Max(pvav => pvav.DisplayOrder);

                    var pvavWithMaxDisplayOrder = (from pvav in preSelectedSet
                                                   where pvav.DisplayOrder == maxDisplayOrder
                                                   select pvav).FirstOrDefault();
                    if (pvavWithMaxDisplayOrder != null)
                    {
                        pva.ResetPreSelected();
                        pvavWithMaxDisplayOrder.IsPreSelected = true;
                    }
                }
            }
        }

        public static void ResetPreSelected(this ProductVariantAttributeModel productVariant)
        {
            foreach (var pvav in productVariant.Values)
            {
                pvav.IsPreSelected = false;
            }
        }
                                     
    }


}