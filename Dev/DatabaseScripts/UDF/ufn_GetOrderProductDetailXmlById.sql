/*
 ===================================================================================================================================================
 Author:  Madhu M B
 Create date: 05 aug 2015
 Description: This function will return the order product detail
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetOrderProductDetailXmlById]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetOrderProductDetailXmlById]
	PRINT 'Dropped UDF [dbo].[ufn_GetOrderProductDetailXmlById]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetOrderProductDetailXmlById]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetOrderProductDetailXmlById](@productId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( Select Id,Name from Product where Id = @productId for xml path('Product'),type )
		
	 Return @xmlresult

END



GO
PRINT 'Created UDF [dbo].[ufn_GetOrderProductDetailXmlById]`'
GO  