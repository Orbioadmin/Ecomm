IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_Order_Details]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_Order_Details]
	PRINT 'Dropped [dbo].[usp_Customer_Order_Details]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_Order_Details]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: [dbo].[usp_Customer_Order_Details]
# File Path:
# CreatedDate: 18-April-2015
# Author: Roshni
# Description: This stored procedure gets  the product detail for the slug
# Output Parameter: XML output
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

CREATE PROCEDURE [dbo].[usp_Customer_Order_Details]
	-- Add the parameters for the stored procedure here
	@curCustomerId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
DECLARE @XmlResult xml
    -- Insert statements for procedure here
	SELECT @XmlResult =  (select Orders.Id AS OrderNumber,Orders.OrderTotal as TotalPrice,Orders.CreatedOnUtc as OrderDate,
	Orders.OrderStatusId As OrderStatus,COUNT(OI.Id) as Quantity
	 from [dbo].[Order] Orders Inner join [dbo].[OrderItem] OI on OI.OrderId=Orders.Id 
	 where Orders.CustomerId=@curCustomerId group by Orders.Id,Orders.OrderTotal,Orders.CreatedOnUtc,Orders.OrderStatusId
	 FOR XML PATH('OrderSummary'),Root('ArrayOfOrderSummary')) --, type) FOR XML PATH('OrderDetails'),type)
	 SELECT @XmlResult as XmlResult
END
GO
PRINT 'Created the procedure [dbo].[usp_Customer_Order_Details]'
GO  


