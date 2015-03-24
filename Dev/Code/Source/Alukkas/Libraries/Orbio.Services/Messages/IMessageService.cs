using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Messages
{
    /// <summary>
    /// Message service interface
    /// </summary>
    public partial interface IMessageService
    {
        int SendCustomerPasswordRecoveryMessage(Customer customer);
        int SendCustomerWelcomeMessage(Customer customer);
        int SendCustomerEmailFrendMessage(Customer customer,ProductDetail product, string email,string message,string Name,string url);
    }
}
