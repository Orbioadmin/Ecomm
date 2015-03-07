using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Customers
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    public partial class Customer 
    {
        //private ICollection<ExternalAuthenticationRecord> _externalAuthenticationRecords;
       // private ICollection<CustomerRole> customerRoles;
        //private ICollection<ShoppingCartItem> _shoppingCartItems;
        //private ICollection<RewardPointsHistory> _rewardPointsHistory;
        //private ICollection<ReturnRequest> _returnRequests;
        //private ICollection<Address> _addresses;

        /// <summary>
        /// Ctor
        /// </summary>
        public Customer()
        {
            this.CustomerGuid = Guid.NewGuid();
            this.PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer Guid
        /// </summary>
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public int PasswordFormatId { get; set; }
        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }
        /// <summary>
        /// Gets or sets the password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is tax exempt
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the vendor identifier with which this customer is associated (maganer)
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the customer system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the Billing Address
        /// </summary>
        public int? BillingAddress_Id { get; set; }

        /// <summary>
        /// Gets or sets the Shipping Address
        /// </summary>
        public int? ShippingAddress_Id { get; set; }

        /// <summary>
        /// Gets or sets the First Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the DOB
        /// </summary>
        public string DOB { get; set; }

        /// <summary>
        /// Gets or sets the Gender
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the mobile number
        /// </summary>
        public string MobileNo { get; set; }

        /// <summary>
        /// Gets or sets is registered
        /// </summary>
        public bool IsRegistered { get; set; }

        /// <summary>
        /// Gets or sets is approved
        /// </summary>
        public bool IsApproved { get; set; }

        #region Navigation properties

        ///// <summary>
        ///// Gets or sets customer generated content
        ///// </summary>
        //public virtual ICollection<ExternalAuthenticationRecord> ExternalAuthenticationRecords
        //{
        //    get { return _externalAuthenticationRecords ?? (_externalAuthenticationRecords = new List<ExternalAuthenticationRecord>()); }
        //    protected set { _externalAuthenticationRecords = value; }
        //}

        //public virtual ICollection<CustomerRole> CustomerRole
        //{
        //    get { return customerRoles ?? (customerRoles = new List<CustomerRole>()); }
        //    protected set { customerRoles = value; }
        //}

        ///// <summary>
        ///// Gets or sets shopping cart items
        ///// </summary>
        //public virtual ICollection<ShoppingCartItem> ShoppingCartItems
        //{
        //    get { return _shoppingCartItems ?? (_shoppingCartItems = new List<ShoppingCartItem>()); }
        //    protected set { _shoppingCartItems = value; }
        //}

        ///// <summary>
        ///// Gets or sets reward points history
        ///// </summary>
        //public virtual ICollection<RewardPointsHistory> RewardPointsHistory
        //{
        //    get { return _rewardPointsHistory ?? (_rewardPointsHistory = new List<RewardPointsHistory>()); }
        //    protected set { _rewardPointsHistory = value; }
        //}

        ///// <summary>
        ///// Gets or sets return request of this customer
        ///// </summary>
        //public virtual ICollection<ReturnRequest> ReturnRequests
        //{
        //    get { return _returnRequests ?? (_returnRequests = new List<ReturnRequest>()); }
        //    protected set { _returnRequests = value; }
        //}

        ///// <summary>
        ///// Default billing address
        ///// </summary>
        //public virtual Address BillingAddress { get; set; }

        ///// <summary>
        ///// Default shipping address
        ///// </summary>
        //public virtual Address ShippingAddress { get; set; }

        ///// <summary>
        ///// Gets or sets customer addresses
        ///// </summary>
        //public virtual ICollection<Address> Addresses
        //{
        //    get { return _addresses ?? (_addresses = new List<Address>()); }
        //    protected set { _addresses = value; }
        //}

        #endregion
    }
}
