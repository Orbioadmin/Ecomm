/*
 ===================================================================================================================================================
 Author:  Madhu MB
 Create date: 08 jul 2015
 Description: This function will return relevant categoryids
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetPreferredCategoryIds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetPreferredCategoryIds]
	PRINT 'Dropped UDF [dbo].[ufn_GetPreferredCategoryIds]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetPreferredCategoryIds]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ufn_GetPreferredCategoryIds 
(
	@productId int	
)
RETURNS @temp Table(CategoryId int)
AS
BEGIN
	
	DECLARE @categoryid int, @allCategoryIds nvarchar(100)
    SELECT Top 1 @categoryid = CategoryId FROM Product_Category_Mapping PCM
	INNER JOIN Category C ON PCM.CategoryId = C.Id  where ProductId = @productId
	AND C.Deleted = 0 AND C.Published = 1
    ORDER BY pcm.DisplayOrder 
 
    
    SELECT @allCategoryIds = dbo.ufn_GetAllParentCateoryIds(@categoryId,null)
    SET @allCategoryIds = @allCategoryIds + CAST(@categoryId as Nvarchar(100))
   SELECT @allCategoryIds = @allCategoryIds + ',' + dbo.ufn_GetAllChildCategoryIds(@productId,@categoryid,null)
 
  INSERT INTO @temp
  SELECT id  FROM  dbo.nop_splitstring_to_table(@allCategoryIds , ',')
  inner join Category on Id = data order by ParentCategoryId
  RETURN
END

GO
PRINT 'Created the UDF ufn_GetPreferredCategoryIds'
GO  