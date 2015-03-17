using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Messages;
using Orbio.Services.Messages;
using Orbio.Core;
using System.Data.SqlClient;

namespace Orbio.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IDbContext context;
        private readonly IMessageTokenProvider messageTokenProvider;
        private readonly ITokenizer tokenizer;
        private readonly IStoreContext storeContext;

        public MessageService(IDbContext context, IMessageTokenProvider messageTokenProvider, ITokenizer tokenizer, IStoreContext store)
        {
            this.context = context;
            this.messageTokenProvider = messageTokenProvider;
            this.tokenizer = tokenizer;
            this.storeContext = store;
        }
        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendCustomerPasswordRecoveryMessage(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var store = storeContext.CurrentStore;

            var result = context.ExecuteFunction<MessageTemplate>("usp_MessageTemplate",
                 new SqlParameter() { ParameterName = "@messagename", Value = "Customer.PasswordRecovery", DbType = System.Data.DbType.String });
            var messageTemplate = new MessageTemplate();
            messageTemplate = result.FirstOrDefault();

            if (messageTemplate == null)
                return 0;

            //tokens
            var tokens = new List<Token>();
            messageTokenProvider.AddStoreTokens(tokens, store);
            messageTokenProvider.AddCustomerTokens(tokens, customer);

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

            var toEmail = customer.Email;
            var toName = fullName;
            return SendNotification(messageTemplate, tokens,
                toEmail, toName);
        }

        /// <summary>
        /// Sends welcome message to a customer after registration
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendCustomerWelcomeMessage(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var store = storeContext.CurrentStore;

            var result = context.ExecuteFunction<MessageTemplate>("usp_MessageTemplate",
                 new SqlParameter() { ParameterName = "@messagename", Value = "Customer.WelcomeMessage", DbType = System.Data.DbType.String });
            var messageTemplate = new MessageTemplate();
            messageTemplate = result.FirstOrDefault();

            if (messageTemplate == null)
                return 0;

            //tokens
            var tokens = new List<Token>();
            messageTokenProvider.AddStoreTokens(tokens, store);
            messageTokenProvider.AddCustomerTokens(tokens, customer);

            string fullName = "";
            if (String.IsNullOrWhiteSpace(customer.FirstName) && String.IsNullOrWhiteSpace(customer.LastName))
                fullName = string.Format("{0}","Customer");
            else
            {
                if (!String.IsNullOrWhiteSpace(customer.FirstName))
                    fullName = customer.FirstName;

                if (!String.IsNullOrWhiteSpace(customer.LastName))
                    fullName = customer.LastName;
            }

            var toEmail = customer.Email;
            var toName = fullName;
            return SendNotification(messageTemplate, tokens,
                toEmail, toName);
        }


        protected int SendNotification(MessageTemplate messageTemplate, IEnumerable<Token> tokens, string toEmailAddress,
                                               string toName, string attachmentFilePath = null, string attachmentFileName = null)
        {
            //retrieve localized message template data
            var bcc = messageTemplate.BccEmailAddresses;
            var subject = messageTemplate.Subject;
            var body = messageTemplate.Body;

            //Replace subject and body tokens 
            var subjectReplaced = tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = tokenizer.Replace(body, tokens, true);

            //var email = new QueuedEmail()
            //{
            //    Priority = 5,
            //    From = emailAccount.Email,
            //    FromName = emailAccount.DisplayName,
            //    To = toEmailAddress,
            //    ToName = toName,
            //    CC = string.Empty,
            //    Bcc = bcc,
            //    Subject = subjectReplaced,
            //    Body = bodyReplaced,
            //    AttachmentFilePath = attachmentFilePath,
            //    AttachmentFileName = attachmentFileName,
            //    CreatedOnUtc = DateTime.UtcNow,
            //    EmailAccountId = emailAccount.Id
            //};

            //_queuedEmailService.InsertQueuedEmail(email);
            context.ExecuteFunction<Customer>("usp_EmailSending",
                 new SqlParameter() { ParameterName = "@profilename", Value = "Emailsending", DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@toaddress", Value = toEmailAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@todisplayname", Value = toName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@subject", Value = subjectReplaced, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@body", Value = bodyReplaced, DbType = System.Data.DbType.String });
            return 0;
        }
    }


}
