using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Discount
{
    public interface IDiscountService
    {
        /// <summary>
        /// Get all discount lists
        /// </summary>
        List<Orbio.Core.Domain.Discounts.Discount> GetAllDiscounts();
    }
}
