using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Orbio.Core.Domain.Customers;

namespace Orbio.Services.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        //private readonly ICustomerService _customerService;
        //private readonly CustomerSettings _customerSettings;
        private readonly TimeSpan _expirationTimeSpan;

        private Customer cachedCustomer;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsAuthenticationService(HttpContextBase httpContext
            //,            ICustomerService customerService, CustomerSettings customerSettings
            )
        {
            this._httpContext = httpContext;
            //this._customerService = customerService;
            //this._customerSettings = customerSettings;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }


        public virtual void SignIn(Customer customer, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                customer.Email,  //hardcoding for now need to get customersetting _customerSettings.UsernamesEnabled ? customer.Username : customer.Email,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                customer.Email,  //hardcoding for now need to get customersetting _customerSettings.UsernamesEnabled ? customer.Username : customer.Email,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _httpContext.Response.Cookies.Add(cookie);
            cachedCustomer = customer;
        }

        public virtual void SignOut()
        {
            cachedCustomer = null;
            FormsAuthentication.SignOut();
        }

        public virtual string GetAuthenticatedCustomerData()
        {
            //if (_cachedCustomer != null)
            //    return _cachedCustomer;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            return GetAuthenticatedCustomerDataFromTicket(formsIdentity.Ticket); 
            //var customerData = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            //if (customer != null && customer.Active && !customer.Deleted && customer.IsRegistered())
            //    _cachedCustomer = customer;
            //return _cachedCustomer;
        }

        public virtual string GetAuthenticatedCustomerDataFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            return ticket.UserData;
            //var usernameOrEmail = ticket.UserData;

            //if (String.IsNullOrWhiteSpace(usernameOrEmail))
            //    return null;
            //var customer = _customerSettings.UsernamesEnabled
            //    ? _customerService.GetCustomerByUsername(usernameOrEmail)
            //    : _customerService.GetCustomerByEmail(usernameOrEmail);
            //return customer;
        }
    }
}
