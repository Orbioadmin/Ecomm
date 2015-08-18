IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Get_AdminOrderDetails]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Get_AdminOrderDetails]
	PRINT 'Dropped [dbo].[usp_Get_AdminOrderDetails]'
END	
GO

PRINT 'Creating [dbo].[usp_Get_AdminOrderDetails]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Get_AdminOrderDetails
# File Path:
# CreatedDate: 16-Aug-2015
# Author: Sankar T S
# Description: This stored procedure return list of orders
# Output Parameter: none
# Return Parameter: None
# History  of changes:
#--------------------------------------------------------------------------------------
# Version No.	Date of Change		Changed By		Reason for change
#--------------------------------------------------------------------------------------
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Get_AdminOrderDetails]
	@orderStatusId int = null,
	@paymentStatusId int = null,
	@shippingStatusId int = null,
	@customerId int = null,
	@createdFromUtc datetime = null,
	@createdToUtc datetime = null,
	@billingEmail varchar(150) = null,
	@orderNo int = null
AS
BEGIN

	declare @query varchar(300) = '';
	
	if(@orderStatusId is not null and @orderStatusId > 0)
		begin
			Set @query += ' and ord.OrderStatusId = '+convert(varchar,@orderStatusId)+''

		end
	if(@paymentStatusId is not null and @paymentStatusId > 0)
		begin
			Set @query += ' and ord.PaymentStatusId ='+convert(varchar,@paymentStatusId)+''
		end
	if(@shippingStatusId is not null and @shippingStatusId > 0)
		begin
			Set @query += ' and ord.ShippingStatusId = '+convert(varchar,@shippingStatusId)+''
		end
	if(@customerId is not null and @customerId > 0)
		begin
			Set @query += ' and ord.CustomerId = '+convert(varchar,@customerId)+''
		end
	if(@createdFromUtc is not null)
		begin
			Set @query += ' and Convert(datetime,ord.CreatedOnUtc,103) >= Convert(datetime,'''+Convert(varchar,@createdFromUtc,103)+''',103)'
		end
	if(@createdToUtc is not null )
		begin
			Set @query += ' and Convert(datetime,ord.CreatedOnUtc,103) <= Convert(datetime,'''+convert(varchar,@createdToUtc,103)+''',103)'
		end
	if(@billingEmail is not null)
		begin
			Set @query += ' and adr.Email = '''+@billingEmail+''''
		end
	if(@orderNo is not null and @orderNo > 0)
		begin
			Set @query += ' and ord.Id = '+convert(varchar,@orderNo)+''
		end
	
	exec('Select ord.Id as OrderId,ord.OrderGuid,ord.StoreId,ord.CustomerId,ord.BillingAddressId,
		ord.ShippingAddressId, ord.OrderStatusId, ord.ShippingStatusId, ord.PaymentStatusId,
		ord.PaymentMethodSystemName, ord.CurrencyRate,ord.CustomerTaxDisplayTypeId,
		 ord.OrderSubtotalInclTax,ord.OrderSubtotalExclTax,ord.OrderSubTotalDiscountInclTax,
		 ord.OrderSubTotalDiscountExclTax,ord.OrderShippingInclTax, ord.OrderShippingExclTax,
		 ord.PaymentMethodAdditionalFeeInclTax,ord.PaymentMethodAdditionalFeeExclTax,
		 ord.TaxRates,OrderTax, ord.OrderDiscount, ord.OrderTotal,ord.RefundedAmount, ord.RewardPointsWereAdded,
		 ord.CustomerLanguageId,ord.AffiliateId, ord.CustomerIp, ord.AllowStoringCreditCardNumber,
		 ord.Deleted, ord.CreatedOnUtc from [Order] as ord
	inner join [Address] as adr on ord.ShippingAddressId = adr.Id
	where ord.Deleted <> 1'+@query)
END




GO
PRINT 'Created the procedure usp_Get_AdminOrderDetails'
GO  