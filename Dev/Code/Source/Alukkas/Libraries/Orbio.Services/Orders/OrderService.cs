using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Orders;
using Orbio.Services.Payments;
using Orbio.Services.Utility;

namespace Orbio.Services.Orders
{
    public class OrderService : IOrderService
    {
         private readonly IDbContext context;
         private readonly IShoppingCartService shoppingCartService;
        /// <summary>
        /// instantiates Store service type
        /// </summary>
        /// <param name="context">db context</param>
         public OrderService(IDbContext context, IShoppingCartService shoppingCartService)
        {
            this.context = context;
            this.shoppingCartService = shoppingCartService;
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
                 return orderDetails;
             }
             return new List<OrderSummary>();
         }


         public string PlaceOrder(ProcessPaymentRequest processPaymentRequest)
         {
             if (processPaymentRequest.OrderGuid == Guid.Empty)
                 processPaymentRequest.OrderGuid = Guid.NewGuid();

             //load and validate customer shopping cart
                IList<ShoppingCartItem> cart = null;
                
                    //load shopping cart
                    //cart = customer.ShoppingCartItems
                    //    .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                    //    .Where(sci => sci.StoreId == processPaymentRequest.StoreId)
                    //    .ToList();
                cart = shoppingCartService.GetCartItems("select", 0, ShoppingCartType.ShoppingCart, 0, processPaymentRequest.CustomerId, 0, 0);
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
                if (processPaymentRequest.Success)
                {
                }

             return string.Empty;
         }
    }
}
