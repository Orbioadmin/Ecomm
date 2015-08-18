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
            sb.AppendLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"   style=\"margin-left:10px;width:600px;margin-right:10px;border:1px solid #cccccc\" align=\"center\">");

            #region Products
            sb.AppendLine("<tr  style=\"background-color:#f8c301;font-family:'Arial'\">");
            sb.AppendLine("<td style=\"padding:5px;font-size:12px;color:#282828;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"center\"> S No.</td> ");
            sb.AppendLine("<td style=\"padding:5px;font-size:12px;color:#282828;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"center\"> Product Code</td>");
            sb.AppendLine("<td style=\"padding:5px;font-size:12px;color:#282828;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"center\"> Item Name</td>");
            sb.AppendLine("<td style=\"padding:5px;font-size:12px;color:#282828;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"center\"> Quantity</td>");
            sb.AppendLine("<td style=\"padding:5px;font-size:12px;color:#282828;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"center\"> Unit Price</td>");
            sb.AppendLine("<td style=\"padding:5px;font-size:12px;color:#282828;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"center\"> Price</td>");
            sb.AppendLine("</tr>");

            var table = order.OrderItems.ToList();
            for (int i = 0; i <= table.Count - 1; i++)
            {
                var orderItem = table[i];
                var product = orderItem.Product;
                if (product == null)
                    continue;

                sb.AppendLine("<tr>");

                sb.AppendLine("<td style=\"font-size:12px;color:#434343;border-right:1px solid #cccccc;border-bottom: 1px solid #cccccc;font-family:'Arial'\" align=\"center\" valign=\"top\">" + (i + 1) + "</td>");

                sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#434343;border-right:1px solid #cccccc;border-bottom: 1px solid #cccccc;font-family:'Arial'\" align=\"center\" valign=\"top\">{0}</td>", product.Sku));

                sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#434343;border-right:1px solid #cccccc;border-bottom: 1px solid #cccccc;font-family:'Arial'\" align=\"center\" valign=\"top\">{0}", product.Name));

                //attributes
                if (!String.IsNullOrEmpty(orderItem.AttributeDescription))
                {
                    sb.AppendLine("<br />");
                    sb.AppendLine(orderItem.AttributeDescription);
                }

                sb.AppendLine("</td>");


                sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#434343;border-right:1px solid #cccccc;border-bottom: 1px solid #cccccc;font-family:'Arial'\" align=\"center\" valign=\"top\">{0}</td>", orderItem.Quantity));

                sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#434343;border-right:1px solid #cccccc;border-bottom: 1px solid #cccccc;font-family:'Arial'\" align=\"center\" valign=\"top\"><span>&nbsp;{0} </span>{1}</td>", "Rs", orderItem.UnitPriceInclTax.ToString("#,##0.00")));

                sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#434343;border-right:1px solid #cccccc;border-bottom: 1px solid #cccccc;font-family:'Arial'\" align=\"center\" valign=\"top\"><span>&nbsp;{0} </span>{1}</td>", "Rs", orderItem.PriceInclTax.ToString("#,##0.00")));

                sb.AppendLine("</tr>");
            }
            #endregion

            #region Totals

            sb.AppendLine("<tr>");

            sb.AppendLine("<td style=\"padding:5px;border-top:1px solid #cccccc;;border-right:1px solid #cccccc\" align=\"center\" colspan=\"3\"></td>");

            sb.AppendLine("<td style=\"font-size:12px;color:#434343;border-top:1px solid #cccccc;border-right:1px solid #cccccc;font-family:'Arial'\" align=\"right\" colspan=\"2\" ><p style=\"padding-right: 6px;\">SUB TOTAL</p><p style=\"padding-right: 6px;\">Promotional Discounts</p><p style=\"padding-right: 6px;\">SHIPPING COST</p></td>");

            //subtotal
            sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#434343;border-top:1px solid #cccccc;font-family:'Arial'\" align=\"center\"><p><span>&nbsp;{0} </span>{1}</p>", "Rs", order.OrderSubtotalExclTax.ToString("#,##0.00")));

            //discount
            sb.AppendLine(string.Format("<p><span>&nbsp;&nbsp;&nbsp;&nbsp; </span><span>&nbsp;{0}  </span>{1}</p>", "Rs",order.OrderSubTotalDiscountExclTax.ToString("#,##0.00")));
            
            ///shipping
            sb.AppendLine(string.Format("<P><span>&nbsp;  </span><span>&nbsp;{0} </span> {1} </P></td>", "Rs", order.OrderShippingExclTax.ToString("#,##0.00")));

            sb.AppendLine("</tr>");

            sb.AppendLine("<tr style=\"background-color:#282828\"><td style=\"padding:5px\" align=\"center\" colspan=\"4\"></td>");
            
            ////total
            sb.AppendLine("<td style=\"font-size:12px;color:#fff;font-family:'Arial';padding-top: 5px;padding-bottom: 5px;\" align=\"center\" colspan=\"0\"> TOTAL</td>");
            sb.AppendLine(string.Format("<td style=\"font-size:12px;color:#fff;font-family:'Arial'\" align=\"center\"><span>&nbsp;{0} </span>{1}</td>","Rs" ,order.OrderTotal.ToString("#,##0.00")));
            #endregion

            sb.AppendLine("</tr></table>");
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

        public virtual void AddOrderNoteTokens(IList<Token> tokens, OrderNote orderNote)
        {
            tokens.Add(new Token("Order.NewNoteText", orderNote.Note, true));

            //event notification
            //_eventPublisher.EntityTokensAdded(orderNote, tokens);
        }

        #endregion
    }
}
