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
using Orbio.Core.Domain.Orders;

namespace Orbio.Services.Messages
{
    public partial class MessageTokenProvider : IMessageTokenProvider
    {
        private readonly IWebHelper webHelper;

        public MessageTokenProvider(IWebHelper webHelper)
        {
            this.webHelper = webHelper;
        }

        #region Utilities

        /// <summary>
        /// Convert a collection to a HTML table
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>HTML table of products</returns>
        protected string ProductListToHtmlTable(Order order)
        {
            var result = "";

            var sb = new StringBuilder();
            sb.AppendLine("<table border=\"0\" style=\"width:100%;\">");

            #region Products
            sb.AppendLine("<tr style=\"background-color:white;text-align:center;\">");
            sb.AppendLine("<th>Name</th>");
            sb.AppendLine("<th>Price</th>");
            sb.AppendLine("<th>Quantity</th>");
            sb.AppendLine("<th>Total</th>");
            sb.AppendLine("</tr>");

            var table = order.OrderItems.ToList();
            for (int i = 0; i <= table.Count - 1; i++)
            {
                var orderItem = table[i];
                var product = orderItem.Product;
                if (product == null)
                    continue;

                sb.AppendLine(string.Format("<tr style=\"background-color: {0};text-align: center;\">", "white"));

                sb.AppendLine("<td style=\"padding: 0.6em 0.4em;text-align: left;\">" + HttpUtility.HtmlEncode(product.Name));

                //attributes
                if (!String.IsNullOrEmpty(orderItem.AttributeDescription))
                {
                    sb.AppendLine("<br />");
                    sb.AppendLine(orderItem.AttributeDescription);
                }

                sb.AppendLine("</td>");

                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", orderItem.UnitPriceInclTax.ToString("#,##0.00")));

                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: center;\">{0}</td>", orderItem.Quantity));

                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", orderItem.PriceInclTax.ToString("#,##0.00")));

                sb.AppendLine("</tr>");
            }
            #endregion

            #region Totals

            //subtotal
            sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: white;padding:0.6em 0.4 em;\"><strong>Sub Total</strong></td> <td style=\"background-color: white;padding:0.6em 0.4 em;\"><strong>{0}</strong></td></tr>", order.OrderSubtotalExclTax.ToString("#,##0.00")));

            //discount
            sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: white;padding:0.6em 0.4 em;\"><strong>Promotional Discounts</strong></td> <td style=\"background-color: white;padding:0.6em 0.4 em;\"><strong>{0}</strong></td></tr>", order.OrderSubTotalDiscountExclTax.ToString("#,##0.00")));
            
            ////total
            sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: white;padding:0.6em 0.4 em;\"><strong>Order Total</strong></td> <td style=\"background-color: white;padding:0.6em 0.4 em;\"><strong>{0}</strong></td></tr>", order.OrderTotal.ToString("#,##0.00")));
            #endregion

            sb.AppendLine("</table>");
            result = sb.ToString();
            return result;
        }

        #endregion

        #region Methods

        public virtual void AddStoreTokens(IList<Token> tokens,Store store)
        {
            //tokens.Add(new Token("Store.Name", store.Name));
            //tokens.Add(new Token("Store.URL", store.Url, true));
            tokens.Add(new Token("Store.Name", "Alukkasonline"));
            tokens.Add(new Token("Store.URL", webHelper.GetStoreLocation(), true));

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

        public virtual void AddOrderTokens(IList<Token> tokens, Order order)
        {
            tokens.Add(new Token("Order.OrderNumber", order.OrderId.ToString()));

            tokens.Add(new Token("Order.CustomerFullName", string.Format("{0} {1}", order.Customer.FirstName, order.Customer.LastName)));
            tokens.Add(new Token("Order.CustomerEmail", order.Customer.Email));


            tokens.Add(new Token("Order.BillingFirstName", order.BillingAddress.FirstName));
            tokens.Add(new Token("Order.BillingLastName", order.BillingAddress.LastName));
            tokens.Add(new Token("Order.BillingPhoneNumber", order.BillingAddress.PhoneNumber));
            //tokens.Add(new Token("Order.BillingEmail", order.BillingAddress.Email));
            //tokens.Add(new Token("Order.BillingFaxNumber", order.BillingAddress.FaxNumber));
            //tokens.Add(new Token("Order.BillingCompany", order.BillingAddress.Company));
            tokens.Add(new Token("Order.BillingAddress1", order.BillingAddress.Address1));
            tokens.Add(new Token("Order.BillingAddress2", order.BillingAddress.Address2));
            tokens.Add(new Token("Order.BillingCity", order.BillingAddress.City));
            tokens.Add(new Token("Order.BillingStateProvince", order.BillingAddress.States));
            tokens.Add(new Token("Order.BillingZipPostalCode", order.BillingAddress.ZipPostalCode));
            tokens.Add(new Token("Order.BillingCountry", order.BillingAddress.Country));

            tokens.Add(new Token("Order.ShippingMethod", order.ShippingMethod));
            tokens.Add(new Token("Order.ShippingFirstName", order.ShippingAddress != null ? order.ShippingAddress.FirstName : ""));
            tokens.Add(new Token("Order.ShippingLastName", order.ShippingAddress != null ? order.ShippingAddress.LastName : ""));
            tokens.Add(new Token("Order.ShippingPhoneNumber", order.ShippingAddress != null ? order.ShippingAddress.PhoneNumber : ""));
            //tokens.Add(new Token("Order.ShippingEmail", order.ShippingAddress != null ? order.ShippingAddress.Email : ""));
            //tokens.Add(new Token("Order.ShippingFaxNumber", order.ShippingAddress != null ? order.ShippingAddress.FaxNumber : ""));
            //tokens.Add(new Token("Order.ShippingCompany", order.ShippingAddress != null ? order.ShippingAddress.Company : ""));
            tokens.Add(new Token("Order.ShippingAddress1", order.ShippingAddress != null ? order.ShippingAddress.Address1 : ""));
            tokens.Add(new Token("Order.ShippingAddress2", order.ShippingAddress != null ? order.ShippingAddress.Address2 : ""));
            tokens.Add(new Token("Order.ShippingCity", order.ShippingAddress != null ? order.ShippingAddress.City : ""));
            tokens.Add(new Token("Order.ShippingStateProvince", order.ShippingAddress != null ? order.ShippingAddress.States : ""));
            tokens.Add(new Token("Order.ShippingZipPostalCode", order.ShippingAddress != null ? order.ShippingAddress.ZipPostalCode : ""));
            tokens.Add(new Token("Order.ShippingCountry", order.ShippingAddress != null && order.ShippingAddress.Country != null ? order.ShippingAddress.Country : ""));

            tokens.Add(new Token("Order.PaymentMethod", order.PaymentMethodSystemName));
            //    tokens.Add(new Token("Order.VatNumber", order.VatNumber));

            tokens.Add(new Token("Order.Product(s)", ProductListToHtmlTable(order), true));

            tokens.Add(new Token("Order.CreatedOn", order.CreatedOnUtc.ToString("D")));

            //TODO add a method for getting URL (use routing because it handles all SEO friendly URLs)
            tokens.Add(new Token("Order.OrderURLForCustomer", string.Format("{0}Customer/MyAccount", webHelper.GetStoreLocation(false)), true));


        }

        #endregion
    }
}
