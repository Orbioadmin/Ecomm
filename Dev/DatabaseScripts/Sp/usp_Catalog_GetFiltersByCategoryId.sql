 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetFiltersByCategoryId]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetFiltersByCategoryId]
	PRINT 'Dropped [dbo].[usp_Catalog_GetFiltersByCategoryId]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetFiltersByCategoryId]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetFiltersByCategoryId
# File Path:
# CreatedDate: 17-dec-2014
# Author: Madhu MB
# Description: This stored procedure gets all the products for the categoryid
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

CREATE PROCEDURE [dbo].[usp_Catalog_GetFiltersByCategoryId] (@categoryId INT) 
AS    
BEGIN 
 
 

select distinct SA.DisplayOrder as SpecifiationAttributeOrder,
SA.Id as SpecificationAttributeId,   SA.Name as SpecificationAttributeName,
 SAO.Id as SpecificationAttributeOptionId,SAO.Name as SpecificationAttributeOptionName
 ,SAO.DisplayOrder as SpecifiationAttributeOptionOrder
from Product p 
inner join  Product_SpecificationAttribute_Mapping PSM on p.Id = psm.ProductId
inner join SpecificationAttributeOption SAO on psm.SpecificationAttributeOptionId = sao.Id
inner join SpecificationAttribute SA on SAO.SpecificationAttributeId = sa.Id
inner join Product_Category_Mapping pcm on p.Id = pcm.ProductId
inner join  ufn_GetProductsByCategoryId(@categoryId) PC on p.Id = pc.Id
where --PSM.AllowFiltering = 1 temp commented for dev   
--AND 
pcm.CategoryId = @categoryId
 ORDER BY SA.DisplayOrder, SA.Name, SAO.DisplayOrder, SAO.Name
  
END
GO
PRINT 'Created the procedure usp_Catalog_GetFiltersByCategoryId'
GO  
