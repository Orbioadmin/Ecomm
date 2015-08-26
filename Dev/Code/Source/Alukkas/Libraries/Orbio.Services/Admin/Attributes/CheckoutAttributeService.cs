using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public class CheckoutAttributeService : ICheckoutAttributeService
    {
        #region Fields
        private readonly OrbioAdminContext context = new OrbioAdminContext();
        #endregion

        public List<CheckoutAttribute> GetCheckoutAttribute()
        {
            var result = context.CheckoutAttributes.ToList();
            return result;
        }

        public  List<TaxCategory> GetTaxCategory()
        {
            var result = context.TaxCategories.OrderBy(m=>m.DisplayOrder).ToList();
            return result;
        }

        public CheckoutAttribute GetCheckoutAttributeById(int Id)
        {
            var result = context.CheckoutAttributes.Include("CheckoutAttributeValues").Where(m => m.Id == Id).FirstOrDefault();
            return result;
        }

        public int DeleteCheckoutAttribute(int Id)
        {
            try
            {
                var result = context.CheckoutAttributes.Where(m => m.Id == Id).FirstOrDefault();
                if (result != null)
                {
                    context.CheckoutAttributes.Remove(result);
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int AddOrEditCheckoutAttribute(CheckoutAttribute model)
        {
            try
            {
                var checkout = context.CheckoutAttributes.Where(m => m.Id == model.Id).FirstOrDefault();
                if(checkout!=null)
                {
                    checkout.Name = model.Name;
                    checkout.TextPrompt = model.TextPrompt;
                    checkout.IsRequired = model.IsRequired;
                    checkout.ShippableProductRequired = model.ShippableProductRequired;
                    checkout.IsTaxExempt = model.IsTaxExempt;
                    checkout.TaxCategoryId = model.TaxCategoryId;
                    checkout.AttributeControlTypeId = model.AttributeControlTypeId;
                    checkout.DisplayOrder = model.DisplayOrder;
                    context.SaveChanges();
                }
                else
                {
                    var result = context.CheckoutAttributes.FirstOrDefault();
                    result.Name = model.Name;
                    result.TextPrompt = model.TextPrompt;
                    result.IsRequired = model.IsRequired;
                    result.ShippableProductRequired = model.ShippableProductRequired;
                    result.IsTaxExempt = model.IsTaxExempt;
                    result.TaxCategoryId = model.TaxCategoryId;
                    result.AttributeControlTypeId = model.AttributeControlTypeId;
                    result.DisplayOrder = model.DisplayOrder;
                    context.CheckoutAttributes.Add(result);
                    context.SaveChanges();
                    return result.Id;
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int DeleteCheckoutAttributeValue(int Id)
        {
            try
            {
                var result = context.CheckoutAttributeValues.Where(m => m.Id == Id).FirstOrDefault();
                if (result != null)
                {
                    context.CheckoutAttributeValues.Remove(result);
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int AddOrEditCheckoutAttributeValue(CheckoutAttributeValue model)
        {
            try
            {
                var checkout = context.CheckoutAttributeValues.Where(m => m.Id == model.Id).FirstOrDefault();
                if (checkout != null)
                {
                    checkout.Name = model.Name;
                    checkout.CheckoutAttributeId = model.CheckoutAttributeId;
                    checkout.WeightAdjustment = model.WeightAdjustment;
                    checkout.PriceAdjustment = model.PriceAdjustment;
                    checkout.IsPreSelected = model.IsPreSelected;
                    checkout.DisplayOrder = model.DisplayOrder;
                    context.SaveChanges();
                }
                else
                {
                    var result = context.CheckoutAttributeValues.FirstOrDefault();
                    result.Name = model.Name;
                    result.CheckoutAttributeId = model.CheckoutAttributeId;
                    result.WeightAdjustment = model.WeightAdjustment;
                    result.PriceAdjustment = model.PriceAdjustment;
                    result.IsPreSelected = model.IsPreSelected;
                    result.DisplayOrder = model.DisplayOrder;
                    context.CheckoutAttributeValues.Add(result);
                    context.SaveChanges();
                    return result.Id;
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public CheckoutAttributeValue AddOrEditCheckoutAttributeValue(int Id)
        {
            var result = context.CheckoutAttributeValues.Where(m => m.Id == Id).FirstOrDefault();
            return result;
        }
    }
}
