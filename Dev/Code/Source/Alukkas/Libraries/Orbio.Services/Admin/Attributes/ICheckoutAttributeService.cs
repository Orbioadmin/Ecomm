using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public interface ICheckoutAttributeService
    {
        List<CheckoutAttribute> GetCheckoutAttribute();

        CheckoutAttribute GetCheckoutAttributeById(int Id);

        int DeleteCheckoutAttribute(int Id);

        List<TaxCategory> GetTaxCategory();

        int AddOrEditCheckoutAttribute(CheckoutAttribute model);

        int AddOrEditCheckoutAttributeValue(CheckoutAttributeValue model);

        int DeleteCheckoutAttributeValue(int Id);

        CheckoutAttributeValue AddOrEditCheckoutAttributeValue(int Id);

    }
}
