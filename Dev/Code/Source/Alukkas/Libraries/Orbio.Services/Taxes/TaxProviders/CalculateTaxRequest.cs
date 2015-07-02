
using Orbio.Core.Domain.Customers;
using System.Collections.Generic;
namespace Orbio.Services.Taxes.TaxProviders
{
    public class CalculateTaxRequest
    {
        /// <summary>
        /// gets or sets taxcategory id
        /// </summary>
        public List<int> TaxCategoryIds { get; set; }

        /// <summary>
        /// Gets or sets a customer
        /// </summary>
        public Customer Customer { get; set; }
    }
}
