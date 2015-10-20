using System.Collections.Generic;
using Orbio.Services.Messages;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Stores;
using Orbio.Core.Domain.Orders;

namespace Orbio.Services.Messages
{
    public partial interface IMessageTokenProvider
    {
        void AddStoreTokens(IList<Token> tokens, Store store);

        void AddCustomerTokens(IList<Token> tokens, Customer customer);

        void AddOrderTokens(IList<Token> tokens, Order order);

        void AddOrderNoteTokens(IList<Token> tokens, OrderNote orderNote);

        void AddShipmentTokens(IList<Token> tokens, Orbio.Core.Data.Shipment shipment, Order order, int languageId);

        void AddDiscountTokens(IList<Token> tokens, Orbio.Core.Domain.Admin.Catalog.DiscountDetails discounts);
    }
}
