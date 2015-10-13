IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetCategoryDetails]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetCategoryDetails]
	PRINT 'Dropped [dbo].[usp_Catalog_GetCategoryDetails]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetCategoryDetails]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetCategoryDetails
# File Path:
# CreatedDate: 11-SEP-2015
# Author: Roshni
# Description: This stored procedure get all category details by id
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
CREATE PROCEDURE [dbo].[usp_Catalog_GetCategoryDetails] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @xmlResult xml
	
	SELECT @xmlResult=(select (select C.*,PC.RelativeUrl as PictureUrl,UR.Slug,(select C.Id,c.Name,c.ParentCategoryId from dbo.Category C where c.Deleted='false' FOR XML PATH ('CategoryList'),ROOT('ParentCategoryList'), Type),
							  (select CT.Id,CT.Name from dbo.CategoryTemplate CT FOR XML PATH ('Templates'),ROOT('CategoryTemplateList'), Type)		
							   from dbo.Category C left join dbo.UrlRecord UR on C.Id= UR.EntityId 
							   left join dbo.Picture PC on C.PictureId=PC.Id
							   where C.Id=@id and UR.EntityName='Category'
							   FOR XML PATH ('Categories'),Type),
							   
	
							  (select P.Id,P.Name,PCM.IsFeaturedProduct,P.DisplayOrder from dbo.Product P join dbo.Product_Category_Mapping PCM on P.Id=pcm.ProductId where PCM.CategoryId=@id and P.Deleted='false'FOR XML PATH ('ProductDetails'),ROOT('Products'), Type),
							  (select D.Id,D.Name from dbo.Discount D join dbo.Discount_AppliedToCategories DAC on D.Id=DAC.Discount_Id where DAC.Category_Id=@id FOR XML PATH ('DiscountDetails'),ROOT('Discount'), Type)
							   FOR XML PATH('CategoryDetails'))
							   
							   select @xmlResult as XmlResult
	
END



GO
PRINT 'Created the procedure usp_Catalog_GetCategoryDetails'
GO  


