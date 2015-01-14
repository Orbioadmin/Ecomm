
/*
 =============================================  
 Author:  Madhu MB
 Create date: 02 dec 2014
 Description: It will get the Sub categories  
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetSubCategories]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetSubCategories]
	PRINT 'Dropped UDF [dbo].[ufn_GetSubCategories]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetSubCategories]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetSubCategories](@parentCategoryId INT, @entityName nvarchar(400))
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 

	   DECLARE @xmlResult xml;
	 
		WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( SELECT C.Id  AS 'ns:Id', C.Name as 'ns:Name', C.Description as 'ns:Description', 
			Slug as 'ns:SeName', C.ParentCategoryId
	as 'ns:ParentCategoryId', dbo.[ufn_GetSubCategories](C.Id,'Category')
	  FROM Category C INNER JOIN  UrlRecord UR ON C.Id = UR.EntityId AND   
		UR.EntityName = @entityName
		AND UR.LanguageId=0
		AND UR.IsActive=1 WHERE C.ParentCategoryId = @parentCategoryId	 AND C.IncludeInTopMenu=1 AND C.Deleted=0   
	  ORDER BY C.displayorder FOR XML PATH('ns:Category'), ROOT('ns:SubCategories'), TYPE )
		    
		 
	 Return @xmlresult
	
	
END

GO
PRINT 'Created UDF: ufn_GetSubCategories'
GO

