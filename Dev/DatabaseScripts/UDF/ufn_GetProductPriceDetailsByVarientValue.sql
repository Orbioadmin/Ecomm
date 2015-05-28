/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 28 April 2015
 Description: This function will return the product component and price component by product varient value
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetProductPriceDetailsByVarientValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetProductPriceDetailsByVarientValue]
	PRINT 'Dropped UDF [dbo].[ufn_GetProductPriceDetailsByVarientValue]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetProductPriceDetailsByVarientValue]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetProductPriceDetailsByVarientValue](@productid INT,@variantValueId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = (select (select PC.[ComponentName] as 'Name',PPPC.[Weight] as 'Weight',PPPC.[UnitPrice] as 'UnitPrice' from [dbo].[ProductComponent] PC
			inner join [dbo].[Product_ProductVarientValue_ProductComponent_Mapping] PPPC  on PPPC.[ComponentId] = PC.Id
			LEFT OUTER Join ProductVariantAttributeValue PVA on PVA.Id = PPPC.[ProductVarientAttributeValueId]
			where PPPC.ProductId = @productid and PC.IsActive = 1 and PC.Deleted=0 and PPPC.ProductVarientAttributeValueId = @variantValueId
			FOR XML PATH('ProductComponent'), ROOT('ProductComponents'), TYPE )
			,
			(select PC.[Name] as 'Name',PPPC.Price as 'Price',PPPC.Percentage as 'Percentage', PPPC.Itemrate as 'ItemPrice' 
			from [dbo].[PriceComponent] PC
			inner join [dbo].[Product_ProductVarientValue_PriceComponent_Mapping] PPPC  on PPPC.[PriceComponentId] = PC.Id
			LEFT OUTER Join ProductVariantAttributeValue PVA on PVA.Id = PPPC.[ProductVarientAttributeValueId]
			where PPPC.ProductId = @productid and PC.IsActive = 1 and PC.Deleted=0 and PPPC.ProductVarientAttributeValueId = @variantValueId
			FOR XML PATH('PriceComponent'), ROOT('PriceComponents'), TYPE )
		FOR XML PATH('ProductPriceDetail'))

		 
	 Return @xmlresult

END




GO
PRINT 'Created UDF [dbo].[ufn_GetProductPriceDetailsByVarientValue]`'
GO  