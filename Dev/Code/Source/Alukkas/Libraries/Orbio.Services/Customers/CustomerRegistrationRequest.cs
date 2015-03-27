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
        public Customer customer { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public string mobileNo { get; set; }
        public PasswordFormat passwordFormat { get; set; }
        public bool isApproved { get; set; }
        public int id { get; set; }

        public CustomerRegistrationRequest(Customer customer, string email,
            string gender, string mobileNo, string password,
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.customer = customer;
            this.email = email;
            this.userName = email;
            this.password = password;
            this.gender = gender;
            this.mobileNo = mobileNo;
            this.passwordFormat = passwordFormat;
            this.isApproved = isApproved;
            this.id = customer.Id;
        }
    }
}
