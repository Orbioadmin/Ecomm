using Orbio.Web.UI.Models.Catalog;
using Orbio.Web.UI.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Discount
{
    public class DiscountModel
    {
        public DiscountModel()
        {
            this.Products = new List<ProductOverViewModel>();
            this.Categories = new List<CategorySimpleModel>();
            this.Customers = new List<CustomerModel>();
        }

        public DiscountModel(Orbio.Core.Domain.Discounts.Discount discount)
        {
            this.Id = discount.Id;
            this.Name = discount.Name;
            this.DiscountTypeId = discount.DiscountTypeId;
            this.UsePercentage = discount.UsePercentage;
            this.DiscountPercentage = discount.DiscountPercentage;
            this.DiscountAmount = discount.DiscountAmount;
            this.RequiresCouponCode = discount.RequiresCouponCode;
            this.CouponCode = discount.CouponCode;
            this.DiscountLimitationId = discount.DiscountLimitationId;
            this.LimitationTimes = discount.LimitationTimes;
            this.StartDateUtc = discount.StartDateUtc;
            this.EndDateUtc = discount.EndDateUtc;
            if (discount.Categories != null && discount.Categories.Count > 0)
            {
                this.Categories = (from c in discount.Categories
                                   select new CategorySimpleModel(c)).ToList();
            }
            if (discount.Products != null && discount.Products.Count > 0)
            {
                this.Products = (from p in discount.Products
                                 select new ProductOverViewModel(p)).ToList();
            }
            if (discount.Customers != null && discount.Customers.Count > 0)
            {
                this.Customers = (from p in discount.Customers
                                   select new CustomerModel(p)).ToList();
            }
        }

        public int Id { get; set; }

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

        public List<ProductOverViewModel> Products { get; set; }

        public List<CategorySimpleModel> Categories { get; set; }

        public List<CustomerModel> Customers { get; set; }
    }
}