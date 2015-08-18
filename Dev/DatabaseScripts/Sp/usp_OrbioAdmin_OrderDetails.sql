IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_OrderDetails]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].usp_OrbioAdmin_OrderDetails
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_OrderDetails]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_OrderDetails]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_OrderDetails
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

CREATE PROCEDURE [dbo].[usp_OrbioAdmin_OrderDetails]
	@orderStatusId int,
	@paymentStatusId int,
	@shippingStatusId int,
	@customerId int,
	@createdFromUtc datetime,
	@createdToUtc datetime,
	@billingEmail varchar(150)
AS
BEGIN

	declare @query varchar(300) = '';
	
	if(@orderStatusId is not null and @orderStatusId > 0)
		begin
			Set @query = ' and ord.OrderStatusId = '+convert(varchar,@orderStatusId)+''
					print @query

		end
	if(@paymentStatusId is not null and @paymentStatusId > 0)
		begin
			Set @query =+ ' and ord.PaymentStatusId ='+convert(varchar,@paymentStatusId)+''
		end
	if(@shippingStatusId is not null and @shippingStatusId > 0)
		begin
			Set @query =+ ' and ord.ShippingStatusId = '+convert(varchar,@shippingStatusId)+''
		end
	if(@customerId is not null and @customerId > 0)
		begin
			Set @query =+ ' and ord.CustomerId = '+convert(varchar,@customerId)+''
		end
	if(@createdFromUtc is not null)
		begin
			Set @query =+ ' and Convert(varchar,ord.CreatedOnUtc,103) >= '''+Convert(varchar,@createdFromUtc,103)+''''
		end
	if(@createdToUtc is not null )
		begin
			Set @query =+ ' and ord.CreatedOnUtc <= '+Convert(varchar,@createdToUtc,103)+''
		end
	if(@billingEmail is not null)
		begin
			Set @query =+ ' and adr.Email = '+@billingEmail
		end
	
	exec('Select ord.OrderStatusId,ord.ShippingStatusId,ord.PaymentStatusId,ord.OrderTotal,ord.CreatedOnUtc from [Order] as ord
	inner join [Address] as adr on ord.ShippingAddressId = adr.Id
	where ord.Deleted <> 1'+@query)
END

GO
PRINT 'Created the procedure usp_OrbioAdmin_OrderDetails'
GO  