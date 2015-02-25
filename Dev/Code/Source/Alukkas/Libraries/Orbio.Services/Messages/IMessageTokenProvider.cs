using System.Collections.Generic;
using Orbio.Services.Messages;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Stores;

namespace Orbio.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        void AddStoreTokens(IList<Token> tokens, Store store);

        void AddCustomerTokens(IList<Token> tokens, Customer customer);
    }
}
