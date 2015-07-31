/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 31 july 2015
 Description: This function will return the product id from order items
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetOrderProductId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetOrderProductId]
	PRINT 'Dropped UDF [dbo].[ufn_GetOrderProductId]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetOrderProductId]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetOrderProductId](@orderItemsXml xml,@OrderItemGuid nvarchar(100))  
RETURNS @TABLE TABLE (ProductId int)  
AS  
BEGIN 

Declare @product Table (ProductId int,OrderItemGuid nvarchar(100))

Insert @product(ProductId,OrderItemGuid)
 SELECT O.D.value('(Product/Id)[1]','int') as ProductId, O.D.value('(OrderItemGuid)[1]','nvarchar(100)') as OrderItemGuid
	FROM @orderItemsXml.nodes('/Order/OrderItems/OrderItem') O(d)
 
    INSERT @TABLE
 Select ProductId from @product where OrderItemGuid = @OrderItemGuid

 RETURN  
   
END



GO
PRINT 'Created UDF [dbo].[ufn_GetOrderProductId]`'
GO  