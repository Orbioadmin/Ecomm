using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Orbio.Services.Messages;
using Orbio.Core.Domain.Customers;
using Orbio.Core;
using Orbio.Core.Domain.Stores;

namespace Orbio.Services.Messages
{
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        private readonly IWebHelper webHelper;

        public MessageTokenProvider(IWebHelper webHelper)
        {
            this.webHelper = webHelper;
        }

        #region Methods

        public virtual void AddStoreTokens(IList<Token> tokens,Store store)
        {
            tokens.Add(new Token("Store.Name", store.Name));
            tokens.Add(new Token("Store.URL", store.Url, true));

        }

        public virtual void AddCustomerTokens(IList<Token> tokens, Customer customer)
        {
            tokens.Add(new Token("Customer.Email", customer.Email));
            tokens.Add(new Token("Customer.Username", customer.Username));
            string fullName = "";
            if (!String.IsNullOrWhiteSpace(customer.FirstName) && !String.IsNullOrWhiteSpace(customer.LastName))
                fullName = string.Format("{0} {1}", customer.FirstName, customer.LastName);
            else
            {
                if (!String.IsNullOrWhiteSpace(customer.FirstName))
                    fullName = customer.FirstName;

                if (!String.IsNullOrWhiteSpace(customer.LastName))
                    fullName = customer.LastName;
            }
            tokens.Add(new Token("Customer.FullName", fullName));

            //note: we do not use SEO friendly URLS because we can get errors caused by having .(dot) in the URL (from the email address)
            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            string passwordRecoveryUrl = string.Format("{0}Customer/PasswordRecoveryConfirm?token={1}&email={2}", webHelper.GetStoreLocation(false), customer.CustomerGuid, HttpUtility.UrlEncode(customer.Email));
            string accountActivationUrl = string.Format("{0}customer/activation?token={1}&email={2}", webHelper.GetStoreLocation(false), customer.CustomerGuid, HttpUtility.UrlEncode(customer.Email));
            tokens.Add(new Token("Customer.PasswordRecoveryURL", passwordRecoveryUrl, true));
            tokens.Add(new Token("Customer.AccountActivationURL", accountActivationUrl, true));
        }

        #endregion
    }
}
