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

        /// <summary>
        /// Get discount by Id
        /// </summary>
        public Orbio.Core.Domain.Discounts.Discount GetDiscountById(string action, Orbio.Core.Domain.Discounts.Discount discount)
        {

            var result = dbContext.ExecuteFunction<XmlResultSet>("usp_CreateOrUpdateDiscount",
                              new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                               new SqlParameter() { ParameterName = "@id", Value = discount.Id, DbType = System.Data.DbType.Int32 },
                                new SqlParameter() { ParameterName = "@name", Value = discount.Name, DbType = System.Data.DbType.String },
                                 new SqlParameter() { ParameterName = "@discountTypeId", Value = discount.DiscountTypeId, DbType = System.Data.DbType.Int32 },
                                 new SqlParameter() { ParameterName = "@usePercentage", Value = discount.UsePercentage, DbType = System.Data.DbType.Boolean },
             new SqlParameter() { ParameterName = "@discountPercentage", Value = discount.DiscountPercentage, DbType = System.Data.DbType.Decimal },
             new SqlParameter() { ParameterName = "@discountAmount", Value = discount.DiscountAmount, DbType = System.Data.DbType.Decimal },
             new SqlParameter() { ParameterName = "@startDateUtc", Value = discount.StartDateUtc, DbType = System.Data.DbType.DateTime },
             new SqlParameter() { ParameterName = "@endDateUtc", Value = discount.EndDateUtc, DbType = System.Data.DbType.DateTime },
             new SqlParameter() { ParameterName = "@requiresCouponCode", Value = discount.RequiresCouponCode, DbType = System.Data.DbType.Boolean },
             new SqlParameter() { ParameterName = "@couponCode", Value = discount.CouponCode, DbType = System.Data.DbType.String },
            new SqlParameter() { ParameterName = "@discountLimitationId", Value = discount.DiscountLimitationId, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@limitationTime", Value = discount.LimitationTimes, DbType = System.Data.DbType.Int32 }).FirstOrDefault();
            
            if (result != null)
            {
                var discountDetails = Serializer.GenericDeSerializer<Orbio.Core.Domain.Discounts.Discount>(result.XmlResult);
                return discountDetails;
            }

            return new Orbio.Core.Domain.Discounts.Discount();
        }

        /// <summary>
        /// Create or update discount
        /// </summary>
        /// <param name="action"></param>
        /// <param name="discount"></param>
        public void CreateOrUpdateDiscount(string action, Orbio.Core.Domain.Discounts.Discount discount)
        {
            dbContext.ExecuteFunction<Orbio.Core.Domain.Discounts.Discount>("usp_CreateOrUpdateDiscount",
                              new SqlParameter() { ParameterName = "@action", Value = action, DbType = System.Data.DbType.String },
                               new SqlParameter() { ParameterName = "@id", Value = discount.Id, DbType = System.Data.DbType.Int32 },
                                new SqlParameter() { ParameterName = "@name", Value = discount.Name, DbType = System.Data.DbType.String },
                                 new SqlParameter() { ParameterName = "@discountTypeId", Value = discount.DiscountTypeId, DbType = System.Data.DbType.Int32 },
                                 new SqlParameter() { ParameterName = "@usePercentage", Value = discount.UsePercentage, DbType = System.Data.DbType.Boolean },
             new SqlParameter() { ParameterName = "@discountPercentage", Value = discount.DiscountPercentage, DbType = System.Data.DbType.Decimal },
             new SqlParameter() { ParameterName = "@discountAmount", Value = discount.DiscountAmount, DbType = System.Data.DbType.Decimal },
             new SqlParameter() { ParameterName = "@startDateUtc", Value = discount.StartDateUtc, DbType = System.Data.DbType.DateTime },
             new SqlParameter() { ParameterName = "@endDateUtc", Value = discount.EndDateUtc, DbType = System.Data.DbType.DateTime },
             new SqlParameter() { ParameterName = "@requiresCouponCode", Value = discount.RequiresCouponCode, DbType = System.Data.DbType.Boolean },
             new SqlParameter() { ParameterName = "@couponCode", Value = discount.CouponCode, DbType = System.Data.DbType.String },
            new SqlParameter() { ParameterName = "@discountLimitationId", Value = discount.DiscountLimitationId, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@limitationTime", Value = discount.LimitationTimes, DbType = System.Data.DbType.Int32 });
        }

        /// <summary>
        /// Get All Usage History By Discount Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DiscountUsageHistory> GetAllUsageHistoryByDiscountId(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.DiscountUsageHistories.Where(d=>d.DiscountId == id).ToList();
                return result;
            }
        }

        /// <summary>
        /// Delete Discount usage history
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUsageHistory(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                DiscountUsageHistory discountUsageHistory = context.DiscountUsageHistories.Where(m => m.Id == id).FirstOrDefault();
                context.Entry(discountUsageHistory).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }
        #endregion
    }
}
