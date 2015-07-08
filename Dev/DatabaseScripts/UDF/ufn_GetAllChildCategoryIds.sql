
/*
 =============================================  
 Author:  Madhu MB
 Create date: 08 jul 2014
 Description: It will get all child category ids
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAllChildCategoryIds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAllChildCategoryIds]
	PRINT 'Dropped UDF [dbo].[ufn_GetAllChildCategoryIds]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAllChildCategoryIds]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAllChildCategoryIds](@productId INT, @categoryId INT, @childIds varchar(500))
RETURNS varchar(500)
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	  IF(@childIds is null)
	  begin
		SET @childIds = ''
	  end
	  IF EXISTS(SELECT 1 FROM Category C INNER JOIN Product_Category_Mapping PCM
	  ON C.Id = PCM.CategoryId WHERE PCM.ProductId=@productId AND ParentCategoryId=@categoryId)
	  BEGIN
		SELECT  @childIds = @childIds + CAST(C.Id as varchar(500))+ ',' +
		[dbo].[ufn_GetAllChildCategoryIds](@productId, C.Id,@childIds)	
			FROM Category C INNER JOIN Product_Category_Mapping PCM
	  ON C.Id = PCM.CategoryId WHERE PCM.ProductId=@productId AND ParentCategoryId=@categoryId
	  END
	
	  RETURN @childIds
END

GO
PRINT 'Created UDF: ufn_GetAllChildCategoryIds'
GO

