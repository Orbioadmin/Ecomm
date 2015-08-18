/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 16 Aug 2015
 Description: This function will return order items detail
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAdminOrderProductDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAdminOrderProductDetails]
	PRINT 'Dropped UDF [dbo].[ufn_GetAdminOrderProductDetails]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAdminOrderProductDetails]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAdminOrderProductDetails](@orderId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.OrderItem' AS ns)
		SELECT @xmlResult = (Select ori.Id,ori.OrderItemGuid,ori.OrderId,ori.ProductId,ori.Quantity,ori.UnitPriceExclTax,ori.UnitPriceInclTax,ori.PriceInclTax,ori.PriceExclTax,ori.DiscountAmountInclTax,
					ori.OriginalProductCost,ori.AttributeDescription,ori.AttributesXml,ori.DownloadCount,ori.IsDownloadActivated,ori.LicenseDownloadId,
					ori.ItemWeight,ori.PriceDetailXml,[dbo].[ufn_GetOrderProductDetails](ori.Id) from OrderItem ori inner join [Order] ord on ori.OrderId = ord.Id
					inner join Product prd on ori.ProductId = prd.Id where ori.OrderId = @orderId for xml path('OrderItem'),Root('OrderItems'),type)
		    
	 Return @xmlresult

END




GO
PRINT 'Created the UDF ufn_GetAdminOrderProductDetails'
GO  