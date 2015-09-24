using AutoMapper;
using Orbio.Core.Data;
using Orbio.Web.UI.Area.Admin.Models.CheckOut;
using Orbio.Web.UI.Areas.Admin.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin
{
    public static class MappingExtensions
    {
        #region Address

        public static Orbio.Core.Domain.Checkout.Address ToModelBill(this Address entity)
        {

            Mapper.CreateMap<Address, Orbio.Core.Domain.Checkout.Address>()
                 .ForMember(ev => ev.BillingAddress_Id, m => m.MapFrom(a => a.Id))
                .ForMember(ev => ev.FirstName, m => m.MapFrom(a => a.FirstName))
                .ForMember(ev => ev.LastName, m => m.MapFrom(a => a.LastName))
                .ForMember(ev => ev.Address1, m => m.MapFrom(a => a.Address1 + ", " + a.Address2))
                .ForMember(ev => ev.PhoneNumber, m => m.MapFrom(a => a.PhoneNumber))
                .ForMember(ev => ev.ZipPostalCode, m => m.MapFrom(a => a.ZipPostalCode))
               .ForMember(ev => ev.Country, m => m.MapFrom(a => a.Country.Name))
                .ForMember(ev => ev.States, m => m.MapFrom(a => a.StateProvince.Name))
                .ForMember(ev => ev.City, m => m.MapFrom(a => a.City));

            return Mapper.Map<Address, Orbio.Core.Domain.Checkout.Address>(entity);
        }

        public static Orbio.Core.Domain.Checkout.Address ToModelShip(this Address entity)
        {

            Mapper.CreateMap<Address, Orbio.Core.Domain.Checkout.Address>()
                 .ForMember(ev => ev.ShippingAddress_Id, m => m.MapFrom(a => a.Id))
                .ForMember(ev => ev.FirstName, m => m.MapFrom(a => a.FirstName))
                .ForMember(ev => ev.LastName, m => m.MapFrom(a => a.LastName))
                .ForMember(ev => ev.Address1, m => m.MapFrom(a => a.Address1 + ", " + a.Address2))
                .ForMember(ev => ev.PhoneNumber, m => m.MapFrom(a => a.PhoneNumber))
                .ForMember(ev => ev.ZipPostalCode, m => m.MapFrom(a => a.ZipPostalCode))
                .ForMember(ev => ev.Country, m => m.MapFrom(a => a.Country.Name))
                .ForMember(ev => ev.States, m => m.MapFrom(a => a.StateProvince.Name))
                .ForMember(ev => ev.City, m => m.MapFrom(a => a.City));

            return Mapper.Map<Address, Orbio.Core.Domain.Checkout.Address>(entity);
        }

        public static AddressModel ToModel(this Address entity)
        {

            Mapper.CreateMap<Address, AddressModel>()
                 .ForMember(ev => ev.Id, m => m.MapFrom(a => a.Id))
                .ForMember(ev => ev.FirstName, m => m.MapFrom(a => a.FirstName))
                .ForMember(ev => ev.LastName, m => m.MapFrom(a => a.LastName))
                .ForMember(ev => ev.Address, m => m.MapFrom(a => a.Address1 + ", " + a.Address2))
                .ForMember(ev => ev.Phone, m => m.MapFrom(a => a.PhoneNumber))
                .ForMember(ev => ev.Pincode, m => m.MapFrom(a => a.ZipPostalCode))
                //.ForMember(ev => ev.Country, m => m.MapFrom(a => a.Country))
                //.ForMember(ev => ev.State, m => m.MapFrom(a => a.States))
                .ForMember(ev => ev.City, m => m.MapFrom(a => a.City));

            return Mapper.Map<Address, AddressModel>(entity);
        }

        #endregion

        #region Shipment
        public static Orbio.Web.UI.Areas.Admin.Models.Orders.OrderModel.Shipping ToModel(this Shipment entity)
        {

            Mapper.CreateMap<Shipment, Orbio.Web.UI.Areas.Admin.Models.Orders.OrderModel.Shipping>()
                 .ForMember(ev => ev.Id, m => m.MapFrom(a => a.Id))
                .ForMember(ev => ev.TrackingNumber, m => m.MapFrom(a => a.TrackingNumber))
                .ForMember(ev => ev.TotalWeight, m => m.MapFrom(a => a.TotalWeight))
                .ForMember(ev => ev.DateShipped, m => m.MapFrom(a => a.ShippedDateUtc))
                .ForMember(ev => ev.DateDelivered, m => m.MapFrom(a => a.DeliveryDateUtc));

            return Mapper.Map<Shipment, Orbio.Web.UI.Areas.Admin.Models.Orders.OrderModel.Shipping>(entity);
        }
        #endregion

        #region Product

        public static ProductModel ToModel(this Product model)
        {
            Mapper.CreateMap<Product, ProductModel>()
                 .ForMember(ev => ev.Id, m => m.MapFrom(a => a.Id))
                .ForMember(ev => ev.Name, m => m.MapFrom(a => a.Name))
                .ForMember(ev => ev.ShortDescription, m => m.MapFrom(a => a.ShortDescription))
                .ForMember(ev => ev.FullDescription, m => m.MapFrom(a => a.FullDescription))
                .ForMember(ev => ev.AdminComment, m => m.MapFrom(a => a.AdminComment))
                .ForMember(ev => ev.ShowOnHomePage, m => m.MapFrom(a => a.ShowOnHomePage))
                .ForMember(ev => ev.AllowCustomerReviews, m => m.MapFrom(a => a.AllowCustomerReviews))
                .ForMember(ev => ev.Sku, m => m.MapFrom(a => a.Sku))
                .ForMember(ev => ev.Price, m => m.MapFrom(a => a.Price))
                .ForMember(ev => ev.ProductCost, m => m.MapFrom(a => a.ProductCost))
                .ForMember(ev => ev.SpecialPrice, m => m.MapFrom(a => a.SpecialPrice))
                .ForMember(ev => ev.SpecialPriceStartDateTimeUtc, m => m.MapFrom(a => a.SpecialPriceStartDateTimeUtc))
                .ForMember(ev => ev.SpecialPriceEndDateTimeUtc, m => m.MapFrom(a => a.SpecialPriceEndDateTimeUtc))
                .ForMember(ev => ev.FullDescription, m => m.MapFrom(a => a.FullDescription))
                .ForMember(ev => ev.ProductUnit, m => m.MapFrom(a => a.ProductUnit))
                .ForMember(ev => ev.IsShipEnabled, m => m.MapFrom(a => a.IsShipEnabled))
                .ForMember(ev => ev.IsFreeShipping, m => m.MapFrom(a => a.IsFreeShipping))
                .ForMember(ev => ev.AdditionalShippingCharge, m => m.MapFrom(a => a.AdditionalShippingCharge))
                .ForMember(ev => ev.AllowCustomerReviews, m => m.MapFrom(a => a.AllowCustomerReviews))
                .ForMember(ev => ev.Weight, m => m.MapFrom(a => a.Weight))
                .ForMember(ev => ev.Height, m => m.MapFrom(a => a.Height))
                .ForMember(ev => ev.Width, m => m.MapFrom(a => a.Width))
                .ForMember(ev => ev.DeliveryDateId, m => m.MapFrom(a => a.DeliveryDateId))
                .ForMember(ev => ev.IsTaxExempt, m => m.MapFrom(a => a.IsTaxExempt))
                .ForMember(ev => ev.TaxCategoryId, m => m.MapFrom(a => a.TaxCategoryId))
                .ForMember(ev => ev.ManageInventoryMethodId, m => m.MapFrom(a => a.ManageInventoryMethodId))
                .ForMember(ev => ev.StockQuantity, m => m.MapFrom(a => a.StockQuantity))
                .ForMember(ev => ev.DisplayStockAvailability, m => m.MapFrom(a => a.DisplayStockAvailability))
                .ForMember(ev => ev.MinStockQuantity, m => m.MapFrom(a => a.MinStockQuantity))
                .ForMember(ev => ev.LowStockActivityId, m => m.MapFrom(a => a.LowStockActivityId))
                .ForMember(ev => ev.NotifyAdminForQuantityBelow, m => m.MapFrom(a => a.NotifyAdminForQuantityBelow))
                .ForMember(ev => ev.BackorderModeId, m => m.MapFrom(a => a.BackorderModeId))
                .ForMember(ev => ev.AllowBackInStockSubscriptions, m => m.MapFrom(a => a.AllowBackInStockSubscriptions))
                .ForMember(ev => ev.OrderMinimumQuantity, m => m.MapFrom(a => a.OrderMinimumQuantity))
                .ForMember(ev => ev.OrderMaximumQuantity, m => m.MapFrom(a => a.OrderMaximumQuantity))
                .ForMember(ev => ev.AllowedQuantities, m => m.MapFrom(a => a.AllowedQuantities))
                .ForMember(ev => ev.AvailableStartDateTimeUtc, m => m.MapFrom(a => a.AvailableStartDateTimeUtc))
                .ForMember(ev => ev.AvailableEndDateTimeUtc, m => m.MapFrom(a => a.AvailableEndDateTimeUtc))
                .ForMember(ev => ev.Published, m => m.MapFrom(a => a.Published))
                .ForMember(ev => ev.CreatedOnUtc, m => m.MapFrom(a => a.CreatedOnUtc))
                .ForMember(ev => ev.UpdatedOnUtc, m => m.MapFrom(a => a.UpdatedOnUtc));
            return Mapper.Map<Product, ProductModel>(model);
        }

        public static Product ToEntity(this ProductModel model)
        {
            Mapper.CreateMap<ProductModel, Product>()
                 .ForMember(ev => ev.Id, m => m.MapFrom(a => a.Id))
                .ForMember(ev => ev.Name, m => m.MapFrom(a => a.Name))
                .ForMember(ev => ev.ShortDescription, m => m.MapFrom(a => a.ShortDescription))
                .ForMember(ev => ev.FullDescription, m => m.MapFrom(a => a.FullDescription.ToString()))
                .ForMember(ev => ev.AdminComment, m => m.MapFrom(a => a.AdminComment))
                .ForMember(ev => ev.ShowOnHomePage, m => m.MapFrom(a => a.ShowOnHomePage))
                .ForMember(ev => ev.AllowCustomerReviews, m => m.MapFrom(a => a.AllowCustomerReviews))
                .ForMember(ev => ev.Sku, m => m.MapFrom(a => a.Sku))
                .ForMember(ev => ev.Price, m => m.MapFrom(a => a.Price))
                .ForMember(ev => ev.ProductCost, m => m.MapFrom(a => a.ProductCost))
                .ForMember(ev => ev.SpecialPrice, m => m.MapFrom(a => a.SpecialPrice))
                .ForMember(ev => ev.SpecialPriceStartDateTimeUtc, m => m.MapFrom(a => a.SpecialPriceStartDateTimeUtc))
                .ForMember(ev => ev.SpecialPriceEndDateTimeUtc, m => m.MapFrom(a => a.SpecialPriceEndDateTimeUtc))
                .ForMember(ev => ev.FullDescription, m => m.MapFrom(a => a.FullDescription))
                .ForMember(ev => ev.ProductUnit, m => m.MapFrom(a => a.ProductUnit))
                .ForMember(ev => ev.IsShipEnabled, m => m.MapFrom(a => a.IsShipEnabled))
                .ForMember(ev => ev.IsFreeShipping, m => m.MapFrom(a => a.IsFreeShipping))
                .ForMember(ev => ev.AdditionalShippingCharge, m => m.MapFrom(a => a.AdditionalShippingCharge))
                .ForMember(ev => ev.AllowCustomerReviews, m => m.MapFrom(a => a.AllowCustomerReviews))
                .ForMember(ev => ev.Weight, m => m.MapFrom(a => a.Weight))
                .ForMember(ev => ev.Height, m => m.MapFrom(a => a.Height))
                .ForMember(ev => ev.Width, m => m.MapFrom(a => a.Width))
                .ForMember(ev => ev.DeliveryDateId, m => m.MapFrom(a => a.DeliveryDateId))
                .ForMember(ev => ev.IsTaxExempt, m => m.MapFrom(a => a.IsTaxExempt))
                .ForMember(ev => ev.TaxCategoryId, m => m.MapFrom(a => a.TaxCategoryId))
                .ForMember(ev => ev.ManageInventoryMethodId, m => m.MapFrom(a => a.ManageInventoryMethodId))
                .ForMember(ev => ev.StockQuantity, m => m.MapFrom(a => a.StockQuantity))
                .ForMember(ev => ev.DisplayStockAvailability, m => m.MapFrom(a => a.DisplayStockAvailability))
                .ForMember(ev => ev.MinStockQuantity, m => m.MapFrom(a => a.MinStockQuantity))
                .ForMember(ev => ev.LowStockActivityId, m => m.MapFrom(a => a.LowStockActivityId))
                .ForMember(ev => ev.NotifyAdminForQuantityBelow, m => m.MapFrom(a => a.NotifyAdminForQuantityBelow))
                .ForMember(ev => ev.BackorderModeId, m => m.MapFrom(a => a.BackorderModeId))
                .ForMember(ev => ev.AllowBackInStockSubscriptions, m => m.MapFrom(a => a.AllowBackInStockSubscriptions))
                .ForMember(ev => ev.OrderMinimumQuantity, m => m.MapFrom(a => a.OrderMinimumQuantity))
                .ForMember(ev => ev.OrderMaximumQuantity, m => m.MapFrom(a => a.OrderMaximumQuantity))
                .ForMember(ev => ev.AllowedQuantities, m => m.MapFrom(a => a.AllowedQuantities))
                .ForMember(ev => ev.AvailableStartDateTimeUtc, m => m.MapFrom(a => a.AvailableStartDateTimeUtc))
                .ForMember(ev => ev.AvailableEndDateTimeUtc, m => m.MapFrom(a => a.AvailableEndDateTimeUtc))
                .ForMember(ev => ev.Published, m => m.MapFrom(a => a.Published))
                .ForMember(ev => ev.CreatedOnUtc, m => m.MapFrom(a => a.CreatedOnUtc))
                .ForMember(ev => ev.UpdatedOnUtc, m => m.MapFrom(a => a.UpdatedOnUtc));
            return Mapper.Map<ProductModel, Product>(model);
        }

        #endregion
    }
}
