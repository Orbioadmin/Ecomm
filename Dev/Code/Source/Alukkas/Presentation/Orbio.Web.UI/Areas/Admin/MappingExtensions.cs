﻿using AutoMapper;
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
    }
}