using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Discount
{
    public class DiscountModel
    {
        public DiscountModel()
        { 
        }

        public DiscountModel(Orbio.Core.Domain.Discounts.Discount discount)
        {
            Name = discount.Name;
            UsePercentage = discount.UsePercentage;
            DiscountPercentage = discount.DiscountPercentage;
            DiscountAmount = discount.DiscountAmount;
            StartDateUtc = discount.StartDateUtc;
            EndDateUtc = discount.EndDateUtc;
        }

        public string Name { get; set; }

        public int DiscountTypeId { get; set; }

        public bool UsePercentage { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal DiscountAmount { get; set; }

        public DateTime? StartDateUtc { get; set; }

        public DateTime? EndDateUtc { get; set; }

        public bool RequiresCouponCode { get; set; }

        public string CouponCode { get; set; }

        public int DiscountLimitationId { get; set; }

        public int LimitationTimes { get; set; }

    }
}