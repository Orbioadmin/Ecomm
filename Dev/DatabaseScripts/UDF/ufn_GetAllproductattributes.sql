/*
 =============================================  
 Author:  Sankar T.S
 Create date: 06 Feb 2015
 Description: It will get the all product attributes 
 Return Format: XML
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAllproductattributes]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAllproductattributes]
	PRINT 'Dropped UDF [dbo].[ufn_GetAllproductattributes]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAllproductattributes]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAllproductattributes](@productid INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( SELECT PA.Name 'Name', PM.TextPrompt, PA.Description 'Description'
	   FROM [dbo].[Product_ProductAttribute_Mapping] PM inner join [dbo].[ProductAttribute] PA on PM.ProductAttributeId = PA.Id  where PM.ProductId=@productid 
 FOR XML PATH('ProductAttribute'), ROOT('ProductAttributes'), TYPE )
		    
		 
	 Return @xmlresult

END



GO
PRINT 'Created UDF: ufn_GetAllproductattributes'
GO