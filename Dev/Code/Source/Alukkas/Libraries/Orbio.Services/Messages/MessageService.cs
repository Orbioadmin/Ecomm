using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Orbio.Core.Domain.Email;
using Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Messages;
using Orbio.Services.Messages;
using Orbio.Services.Email;
using Orbio.Core;
using System.Data.SqlClient;
using Orbio.Core.Domain.Catalog;
using System.Data;
using System.Configuration;

namespace Orbio.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IDbContext context;
        private readonly IMessageTokenProvider messageTokenProvider;
        private readonly ITokenizer tokenizer;
        private readonly IStoreContext storeContext;
        private readonly IEmailService emailService;

        public MessageService(IDbContext context, IMessageTokenProvider messageTokenProvider, ITokenizer tokenizer, IStoreContext store, IEmailService emailService)
        {
            this.context = context;
            this.messageTokenProvider = messageTokenProvider;
            this.tokenizer = tokenizer;
            this.storeContext = store;
            this.emailService = emailService;
        }
        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendCustomerPasswordRecoveryMessage(Customer customer)
        {
            DataTable dt = new DataTable();
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
            EmailDetail Sent = SendNotification(messageTemplate, tokens,
                toEmail, toName);
            return emailService.SentEmail(Sent);
        }

        /// <summary>
        /// Sends welcome message to a customer after registration
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendCustomerWelcomeMessage(Customer customer)
        {
            DataTable dt = new DataTable();
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
            EmailDetail Sent = SendNotification(messageTemplate, tokens,
                toEmail, toName);
            return emailService.SentEmail(Sent);
           
        }

        public int SendCustomerEmailFrendMessage(Customer customer, ProductDetail product, string mail, string message, string name, string url)
        {
            DataTable dt = new DataTable();
            if (customer == null)
                throw new ArgumentNullException("customer");
            var store = storeContext.CurrentStore;
            var result = context.ExecuteFunction<MessageTemplate>("usp_MessageTemplate",
                new SqlParameter() { ParameterName = "@messagename", Value = "Service.EmailAFriend", DbType = System.Data.DbType.String });

            var messageTemplate = new MessageTemplate();
            messageTemplate = result.FirstOrDefault();
            if (messageTemplate == null)
                return 0;

            //tokens
            var tokens = new List<Token>();
            messageTokenProvider.AddStoreTokens(tokens, store);
            messageTokenProvider.AddCustomerTokens(tokens, customer);
            tokens.Add(new Token("EmailAFriend.PersonalMessage", message, true));
            tokens.Add(new Token("EmailAFriend.Email", mail));
            tokens.Add(new Token("Product.ProductURLForCustomer", url));
            tokens.Add(new Token("Product.Name", name));
            tokens.Add(new Token("Product.ShortDescription", product.ShortDescription));

            var toEmail = mail;
            var toName = "";

            EmailDetail Sent = SendNotification(messageTemplate, tokens,toEmail, toName);
            return emailService.SentEmail(Sent);
        }


        protected EmailDetail SendNotification(MessageTemplate messageTemplate, IEnumerable<Token> tokens, string toEmailAddress,
                                               string toName, string attachmentFilePath = null, string attachmentFileName = null)
        {
            DataTable dt = new DataTable();
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

             context.ExecuteFunction<EmailDetail>("usp_EmailSending",
                 new SqlParameter() { ParameterName = "@profilename", Value = "Emailsending", DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@toaddress", Value = toEmailAddress, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@todisplayname", Value = toName, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@fromaddress", Value = ConfigurationManager.AppSettings["EmailFromAddress"], DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@fromname", Value = ConfigurationManager.AppSettings["EmailFromName"], DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@subject", Value = subjectReplaced, DbType = System.Data.DbType.String },
                 new SqlParameter() { ParameterName = "@body", Value = bodyReplaced, DbType = System.Data.DbType.String });

               var Sent = new EmailDetail();
               Sent.FromAddress = ConfigurationManager.AppSettings["EmailFromAddress"];
               Sent.FromName = ConfigurationManager.AppSettings["EmailFromName"];
               Sent.Password = ConfigurationManager.AppSettings["EmailPassword"];
               Sent.ToAddress = toEmailAddress;
               Sent.ToName = toName;
               Sent.Subject = subjectReplaced;
               Sent.Body = bodyReplaced;

               return Sent;
        }
    }
}
