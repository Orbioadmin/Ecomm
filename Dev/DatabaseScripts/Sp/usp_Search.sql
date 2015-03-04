 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Search]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Search]
	PRINT 'Dropped [dbo].[usp_Search]'
END	
GO

PRINT 'Creating [dbo].[usp_Search]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Search
# File Path:
# CreatedDate: 02-march-2015
# Author: Sankar T S
# Description: This stored procedure gets all the products by search 
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

Create PROCEDURE [dbo].[usp_Search] (@slug nvarchar(400),  
@entityName nvarchar(400), @specificationFilterIds nvarchar(500)=null, @minPrice decimal(18,4)=null,
@maxPrice decimal(18,4)=null,@keyword varchar(max))  
   
AS    
BEGIN    
 --DECLARE @currencyCode nvarchar(5)  

 if(@slug = 'Search')
 begin

declare @temptablefilterIds as table
	(
	id int
	)
SELECT SAO.SpecificationAttributeId,FILTERS.* , IDENTITY(int, 1,1) AS OrderBy
INTO temptablefilterIds FROM  [dbo].[nop_splitstring_to_table](@specificationFilterIds, ',') FILTERS
INNER JOIN SpecificationAttributeOption SAO ON SAO.Id = FILTERS.data

declare @temptableproduct as table
	(
	id int
	)
SELECT  * INTO temptableproduct FROM  ufn_GetProductsBySearch(0,@keyword)




declare @catIds VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.CategoryId)
         FROM temptableproduct p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
        ,1,1,'') Color
FROM temptableproduct p)


DECLARE @filterId1 NVARCHAR(100)
while EXISTS(SELECT 1 FROM temptablefilterIds)
BEGIN
	SELECT TOP 1 @filterId1 = SAO.SpecificationAttributeId from temptablefilterIds INNER JOIN
	SpecificationAttributeOption SAO ON temptablefilterIds.data = SAO.Id 
	
	  DELETE FROM temptableproduct WHERE temptableproduct.Id NOT IN (
	 select ProductId from temptableproduct INNER JOIN
	  Product_SpecificationAttribute_Mapping PSM 
	 ON 
	 temptableproduct.Id = PSM.ProductId --and psm.AllowFiltering=1
	 WHERE PSM.SpecificationAttributeOptionId IN (
	 SELECT data FROM temptablefilterIds WHERE SpecificationAttributeId = @filterId1)
	 )
	
	DELETE FROM temptablefilterIds WHERE SpecificationAttributeId = @filterId1
	
END
 
 IF(@minPrice IS NOT NULL AND @maxPrice IS NOT NULL)
 BEGIN
   DELETE FROM temptableproduct WHERE temptableproduct.Price<@minPrice OR temptableproduct.Price>@maxPrice
 END


DECLARE @XmlResult1 xml


--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult1 = (select 'SearchTemplate.ProductsInGridOrLines' as 'TemplateViewPath',@catIds 'CategoryId',(select count(*) FROM  temptableproduct  PC 
inner join Category on PC.CategoryId = Category.Id
where ','+@catIds+',' LIKE '%,'+CAST(Category.Id AS varchar)+',%'
and ParentCategoryId=0) 'Totalcount',
(select * FROM  temptableproduct  PC 
inner join Category on PC.CategoryId = Category.Id
where ','+@catIds+',' LIKE '%,'+CAST(Category.Id AS varchar)+',%'
and ParentCategoryId=0
FOR XML PATH('Product'), ROOT('Products') , type)
FOR XML PATH('Search') )


SELECT @XmlResult1 as XmlResult

drop table temptableproduct
drop table temptablefilterIds


 end
 else
 begin
 
 DECLARE @parentCategoryIds varchar(500)
 DECLARE @categoryId INT 
 
 SELECT @categoryId = EntityId FROM UrlRecord where Slug = @slug and EntityName = @entityName
and IsActive = 1 and LanguageId = 0
 
 SELECT @parentCategoryIds = dbo.ufn_GetAllParentCateoryIds(@categoryId,null)
 
 SELECT * , IDENTITY(int, 1,1) AS OrderBy
INTO #temp  FROM  [dbo].[nop_splitstring_to_table](@parentCategoryIds, ',')

  SELECT SAO.SpecificationAttributeId,FILTERS.* , IDENTITY(int, 1,1) AS OrderBy
INTO #filterIds FROM  [dbo].[nop_splitstring_to_table](@specificationFilterIds, ',') FILTERS
INNER JOIN SpecificationAttributeOption SAO ON SAO.Id = FILTERS.data


SELECT    * INTO #products FROM  ufn_GetProductsBySearch(@categoryId,@keyword)

declare @catIds1 VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.CategoryId)
         FROM #products p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
        ,1,1,'') Color
FROM #products p)

DECLARE @filterId NVARCHAR(100)
while EXISTS(SELECT 1 FROM #filterIds)
BEGIN
	SELECT TOP 1 @filterId = SAO.SpecificationAttributeId from #filterIds INNER JOIN
	SpecificationAttributeOption SAO ON #filterIds.data = SAO.Id 
	
	
	  DELETE FROM #products WHERE #products.Id NOT IN (
	 select ProductId from #products INNER JOIN
	  Product_SpecificationAttribute_Mapping PSM 
	 ON 
	 #products.Id = PSM.ProductId --and psm.AllowFiltering=1
	 WHERE PSM.SpecificationAttributeOptionId IN (
	 SELECT data FROM #filterIds WHERE SpecificationAttributeId = @filterId)
	 )
	
	DELETE FROM #filterIds WHERE SpecificationAttributeId = @filterId
	
END
 
 IF(@minPrice IS NOT NULL AND @maxPrice IS NOT NULL)
 BEGIN
   DELETE FROM #products WHERE #products.Price<@minPrice OR #products.Price>@maxPrice
 END
 --TODO get all subcategories for the given slug and include products also

DECLARE @XmlResult xml

--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult = (select 'SearchTemplate.ProductsInGridOrLines' as 'TemplateViewPath',@catIds1 'CategoryId',(select count(*) FROM  #products  PC 
inner join Category on PC.CategoryId = Category.Id
where Category.Id = @categoryId
and ParentCategoryId=0) 'Totalcount',
(select * FROM  #products PC 
inner join Category on PC.CategoryId = Category.Id
where Category.Id = @categoryId
and ParentCategoryId=0
FOR XML PATH('Product'), ROOT('Products') , type)
FOR XML PATH('Search') )

SELECT @XmlResult as XmlResult
   end
END



GO
PRINT 'Created the procedure usp_Search'
GO  
