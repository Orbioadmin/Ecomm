using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core;
using Orbio.Core.Domain.Catalog;
using Orbio.Core.Domain.Discounts;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Shipping;
using Orbio.Core.Utility;
using Orbio.Services.Payments;
using Orbio.Services.Taxes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
namespace Orbio.Services.Orders
{
    public class OrderService : IOrderService
    {
         private readonly IDbContext context;
         private readonly IShoppingCartService shoppingCartService;
         private readonly IWebHelper webHelper;
         private readonly IPriceCalculationService priceCalculationService;
         private readonly ITaxCalculationService taxCalculationService;
         private readonly IWorkContext workContext;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
         public OrderService(IDbContext context, IShoppingCartService shoppingCartService,
             IWebHelper webHelper, IPriceCalculationService priceCalculationService, ITaxCalculationService taxCalculationService, IWorkContext workContext)
        {
            this.context = context;
            this.shoppingCartService = shoppingCartService;
            this.webHelper = webHelper;
            this.priceCalculationService = priceCalculationService;
            this.taxCalculationService = taxCalculationService;
            this.workContext = workContext;
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
                    var customer =  workContext.CurrentCustomer;
                    var cart = processOrderRequest.Cart;
                    var appliedDiscounts = new List<int>();
                    var orderDiscountAmount = priceCalculationService.GetAllDiscountAmount(cart, out appliedDiscounts);
                    var taxRates = new Dictionary<decimal, decimal>();
                    var orderTaxTotal = taxCalculationService.CalculateTax(cart,customer, out taxRates);
                    var orderSubTotal = priceCalculationService.GetCartSubTotal(cart, true);
             
                    var shippingStatus = ShippingStatus.NotYetShipped;
                    
                    var order = new Order()
                    {
                        StoreId = processOrderRequest.StoreId,
                        OrderGuid = processOrderRequest.OrderGuid,
                        CustomerId = processOrderRequest.CustomerId,
                        CustomerLanguageId = 1, //hardcoding it for now madhu  mb
                       // CustomerTaxDisplayType = TaxDisplayType.IncludingTax,
                        CustomerIp = webHelper.GetCurrentIpAddress(),
                        OrderSubtotalInclTax = orderSubTotal + orderTaxTotal,
                        OrderSubtotalExclTax = orderSubTotal,
                        OrderSubTotalDiscountInclTax = orderDiscountAmount, //currently no distinction between ordersubtotal or ordertotal discounts madhu mb
                        OrderSubTotalDiscountExclTax = orderDiscountAmount,
                        OrderShippingInclTax = decimal.Zero, //no shipping yet
                        OrderShippingExclTax = decimal.Zero,
                       //////////// PaymentMethodAdditionalFeeInclTax = paymentAdditionalFeeInclTax,
                       //////////// PaymentMethodAdditionalFeeExclTax = paymentAdditionalFeeExclTax,
                        TaxRates =  (from kv in taxRates 
                                     select  string.Format("{0}:{1};   ", 
                                         kv.Key.ToString(CultureInfo.InvariantCulture), kv.Value.ToString(CultureInfo.InvariantCulture))).ToList()
                                         .Aggregate((t1,t2)=>t1+"," + t2),
                        OrderTax = orderTaxTotal, //not caluclating now
                       //////////// OrderTotal = orderTotal.Value,
                        RefundedAmount = decimal.Zero,
                        OrderDiscount = orderDiscountAmount, //need to implement discounts
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

                    foreach (var sci in cart.ShoppingCartItems)
                    {
                        //prices
                        //decimal taxRate = decimal.Zero;
                        decimal scUnitPrice = priceCalculationService.GetFinalPrice(sci, true, false);
                        decimal scSubTotal = priceCalculationService.GetFinalPrice(sci, true, true);
                        decimal sciUnitTaxAmount = taxCalculationService.CalculateTax(scUnitPrice, sci.TaxCategoryId, customer);
                        var sciSubTotalTaxAmount =  taxCalculationService.CalculateTax(scSubTotal, sci.TaxCategoryId, customer);
                        decimal scUnitPriceInclTax = scUnitPrice + sciUnitTaxAmount;
                        decimal scUnitPriceExclTax = scUnitPrice;
                        decimal scSubTotalInclTax = scSubTotal + sciSubTotalTaxAmount;
                        decimal scSubTotalExclTax = scSubTotal;

                        //discounts
                        //Discount scDiscount = null;
                        decimal discountAmount = priceCalculationService.GetDiscountAmount(sci.Discounts, scSubTotal);
                        decimal discountAmountInclTax = discountAmount; // _taxService.GetProductPrice(sc.Product, discountAmount, true, customer, out taxRate);
                        decimal discountAmountExclTax = discountAmount; //.GetProductPrice(sc.Product, discountAmount, false, customer, out taxRate);
                        //if (scDiscount != null && !appliedDiscounts.ContainsDiscount(scDiscount))
                        //    appliedDiscounts.Add(scDiscount);

                        ////attributes
                        //string attributeDescription = _productAttributeFormatter.FormatAttributes(sc.Product, sc.AttributesXml, customer);

                       // var itemWeight = _shippingService.GetShoppingCartItemWeight(sc);
                        var attributeDescription = sci.ProductVariantPriceAdjustments.FormatAttribute();
                        //save order item
                        var orderItem = new OrderItem()
                        {
                            OrderItemGuid = Guid.NewGuid(),                           
                            ProductId = sci.ProductId,
                            UnitPriceInclTax = scUnitPriceInclTax,
                            UnitPriceExclTax = scUnitPriceExclTax,
                            PriceInclTax = scSubTotalInclTax,
                            PriceExclTax = scSubTotalExclTax,
                            OriginalProductCost = priceCalculationService.GetUnitPrice(sci),
                            AttributeDescription = attributeDescription,
                            AttributesXml = sci.AttributeXml,
                            PriceDetailXml = sci.PriceDetailXml,
                            Quantity = sci.Quantity,
                            DiscountAmountInclTax = discountAmountInclTax,
                            DiscountAmountExclTax = discountAmountExclTax,
                            DownloadCount = 0,
                            IsDownloadActivated = false,
                            LicenseDownloadId = 0,
                           // ItemWeight = itemWeight,
                        };
                        order.OrderItems.Add(orderItem);
                    }

                    if (appliedDiscounts.Count > 0)
                    {

                        order.DiscountUsagehistory.AddRange((from duh in appliedDiscounts
                                                             select new DiscountUsageHistory { DiscountId = duh, CreatedOnUtc=DateTime.UtcNow }).ToList());
                    }

                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Order placed",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });

                    //Send email to customer and add order note
                }

             return string.Empty;
         }
    }
}
