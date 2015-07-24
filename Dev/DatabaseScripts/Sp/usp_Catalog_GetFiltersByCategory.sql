 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetFiltersByCategory]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetFiltersByCategory]
	PRINT 'Dropped [dbo].[usp_Catalog_GetFiltersByCategory]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetFiltersByCategory]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetFiltersByCategory
# File Path:
# CreatedDate: 03-march-2015
# Author: Sankar T S
# Description: This stored procedure gets all the products for the categoryids
# Output Parameter: resultset output
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

CREATE PROCEDURE [dbo].[usp_Catalog_GetFiltersByCategory] (@categoryIds Varchar(max),@keyWord Varchar(max)) 
AS    
BEGIN 
 
 DECLARE @categoryId Varchar(max) = @categoryIds
 DECLARE @count INT
 SELECT @count = COUNT(*) FROM  dbo.nop_splitstring_to_table(@categoryIds, ',')
 IF(@count>1) 
 BEGIN
    SET @categoryId = 0
 END
  
select distinct SA.DisplayOrder as SpecifiationAttributeOrder,
SA.Id as SpecificationAttributeId,   SA.Name as SpecificationAttributeName,
 SAO.Id as SpecificationAttributeOptionId,SAO.Name as SpecificationAttributeOptionName
 ,SAO.DisplayOrder as SpecifiationAttributeOptionOrder,(select Min(Price) from ufn_GetProductsBySearch(@categoryId,@keyWord)) as MinPrice ,
 (select Max(Price) from ufn_GetProductsBySearch(@categoryId,@keyWord)) as MaxPrice 
from Product p 
inner join  Product_SpecificationAttribute_Mapping PSM on p.Id = psm.ProductId
inner join SpecificationAttributeOption SAO on psm.SpecificationAttributeOptionId = sao.Id
inner join SpecificationAttribute SA on SAO.SpecificationAttributeId = sa.Id
inner join Product_Category_Mapping pcm on p.Id = pcm.ProductId
inner join  ufn_GetProductsBySearch(@categoryId ,@keyWord) PC on p.Id = pc.Id
where PSM.AllowFiltering = 1 AND
','+@categoryIds+',' LIKE '%,'+CAST(pcm.CategoryId AS varchar)+',%' 
 ORDER BY SA.DisplayOrder, SA.Name, SAO.DisplayOrder, SAO.Name
  
END

GO
PRINT 'Created the procedure usp_Catalog_GetFiltersByCategory'
GO  
