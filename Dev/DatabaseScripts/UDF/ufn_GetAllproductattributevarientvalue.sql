/*
 =============================================  
 Author:  Sankar T.S
 Create date: 06 Feb 2015
 Description: It will get the all product attribute varient values
 Return format: XML
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAllproductattributevarientvalue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAllproductattributevarientvalue]
	PRINT 'Dropped UDF [dbo].[ufn_GetAllproductattributevarientvalue]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAllproductattributevarientvalue]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAllproductattributevarientvalue](@productid INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( SELECT PVAV.*
	  FROM dbo.Product_ProductAttribute_Mapping PAM
		inner join dbo.ProductVariantAttributeValue PVAV on PVAV.ProductVariantAttributeId=PAM.Id where PAM.ProductId=@productid 
 FOR XML PATH('ProductVarientAttributeValue'), ROOT('ProductVarientAttributeValues'), TYPE )
		    
		 
	 Return @xmlresult

END



GO
PRINT 'Created UDF: ufn_GetAllproductattributevarientvalue'
GO