using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public class CustomerRoleService :ICustomerRoleService
    {

        public List<CustomerRole> GetAllCustomerRole()
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.CustomerRoles.AsQueryable();
                return result.ToList();
            }
        }

        public CustomerRole GetCustomerRoleById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.CustomerRoles.Where(m => m.Id == Id);
                return result.FirstOrDefault();
            }
        }

        public int DeleteCustomerRole(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.CustomerRoles.Where(m => m.Id == Id).FirstOrDefault();
                    if (result != null)
                    {
                        context.CustomerRoles.Remove(result);
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public int AddOrUpdateCustomerRole(CustomerRole customerRole)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.CustomerRoles.Where(m => m.Id == customerRole.Id).FirstOrDefault();
                    if (result != null)
                    {
                        result.Name = customerRole.Name;
                        result.SystemName = customerRole.SystemName;
                        result.FreeShipping = customerRole.FreeShipping;
                        result.TaxExempt = customerRole.TaxExempt;
                        result.Active = customerRole.Active;
                        context.SaveChanges();
                    }
                    else
                    {
                        var role = context.CustomerRoles.FirstOrDefault();
                        role.Name = customerRole.Name;
                        role.SystemName = customerRole.SystemName;
                        role.FreeShipping = customerRole.FreeShipping;
                        role.TaxExempt = customerRole.TaxExempt;
                        role.Active = customerRole.Active;
                        role.IsSystemRole = false;
                        context.CustomerRoles.Add(role);
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}
