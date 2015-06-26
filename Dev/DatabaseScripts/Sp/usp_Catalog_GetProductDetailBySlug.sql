IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetProductDetailBySlug]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetProductDetailBySlug]
	PRINT 'Dropped [dbo].[usp_Catalog_GetProductDetailBySlug]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetProductDetailBySlug]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetProductDetailBySlug
# File Path:
# CreatedDate: 08-jan-2015
# Author: Madhu MB
# Description: This stored procedure gets  the product detail for the slug
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

CREATE PROCEDURE [dbo].[usp_Catalog_GetProductDetailBySlug] (@slug nvarchar(400))
   
AS    
BEGIN    
 --DECLARE @currencyCode nvarchar(5)  
 DECLARE @parentCategoryIds varchar(500)
 DECLARE @categoryId INT , @productId INT
 
 SELECT @productId = EntityId FROM UrlRecord where Slug = @slug and EntityName = 'Product'
and IsActive = 1 and LanguageId = 0

SELECT @categoryId= CategoryId FROM Product_Category_Mapping PCM
INNER JOIN Category C ON PCM.CategoryId = C.Id  where ProductId = @productId
AND C.Deleted = 0 AND C.Published = 1



 select @productId, @categoryId
 SELECT @parentCategoryIds = dbo.ufn_GetAllParentCateoryIds(@categoryId,null)
 SET @parentCategoryIds = @parentCategoryIds + CAST(@categoryId as Nvarchar(100))
 
 SELECT * , IDENTITY(int, 1,1) AS OrderBy
INTO #temp  FROM  [dbo].[nop_splitstring_to_table](@parentCategoryIds, ',')

 
DECLARE @XmlResult xml

SELECT @XmlResult = 
(SELECT Name, Slug AS SeName from Category INNER JOIN #temp ON Category.Id = #temp.data
 LEFT JOIN UrlRecord UR ON Category.Id = UR.EntityId AND UR.IsActive=1
 AND UR.LanguageId = 0 AND EntityName = 'Category' ORDER BY #temp.OrderBy
FOR XML PATH('Category'), ROOT('BreadCrumbs'))

SELECT @XmlResult as XmlResult

SELECT * FROM Product WHERE Id = @productId
   
END  
GO
PRINT 'Created the procedure usp_Catalog_GetProductDetailBySlug'
GO  


