﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orbio.Core.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OrbioAdminContext : DbContext
    {
        public OrbioAdminContext()
            : base("name=OrbioAdminContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<CustomerRole> CustomerRoles { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RecurringPayment> RecurringPayments { get; set; }
        public virtual DbSet<Discount_AppliedToCategories> Discount_AppliedToCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryTemplate> CategoryTemplates { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<ManufacturerTemplate> ManufacturerTemplates { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<UrlRecord> UrlRecords { get; set; }
        public virtual DbSet<Product_Category_Mapping> Product_Category_Mapping { get; set; }
        public virtual DbSet<Product_Manufacturer_Mapping> Product_Manufacturer_Mapping { get; set; }
        public virtual DbSet<Product_ProductAttribute_Mapping> Product_ProductAttribute_Mapping { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
        public virtual DbSet<SpecificationAttribute> SpecificationAttributes { get; set; }
        public virtual DbSet<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; }
        public virtual DbSet<Product_SpecificationAttribute_Mapping> Product_SpecificationAttribute_Mapping { get; set; }
        public virtual DbSet<CheckoutAttribute> CheckoutAttributes { get; set; }
        public virtual DbSet<CheckoutAttributeValue> CheckoutAttributeValues { get; set; }
        public virtual DbSet<ProductComponent> ProductComponents { get; set; }
        public virtual DbSet<PriceComponent> PriceComponents { get; set; }
    
        public virtual ObjectResult<Order> usp_Get_AdminOrderDetails(Nullable<int> orderStatusId, Nullable<int> paymentStatusId, Nullable<int> shippingStatusId, Nullable<int> customerId, Nullable<System.DateTime> createdFromUtc, Nullable<System.DateTime> createdToUtc, string billingEmail, Nullable<int> orderNo)
        {
            var orderStatusIdParameter = orderStatusId.HasValue ?
                new ObjectParameter("orderStatusId", orderStatusId) :
                new ObjectParameter("orderStatusId", typeof(int));
    
            var paymentStatusIdParameter = paymentStatusId.HasValue ?
                new ObjectParameter("paymentStatusId", paymentStatusId) :
                new ObjectParameter("paymentStatusId", typeof(int));
    
            var shippingStatusIdParameter = shippingStatusId.HasValue ?
                new ObjectParameter("shippingStatusId", shippingStatusId) :
                new ObjectParameter("shippingStatusId", typeof(int));
    
            var customerIdParameter = customerId.HasValue ?
                new ObjectParameter("customerId", customerId) :
                new ObjectParameter("customerId", typeof(int));
    
            var createdFromUtcParameter = createdFromUtc.HasValue ?
                new ObjectParameter("createdFromUtc", createdFromUtc) :
                new ObjectParameter("createdFromUtc", typeof(System.DateTime));
    
            var createdToUtcParameter = createdToUtc.HasValue ?
                new ObjectParameter("createdToUtc", createdToUtc) :
                new ObjectParameter("createdToUtc", typeof(System.DateTime));
    
            var billingEmailParameter = billingEmail != null ?
                new ObjectParameter("billingEmail", billingEmail) :
                new ObjectParameter("billingEmail", typeof(string));
    
            var orderNoParameter = orderNo.HasValue ?
                new ObjectParameter("orderNo", orderNo) :
                new ObjectParameter("orderNo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order>("usp_Get_AdminOrderDetails", orderStatusIdParameter, paymentStatusIdParameter, shippingStatusIdParameter, customerIdParameter, createdFromUtcParameter, createdToUtcParameter, billingEmailParameter, orderNoParameter);
        }
    
        public virtual ObjectResult<Order> usp_Get_AdminOrderDetails(Nullable<int> orderStatusId, Nullable<int> paymentStatusId, Nullable<int> shippingStatusId, Nullable<int> customerId, Nullable<System.DateTime> createdFromUtc, Nullable<System.DateTime> createdToUtc, string billingEmail, Nullable<int> orderNo, MergeOption mergeOption)
        {
            var orderStatusIdParameter = orderStatusId.HasValue ?
                new ObjectParameter("orderStatusId", orderStatusId) :
                new ObjectParameter("orderStatusId", typeof(int));
    
            var paymentStatusIdParameter = paymentStatusId.HasValue ?
                new ObjectParameter("paymentStatusId", paymentStatusId) :
                new ObjectParameter("paymentStatusId", typeof(int));
    
            var shippingStatusIdParameter = shippingStatusId.HasValue ?
                new ObjectParameter("shippingStatusId", shippingStatusId) :
                new ObjectParameter("shippingStatusId", typeof(int));
    
            var customerIdParameter = customerId.HasValue ?
                new ObjectParameter("customerId", customerId) :
                new ObjectParameter("customerId", typeof(int));
    
            var createdFromUtcParameter = createdFromUtc.HasValue ?
                new ObjectParameter("createdFromUtc", createdFromUtc) :
                new ObjectParameter("createdFromUtc", typeof(System.DateTime));
    
            var createdToUtcParameter = createdToUtc.HasValue ?
                new ObjectParameter("createdToUtc", createdToUtc) :
                new ObjectParameter("createdToUtc", typeof(System.DateTime));
    
            var billingEmailParameter = billingEmail != null ?
                new ObjectParameter("billingEmail", billingEmail) :
                new ObjectParameter("billingEmail", typeof(string));
    
            var orderNoParameter = orderNo.HasValue ?
                new ObjectParameter("orderNo", orderNo) :
                new ObjectParameter("orderNo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Order>("usp_Get_AdminOrderDetails", mergeOption, orderStatusIdParameter, paymentStatusIdParameter, shippingStatusIdParameter, customerIdParameter, createdFromUtcParameter, createdToUtcParameter, billingEmailParameter, orderNoParameter);
        }
    }
}
