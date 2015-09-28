using Orbio.Core.Data;
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

         /// <summary>
        /// Create or update discount
        /// </summary>
        /// <param name="action"></param>
        /// <param name="discount"></param>
        void CreateOrUpdateDiscount(string action, Orbio.Core.Domain.Discounts.Discount discount);

        /// <summary>
        /// Get discount by Id
        /// </summary>
        Orbio.Core.Domain.Discounts.Discount GetDiscountById(string action, Orbio.Core.Domain.Discounts.Discount discount);

        /// <summary>
        /// Get All Usage History By Discount Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<DiscountUsageHistory> GetAllUsageHistoryByDiscountId(int id);

        /// <summary>
        /// Delete Discount usage history
        /// </summary>
        /// <param name="id"></param>
        void DeleteUsageHistory(int id);
    }
}
