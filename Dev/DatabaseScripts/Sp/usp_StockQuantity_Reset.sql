IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_StockQuantity_Reset]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_StockQuantity_Reset]
	PRINT 'Dropped [dbo].[usp_StockQuantity_Reset]'
END	
GO

PRINT 'Creating [dbo].[usp_StockQuantity_Reset]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Order_UpdateOrder
# File Path:
# CreatedDate: 17-Aug-2015
# Author: Sankar T S
# Description: This stored procedure update order data
# Output Parameter: string
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

CREATE PROCEDURE [dbo].[usp_StockQuantity_Reset] (
@storeId int,
@orderXml xml)
AS
BEGIN
DECLARE @orderId int, @OrderItemGuid varchar(100), @orderItemXml xml

   SELECT @orderId = d.value('(OrderId)[1]','nvarchar(100)' )
   from @orderXml.nodes('/Order') O(d)
	
   SELECT @orderItemXml = @orderXml.query('/Order/OrderItems')

	SELECT O.D.value('(OrderItemGuid)[1]','nvarchar(100)') as OrderItemGuid,
		 (select ProductId from dbo.ufn_GetOrderProductId(@orderXml,O.D.value('(OrderItemGuid)[1]','nvarchar(100)'))) as ProductId,
		  O.D.value('(Quantity)[1]','INT') as Quantity
		  into #tempOrderItem
		   FROM 
	  @orderXml.nodes('/Order/OrderItems/OrderItem') O(D)
  
  if(@orderId=0)
  BEGIN
	UPDATE Product SET StockQuantity = (p.StockQuantity-prod.Quantity)
	FROM Product P INNER JOIN #tempOrderItem Prod ON P.Id=prod.ProductId WHERE P.ManageInventoryMethodId=1
  END
  ELSE
  BEGIN
  UPDATE Product SET StockQuantity = (p.StockQuantity+(select Quantity from OrderItem oi where oi.OrderItemGuid=Prod.OrderItemGuid))
	FROM Product P INNER JOIN #tempOrderItem Prod ON P.Id=prod.ProductId
	
	UPDATE Product SET StockQuantity = (p.StockQuantity-prod.Quantity)
	FROM Product P INNER JOIN #tempOrderItem Prod ON P.Id=prod.ProductId WHERE P.ManageInventoryMethodId=1
  END
END

GO
PRINT 'Created the procedure usp_StockQuantity_Reset'
GO  