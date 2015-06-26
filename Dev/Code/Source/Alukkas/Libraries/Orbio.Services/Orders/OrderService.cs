using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Utility;
using Orbio.Services.Payments;

namespace Orbio.Services.Orders
{
    public class OrderService : IOrderService
    {
         private readonly IDbContext context;
         private readonly IShoppingCartService shoppingCartService;
         private readonly IWebHelper webHelper;
         private readonly IPriceCalculationService priceCalculationService;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
         public OrderService(IDbContext context, IShoppingCartService shoppingCartService, IWebHelper webHelper, IPriceCalculationService priceCalculationService)
        {
            this.context = context;
            this.shoppingCartService = shoppingCartService;
            this.webHelper = webHelper;
            this.priceCalculationService = priceCalculationService;
        }

         /// <summary>
         /// Get shopping cart items
         /// </summary>
         /// <param name="action">Action</param>
         public List<OrderSummary> GetOrderDetails(int curCustomerId)
         {
             var sqlParamList = new List<SqlParameter>();
             sqlParamList.Add(new SqlParameter { ParameterName = "@curCustomerId", Value = curCustomerId, DbType = System.Data.DbType.Int32 });

             var result = context.ExecuteFunction<XmlResultSet>("usp_Customer_Order_Details",
                           sqlParamList.ToArray()
                           ).FirstOrDefault();
             if (result != null)
             {
                 var orderDetails = Serializer.GenericDeSerializer<List<OrderSummary>>(result.XmlResult);
                 foreach(var item in orderDetails)
                 {
                     item.OrderStatus = Enum.Parse(typeof(OrderStatus), item.OrderStatus).ToString();
                 }
                 return orderDetails;
             }
             return new List<OrderSummary>();
         }


