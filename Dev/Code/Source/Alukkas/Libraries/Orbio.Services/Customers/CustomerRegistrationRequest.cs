using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Customers;


namespace Orbio.Services.Customers
{
    public class CustomerRegistrationRequest
    {
        public Customer Customer { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool IsApproved { get; set; }
        public int Id { get; set; }

        public CustomerRegistrationRequest(Customer customer, string email,
            string gender, string mobileno, string password,
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.Customer = customer;
            this.Email = email;
            this.Username = email;
            this.Password = password;
            this.Gender = gender;
            this.MobileNo = mobileno;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
            this.Id = customer.Id;
        }
    }
}
