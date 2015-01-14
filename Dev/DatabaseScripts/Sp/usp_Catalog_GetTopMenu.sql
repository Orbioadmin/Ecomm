IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetTopMenu]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetTopMenu]
	PRINT 'Dropped [dbo].[usp_Catalog_GetTopMenu]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetTopMenu]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetTopMenu
# File Path:
# CreatedDate: 28-nov-2014
# Author: Madhu MB
# Description: This stored procedure gets all the categories for top meni 
# Output Parameter: XML output
# Return Parameter: None
# History  of changes:
#--------------------------------------------------------------------------------------
# Version No.	Date of Change		Changed By		Reason for change
#--------------------------------------------------------------------------------------
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Catalog_GetTopMenu]  
 
AS  
BEGIN  
 
 -- WITH TopMenu(Id, Name, Description, Slug, ParentCategoryId, DisplayOrder, Level)
	--AS
	--(
	--	SELECT c.Id, Name, Description, ur.Slug, ParentCategoryId, DisplayOrder, 0 AS Level  
	--	FROM Category C INNER JOIN  UrlRecord UR ON C.Id = UR.EntityId AND   
	--	UR.EntityName = 'Category'
	--	and UR.LanguageId=0
	--	and UR.IsActive=1 
	--	 WHERE IncludeInTopMenu=1 AND Deleted=0 
	--	AND ParentCategoryId=0 --order by DisplayOrder
		
	--	UNION ALL 
		
	--	SELECT C.Id, C.Name, C.Description, UR.Slug, C.ParentCategoryId, C.DisplayOrder, Level + 1  
	--	FROM Category C INNER JOIN  UrlRecord UR ON C.Id = UR.EntityId AND   
	--	UR.EntityName = 'Category'
	--	AND UR.LanguageId=0
	--	AND UR.IsActive=1 
	--	INNER JOIN  TopMenu T ON C.ParentCategoryId= T.Id
	--	where C.IncludeInTopMenu=1 AND C.Deleted=0 	
		
	--)
     
 --    SELECT Id, Name, Description, Slug, ParentCategoryId, DisplayOrder INTO #topmenu 
 --    FROM TopMenu
      
 
     
     DECLARE @XmlResult xml;

	

WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult = ( SELECT C.Id AS 'ns:Id', Name as 'ns:Name', Description as 'ns:Description', 
Slug as 'ns:SeName', ParentCategoryId as 'ns:ParentCategoryId', dbo.[ufn_GetSubCategories](C.Id,'Category')
	  FROM Category C 
	   INNER JOIN  UrlRecord UR ON C.Id = UR.EntityId AND   
		UR.EntityName = 'Category'
		AND UR.LanguageId=0
		AND UR.IsActive=1 
	  WHERE C.ParentCategoryId=0 AND  C.IncludeInTopMenu=1 AND C.Deleted=0 	
	  ORDER BY DisplayOrder
	FOR XML PATH('ns:Category'), ROOT('ns:ArrayOfCategory') );
	   
  SELECT @XmlResult as XmlResult
END

GO
PRINT 'Created the procedure usp_Catalog_GetTopMenu'
GO  


