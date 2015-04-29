/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 28 April 2015
 Description: This function will return the product component and price component
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetProductPriceDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetProductPriceDetails]
	PRINT 'Dropped UDF [dbo].[ufn_GetProductPriceDetails]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetProductPriceDetails]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetProductPriceDetails](@productid INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = (select (select PC.[ComponentName] as 'Name',PC.[Weight] as 'Weight',PC.[UnitPrice] as 'UnitPrice' from [dbo].[ProductComponent] PC
			inner join [dbo].[Product_ProductComponent_Mapping] PCM on PCM.ComponentId = PC.ComponentId
			where PCM.ProductId = @productid and PC.IsActive = 1 and PC.Deleted=0
			FOR XML PATH('ProductComponent'), ROOT('ProductComponents'), TYPE )
			,
			(select PC.[Name] as 'Name',PPCM.Price as 'Price',PPCM.Percentage as 'Percentage', PPCM.Itemrate as 'ItemPrice' 
			from [dbo].[PriceComponent] PC
			inner join [dbo].[Product_PriceComponent_Mapping] PPCM on PPCM.PricecomponentId = PC.PriceComponentId
			where PPCM.ProductId = @productid and PC.IsActive = 1 and PC.Deleted=0
			FOR XML PATH('PriceComponent'), ROOT('PriceComponents'), TYPE )
		FOR XML PATH('ProductPriceDetail'))

		 
	 Return @xmlresult

END



GO
PRINT 'Created UDF [dbo].[ufn_GetProductPriceDetails]`'
GO  