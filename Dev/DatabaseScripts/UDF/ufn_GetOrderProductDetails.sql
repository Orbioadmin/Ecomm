/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 27 july 2015
 Description: This function will return the order product detail
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetOrderProductDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetOrderProductDetails]
	PRINT 'Dropped UDF [dbo].[ufn_GetOrderProductDetails]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetOrderProductDetails]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetOrderProductDetails](@orderItemId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( Select prd.Name from OrderItem ori inner join [Order] ord on ori.OrderId = ord.Id
		inner join Product prd on ori.ProductId = prd.Id where ori.Id = @orderItemId for xml path('Product'),Root('Product'),type )
		    
	 Return @xmlresult

END



GO
PRINT 'Created UDF [dbo].[ufn_GetOrderProductDetails]`'
GO  