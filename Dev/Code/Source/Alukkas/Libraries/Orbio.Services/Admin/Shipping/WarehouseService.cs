using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Shipping
{
    public class WarehouseService : IWarehouseService
    {
        public List<Warehouse> GetAllWarehouseDetails()
        {
            using(var context = new OrbioAdminContext())
            {
                var result = context.Warehouses.Include("Address.Country").Include("Address.StateProvince").ToList();
                return result;
            }
        }

        public Warehouse Edit(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Warehouses.Include("Address.Country").Include("Address.StateProvince").Where(m => m.Id == id).FirstOrDefault();
                return result;
            }
        }

        public int AddOrUpdate(int id, string name, int addressId, int countryId, int stateProvinceId, string city, string address, string pincode)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    if (addressId != 0)
                    {
                        var result = context.Addresses.Where(m => m.Id == addressId).FirstOrDefault();
                        if (result != null)
                        {
                            result.CountryId = countryId;
                            result.StateProvinceId = stateProvinceId;
                            result.City = city;
                            result.Address1 = address;
                            result.ZipPostalCode = pincode;
                            context.SaveChanges();
                            var warehouse = context.Warehouses.Where(m => m.Id == id).FirstOrDefault();
                            if (warehouse != null)
                            {
                                warehouse.Name = name;
                                warehouse.AddressId = addressId;
                                context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var result = context.Addresses.FirstOrDefault();
                        if (result != null)
                        {
                            result.FirstName = null;
                            result.LastName = null;
                            result.Email = null;
                            result.Company = null;
                            result.PhoneNumber = null;
                            result.CountryId = countryId;
                            result.StateProvinceId = stateProvinceId;
                            result.City = city;
                            result.Address1 = address;
                            result.ZipPostalCode = pincode;
                            result.CreatedOnUtc = DateTime.Now;
                            context.Addresses.Add(result);
                            context.SaveChanges();
                            var warehouse = context.Warehouses.FirstOrDefault();
                            if (warehouse != null)
                            {
                                warehouse.Name = name;
                                warehouse.AddressId = result.Id;
                                context.Warehouses.Add(warehouse);
                                context.SaveChanges();
                            }
                        }
                    }
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int Delete(int id)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    var warehouse = context.Warehouses.Where(m => m.Id == id).FirstOrDefault();
                    if (warehouse != null)
                    {
                        context.Warehouses.Remove(warehouse);
                        context.SaveChanges();
                    }
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}