         public string PlaceOrder(ProcessOrderRequest processOrderRequest)
         {
             if (processOrderRequest.OrderGuid == Guid.Empty)
                 processOrderRequest.OrderGuid = Guid.NewGuid();

             //load and validate customer shopping cart
             var cart = processOrderRequest.Cart;
                
                    //load shopping cart
                    //cart = customer.ShoppingCartItems
                    //    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    //    .Where(sci => sci.StoreId == processPaymentRequest.StoreId)
                    //    .ToList();
               // cart = processOrderRequest.ShoppingCartItems;
                ////min totals validation
                //if (!processPaymentRequest.IsRecurringPayment)
                //{
                //    bool minOrderSubtotalAmountOk = ValidateMinOrderSubtotalAmount(cart);
                //    if (!minOrderSubtotalAmountOk)
                //    {
                //        decimal minOrderSubtotalAmount = _currencyService.ConvertFromPrimaryStoreCurrency(_orderSettings.MinOrderSubtotalAmount, _workContext.WorkingCurrency);
                //        throw new NopException(string.Format(_localizationService.GetResource("Checkout.MinOrderSubtotalAmount"), _priceFormatter.FormatPrice(minOrderSubtotalAmount, true, false)));
                //    }
                //    bool minOrderTotalAmountOk = ValidateMinOrderTotalAmount(cart);
                //    if (!minOrderTotalAmountOk)
                //    {
                //        decimal minOrderTotalAmount = _currencyService.ConvertFromPrimaryStoreCurrency(_orderSettings.MinOrderTotalAmount, _workContext.WorkingCurrency);
                //        throw new NopException(string.Format(_localizationService.GetResource("Checkout.MinOrderTotalAmount"), _priceFormatter.FormatPrice(minOrderTotalAmount, true, false)));
                //    }
                //}

                string shippingMethodName = "", shippingRateComputationMethodSystemName = "";

                //var shippingOption = customer.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, processPaymentRequest.StoreId);
                //if (shippingOption != null)
                //{
                //    shippingMethodName = shippingOption.Name;
                //    shippingRateComputationMethodSystemName = shippingOption.ShippingRateComputationMethodSystemName;
                //}

                //shipping calculations goes here
                // next tax calculations
                if (processOrderRequest.Success)
                {
                    var shippingStatus = ShippingStatus.NotYetShipped;
                    
                    var order = new Order()
                    {
                        StoreId = processOrderRequest.StoreId,
                        OrderGuid = processOrderRequest.OrderGuid,
                        CustomerId = processOrderRequest.CustomerId,
                        CustomerLanguageId = 1, //hardcoding it for now madhu  m b
                       // CustomerTaxDisplayType = TaxDisplayType.IncludingTax,
                        CustomerIp = webHelper.GetCurrentIpAddress(),
                       //////////// OrderSubtotalInclTax = orderSubTotalInclTax,
                       //////////// OrderSubtotalExclTax = orderSubTotalExclTax,
                       //////////// OrderSubTotalDiscountInclTax = orderSubTotalDiscountInclTax,
                       //////////// OrderSubTotalDiscountExclTax = orderSubTotalDiscountExclTax,
                       //////////// OrderShippingInclTax = orderShippingTotalInclTax.Value,
                       //////////// OrderShippingExclTax = orderShippingTotalExclTax.Value,
                       //////////// PaymentMethodAdditionalFeeInclTax = paymentAdditionalFeeInclTax,
                       //////////// PaymentMethodAdditionalFeeExclTax = paymentAdditionalFeeExclTax,
                       //////////// TaxRates = string.Empty, //not calculating now madhu mb
                       ////////////// OrderTax = orderTaxTotal, //not caluclating now
                       //////////// OrderTotal = orderTotal.Value,
                        RefundedAmount = decimal.Zero,
                       // OrderDiscount = orderDiscountAmount, //need to implement discounts
                       // CheckoutAttributeDescription = checkoutAttributeDescription,
                       // CheckoutAttributesXml = checkoutAttributesXml,
                       // CustomerCurrencyCode = customerCurrencyCode,
                       // CurrencyRate = customerCurrencyRate,
                       // AffiliateId = affiliateId,
                        OrderStatus = OrderStatus.Pending,
                        //AllowStoringCreditCardNumber = processPaymentResult.AllowStoringCreditCardNumber,
                        //CardType = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processOrderRequest.CreditCardType) : string.Empty,
                        //CardName = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processOrderRequest.CreditCardName) : string.Empty,
                        //CardNumber = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processOrderRequest.CreditCardNumber) : string.Empty,
                        //MaskedCreditCardNumber = _encryptionService.EncryptText(_paymentService.GetMaskedCreditCardNumber(processOrderRequest.CreditCardNumber)),
                        //CardCvv2 = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processOrderRequest.CreditCardCvv2) : string.Empty,
                        //CardExpirationMonth = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processOrderRequest.CreditCardExpireMonth.ToString()) : string.Empty,
                        //CardExpirationYear = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processOrderRequest.CreditCardExpireYear.ToString()) : string.Empty,
                        PaymentMethodSystemName = processOrderRequest.PaymentMethodSystemName,
                        //AuthorizationTransactionId = processPaymentResult.AuthorizationTransactionId,
                        //AuthorizationTransactionCode = processPaymentResult.AuthorizationTransactionCode,
                        //AuthorizationTransactionResult = processPaymentResult.AuthorizationTransactionResult,
                        //CaptureTransactionId = processPaymentResult.CaptureTransactionId,
                        //CaptureTransactionResult = processPaymentResult.CaptureTransactionResult,
                        //SubscriptionTransactionId = processPaymentResult.SubscriptionTransactionId,
                        PurchaseOrderNumber = processOrderRequest.PurchaseOrderNumber,
                        //PaymentStatus = processPaymentResult.NewPaymentStatus,
                        PaidDateUtc = null,                     
                        ShippingStatus = shippingStatus,
                        ShippingMethod = shippingMethodName,
                        ShippingRateComputationMethodSystemName = shippingRateComputationMethodSystemName,
                       // CustomValuesXml = processOrderRequest.SerializeCustomValues(),
                       // VatNumber = vatNumber,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                }

             return string.Empty;
         }
    }
}
