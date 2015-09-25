using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Orbio.Services.Admin.Discount
{
    public class DiscountService:IDiscountService
    {
        private readonly IDbContext dbContext;

        // instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public DiscountService(IDbContext dbContext)
        {
            this.dbContext = dbContext;

        }

        #region methods
        /// <summary>
        /// Get all discount lists
        /// </summary>
        public List<Orbio.Core.Domain.Discounts.Discount> GetAllDiscounts()
        {
            var sqlParamList = new List<SqlParameter>();
            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_OrbioAdmin_GetAllDiscount", sqlParamList.ToArray()).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var order = Serializer.GenericDeSerializer<List<Orbio.Core.Domain.Discounts.Discount>>(result.XmlResult);
                return order;
            }

            return new List<Orbio.Core.Domain.Discounts.Discount>();
        }
        #endregion
    }
}
