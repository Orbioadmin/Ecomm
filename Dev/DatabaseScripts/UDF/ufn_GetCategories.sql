/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 29 SEP 2015
 Description: This function will return the categories by discount id
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetCategories]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetCategories]
	PRINT 'Dropped UDF [dbo].[ufn_GetCategories]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetCategories]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetCategories](@discountId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 

	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( SELECT C.Id AS 'Id', Name as 'Name', Description as 'Description', 
		Slug as 'SeName', ParentCategoryId as 'ParentCategoryId'
		FROM Category C 
	   INNER JOIN  UrlRecord UR ON C.Id = UR.EntityId AND   
		UR.EntityName = 'Category'
		AND UR.LanguageId=0
		AND UR.IsActive=1 
		INNER JOIN dbo.Discount_AppliedToCategories dac on C.Id = dac.Category_Id
	  WHERE C.ParentCategoryId=0 AND  C.IncludeInTopMenu=1 AND C.Deleted=0 and dac.Discount_Id = @discountId	
	  ORDER BY DisplayOrder
	FOR XML PATH('Category'),Root('Categories'))
		    
		 
	 Return @xmlresult
	
	
END



GO
PRINT 'Created UDF [dbo].[ufn_GetCategories]`'
GO  