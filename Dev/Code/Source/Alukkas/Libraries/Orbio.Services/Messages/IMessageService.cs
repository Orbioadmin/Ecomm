using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Orders;
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
        int SendCustomerEmailFrendMessage(Customer customer,ProductDetail product, string email,string message,string name,string url);
        int SendNewOrderNotification(Customer customer, Order order);
        int SendQuantityBelowStoreOwnerNotification(string productIds);
        int SendOrderCompletedCustomerNotification(Order order, int languageId, string attachmentFilePath = null, string attachmentFileName = null);
        int SendOrderCancelledCustomerNotification(Order order, int languageId);
        int SendNewOrderNoteAddedCustomerNotification(OrderNote orderNote, int languageId);
    }
}
