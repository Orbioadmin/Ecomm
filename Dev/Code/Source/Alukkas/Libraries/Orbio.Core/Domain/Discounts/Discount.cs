﻿using System;
using System.Collections.Generic;
using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Customers;

namespace Orbio.Core.Domain.Discounts
{
    /// <summary>
    /// Represents a discount
    /// </summary>
    public partial class Discount : IDiscount  
    {
        //private ICollection<DiscountRequirement> _discountRequirements;
        //private ICollection<Category> _appliedToCategories;
        //private ICollection<Product> _appliedToProducts;
        
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the discount type identifier
        /// </summary>
        public int DiscountTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use percentage
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount amount
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount start date and time
        /// </summary>
        public DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the discount end date and time
        /// </summary>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether discount requires coupon code
        /// </summary>
        public bool RequiresCouponCode { get; set; }

        /// <summary>
        /// Gets or sets the coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the discount limitation identifier
        /// </summary>
        public int DiscountLimitationId { get; set; }

        /// <summary>
        /// Gets or sets the discount limitation times (used when Limitation is set to "N Times Only" or "N Times Per Customer")
        /// </summary>
        public int LimitationTimes { get; set; }

        /// <summary>
        /// sets or gets isvalid specifically used for coupon code
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the discount type
        /// </summary>
        public DiscountType DiscountType
        {
            get
            {
                return (DiscountType)this.DiscountTypeId;
            }
            set
            {
                this.DiscountTypeId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the discount limitation
        /// </summary>
        public DiscountLimitationType DiscountLimitation
        {
            get
            {
                return (DiscountLimitationType)this.DiscountLimitationId;
            }
            set
            {
                this.DiscountLimitationId = (int)value;
            }
        }

        ///// <summary>
        ///// Gets or sets the discount requirement
        ///// </summary>
        //public virtual ICollection<DiscountRequirement> DiscountRequirements
        //{
        //    get { return _discountRequirements ?? (_discountRequirements = new List<DiscountRequirement>()); }
        //    protected set { _discountRequirements = value; }
        //}

        ///// <summary>
        ///// Gets or sets the categories
        ///// </summary>
        //public virtual ICollection<Category> AppliedToCategories
        //{
        //    get { return _appliedToCategories ?? (_appliedToCategories = new List<Category>()); }
        //    protected set { _appliedToCategories = value; }
        //}

        ///// <summary>
        ///// Gets or sets the products 
        ///// </summary>
        //public virtual ICollection<Product> AppliedToProducts
        //{
        //    get { return _appliedToProducts ?? (_appliedToProducts = new List<Product>()); }
        //    protected set { _appliedToProducts = value; }
        //}

        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the products
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the categories
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets Customers
        /// </summary>
        public List<Customer> Customers { get; set; }
    }
}
