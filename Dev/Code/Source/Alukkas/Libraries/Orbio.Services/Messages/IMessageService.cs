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
        int SendOrderCustomerNotification(Order order, int languageId, string attachmentFilePath = null, string attachmentFileName = null, int orderStatusId = 0);
        int SendNewOrderNoteAddedCustomerNotification(Order order,OrderNote orderNote, int languageId);
        int SendShipmentSentCustomerNotification(Orbio.Core.Data.Shipment shipment, Order order, int languageId);
        int SendCustomerNotification(string email, string subject, string body,string name);
        int SendDiscountCustomerNotification(List<Orbio.Core.Domain.Admin.Catalog.DiscountDetails> discounts, string email, string firstName);
    }
}
