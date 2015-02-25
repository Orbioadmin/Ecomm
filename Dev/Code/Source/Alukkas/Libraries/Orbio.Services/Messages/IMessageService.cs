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
    }
}
