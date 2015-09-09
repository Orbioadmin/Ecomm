using AutoMapper;
using Orbio.Core.Data;
using Orbio.Web.UI.Area.Admin.Models.CheckOut;
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
                //.ForMember(ev => ev.Country, m => m.MapFrom(a => a.Country))
                //.ForMember(ev => ev.State, m => m.MapFrom(a => a.States))
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
                //.ForMember(ev => ev.Country, m => m.MapFrom(a => a.Country))
                //.ForMember(ev => ev.State, m => m.MapFrom(a => a.States))
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
    }
}