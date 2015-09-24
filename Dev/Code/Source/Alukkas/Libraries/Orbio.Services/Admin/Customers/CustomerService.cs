using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Utility;
using Orbio.Services.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Orbio.Services.Admin.Customers
{
    public class CustomerService : ICustomerService
    {

        private readonly IEncryptionService encryptionService;
        private readonly IDbContext dbContext;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
        public CustomerService(IEncryptionService encryptionService, IDbContext dbContext)
        {
            this.encryptionService = encryptionService;
            this.dbContext = dbContext;
        }

        public List<Customer> GetAllCustomer(string FirstName, string LastName, string Email, List<int> Roles)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = new List<Customer>();
                var query = context.Customers.Include("CustomerRoles").Where(m => !m.Deleted && m.Email!=null).OrderByDescending(m=>m.Id);
                
                if (FirstName != null)
                    query = query.Where(m => m.FirstName.Contains(FirstName)).OrderByDescending(m => m.Id);
                if (LastName != null)
                    query = query.Where(m => m.LastName.Contains(LastName)).OrderByDescending(m => m.Id);
                if (Email != null)
                    query = query.Where(m => m.Email.Contains(Email)).OrderByDescending(m => m.Id);
                if (Roles!=null)
                    query = query.Where(p => p.CustomerRoles.Any(cr => Roles.Contains(cr.Id))).OrderByDescending(m => m.Id);

                return query.ToList();
            }
        }

        public Customer GetCustomerById(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Customers.Include("CustomerRoles").Where(m => m.Id == Id).FirstOrDefault();

                if(result==null)
                {
                    result = new Customer();
                    result.CustomerRoles = context.CustomerRoles.Where(cr => cr.Active).ToList();
                }
                return result;
            }
        }

        public Customer GetOrderDetails(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Customers.Include("Orders").Where(m => m.Id == Id).FirstOrDefault();
                return result;
            }
        }

        public int  AddOrUpdateCustomerInfo(Customer customer, List<int> Roles)
        {
            using (var context = new OrbioAdminContext())
            {
                var roleXml = Serializer.GenericSerializer(Roles);
                var result = context.Customers.Where(c => c.Id == customer.Id).FirstOrDefault();
                if (result == null)
                {
                    customer.PasswordFormatId = 1;
                    customer.Deleted = false;
                    customer.LastIpAddress = "::1";
                    customer.LastLoginDateUtc = DateTime.UtcNow;
                    customer.LastActivityDateUtc = DateTime.UtcNow;
                    customer.CreatedOnUtc = DateTime.UtcNow;
                    customer.Username = customer.Email;
                    context.Customers.Add(customer);
                    context.SaveChanges();
                    var Id = customer.Id;
                }
                else
                {
                    result.FirstName = customer.FirstName;
                    result.LastName = customer.LastName;
                    result.Email = customer.Email;
                    result.Gender = customer.Gender;
                    result.DOB = customer.DOB;
                    result.AdminComment = customer.AdminComment;
                    result.IsTaxExempt = customer.IsTaxExempt;
                    result.Active = customer.Active;
                    result.LastLoginDateUtc = DateTime.UtcNow;
                    result.LastActivityDateUtc = DateTime.UtcNow;
                    result.Username = customer.Email;
                    context.SaveChanges();
                }
            }
            return 1;
        }

        public Customer GetCustomerAddressDetails(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.Customers.Include("Address.Country.StateProvinces").FirstOrDefault();
            return result; 
            }
        }

        public int DeleteCustomer(int Id)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    var result = context.Customers.Where(c => c.Id == Id).FirstOrDefault();
                    if (result != null)
                    {
                        result.Deleted = true;
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
