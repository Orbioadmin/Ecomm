
/*
 =============================================  
 Author:  Madhu MB
 Create date: 16 dec 2014
 Description: It will get the parent categories  
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAllParentCateoryIds]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAllParentCateoryIds]
	PRINT 'Dropped UDF [dbo].[ufn_GetAllParentCateoryIds]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAllParentCateoryIds]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAllParentCateoryIds](@categoryId INT, @parentIds varchar(500))
RETURNS varchar(500)
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	  IF(@parentIds is null)
	  begin
		SET @parentIds = ''
	  end
	  IF EXISTS(SELECT 1 FROM Category WHERE Id = @categoryId AND ParentCategoryId<>0)
	  BEGIN
		SELECT  @parentIds = @parentIds + CAST(ParentCategoryId as varchar(500))+ ',' +
		[dbo].[ufn_GetAllParentCateoryIds](ParentCategoryId,@parentIds)	
			FROM Category 
		WHERE Id = @categoryId
	  END
	
	  RETURN @parentIds
END

GO
PRINT 'Created UDF: ufn_GetAllParentCateoryIds'
GO

