IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetProductsBySlug]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetProductsBySlug]
	PRINT 'Dropped [dbo].[usp_Catalog_GetProductsBySlug]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetProductsBySlug]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetProductsBySlug
# File Path:
# CreatedDate: 15-dec-2014
# Author: Madhu MB
# Description: This stored procedure gets all the products for the slug
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

CREATE PROCEDURE [dbo].[usp_Catalog_GetProductsBySlug] (@slug nvarchar(400),  
@entityName nvarchar(400), @specificationFilterIds nvarchar(500)=null, @minPrice decimal(18,4)=null,
@maxPrice decimal(18,4)=null,@keyWord varchar(max),@pageNumber int, @pageSize int)  
   
AS    
BEGIN    
 --DECLARE @currencyCode nvarchar(5)  
 DECLARE @parentCategoryIds varchar(500)
 DECLARE @categoryId INT 
 
  IF(LTRIM(RTRIM(@slug)) = 'search' AND  ISNULL(@keyWord,'')<>'' )
 BEGIN
	SET @categoryId = 0	 
END
ELSE
BEGIN
	SET @keyWord = ''
	SELECT @categoryId = EntityId FROM UrlRecord where Slug = @slug and EntityName = @entityName
	and IsActive = 1 and LanguageId = 0
END
 
 SELECT @parentCategoryIds = dbo.ufn_GetAllParentCateoryIds(@categoryId,null)
 
 SELECT * , IDENTITY(int, 1,1) AS OrderBy
INTO #temp  FROM  [dbo].[nop_splitstring_to_table](@parentCategoryIds, ',')

  SELECT FILTERS.* , IDENTITY(int, 1,1) AS OrderBy
INTO #filterIds FROM  [dbo].[nop_splitstring_to_table](@specificationFilterIds, ',') FILTERS



SELECT   1 AS Ranking, * INTO #products FROM  ufn_GetProductsBySearch(@categoryId,@keyWord)

IF EXISTS(SELECT 1 FROM #filterIds)
BEGIN
	DELETE FROM #products WHERE #products.Id NOT IN (
 SELECT #products.Id FROM #products INNER JOIN
	  Product_SpecificationAttribute_Mapping PSM 
	 ON 
	 #products.Id = PSM.ProductId AND psm.AllowFiltering=1
      INNER JOIN #filterIds on PSM.SpecificationAttributeOptionId = #filterIds.data
     )
END
--DECLARE @filterId NVARCHAR(100)
--while EXISTS(SELECT 1 FROM #filterIds)
--BEGIN
--	SELECT TOP 1 @filterId = SAO.SpecificationAttributeId from #filterIds INNER JOIN
--	SpecificationAttributeOption SAO ON #filterIds.data = SAO.Id 
	
	
--	  DELETE FROM #products WHERE #products.Id NOT IN (
--	 select ProductId from #products INNER JOIN
--	  Product_SpecificationAttribute_Mapping PSM 
--	 ON 
--	 #products.Id = PSM.ProductId and psm.AllowFiltering=1
--	 WHERE PSM.SpecificationAttributeOptionId IN (
--	 SELECT data FROM #filterIds WHERE SpecificationAttributeId = @filterId)
--	 )
	
--	DELETE FROM #filterIds WHERE SpecificationAttributeId = @filterId
	
--END
 
 IF(@minPrice IS NOT NULL AND @maxPrice IS NOT NULL)
 BEGIN
   DELETE FROM #products WHERE #products.Price<@minPrice OR #products.Price>@maxPrice
 END
 --TODO get all subcategories for the given slug and include products also
 
  IF(@categoryId=0)
  BEGIN
   
	DELETE FROM #products  WHERE dbo.ufn_GetAllChildCategoryIds(Id,CategoryId,'')<>''
	
	UPDATE P  SET P.Ranking = P.ReRanking 
	FROM ( SELECT Ranking, RANK() over (partition by Id order by categoryid) AS ReRanking
	FROM #products ) AS P

   DELETE FROM #products WHERE Ranking>1
	INSERT INTO #temp SELECT DISTINCT CategoryId FROM #products
	
  END

SELECT *,ROW_NUMBER() OVER(ORDER BY Id) as RowNumber INTO #temptableproducts FROM #products  
DECLARE @XmlResult xml


    IF(@categoryId>0)
    BEGIN
		SELECT @XmlResult = ( select Category.Id as 'CategoryId', Category.Name  as 'Name',  MetaKeywords as 'MetaKeywords',
		MetaTitle as 'MetaTitle', MetaDescription as 'MetaDescription', @slug as 'SeName', 
		CASE WHEN CT.ViewPath IS NULL THEN 'CategoryTemplate.ProductsInGridOrLines' ELSE CT.ViewPath END
		AS TemplateViewPath, Category.PageSize,
		(SELECT Category.Id, Name, Slug AS SeName from Category INNER JOIN #temp ON Category.Id = #temp.data
		 LEFT JOIN UrlRecord UR ON Category.Id = UR.EntityId AND UR.IsActive=1
		 AND UR.LanguageId = 0 AND EntityName = @entityName ORDER BY #temp.OrderBy
		FOR XML PATH('Category'), ROOT('BreadCrumbs'),type)
		,
		 
		(select   *,ROW_NUMBER() OVER(ORDER BY PC.CategoryId),[dbo].[ufn_GetProductPriceDetails](PC.Id),
		(select Weight from [dbo].[ufn_GetProductPriceDetail](PC.Id)) as 'GoldWeight', 
		(select ProductUnit as 'ProductUnit' from [dbo].[ufn_GetProductPriceDetail](PC.Id))  as 'ProductUnit',
		(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
		(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,
		dbo.ufn_GetProductDiscounts(PC.Id)
		FROM  #temptableproducts PC  WHERE PC.CategoryId = Category.Id and PC.RowNumber BETWEEN ((@pageNumber - 1) * @pageSize + 1) AND (@pageNumber * @pageSize)
		FOR XML PATH('Product'), ROOT('Products') , type)  from Category 
		LEFT JOIN CategoryTemplate CT ON Category.CategoryTemplateId = CT.Id  
		where ( @categoryId>0 AND Category.Id  = @categoryId ) OR (@categoryId=0 AND 
		Category.Id IN (SELECT data from #temp))
		FOR XML PATH('CategoryProduct') )
	END
	ELSE
	BEGIN
		SELECT 0 as Id into #cat
		
		SELECT @XmlResult = ( select #cat.Id as 'CategoryId', (SELECT COUNT(1) FROM #temptableproducts) AS TotalProductCount, 
		(SELECT Category.Id, Name, Slug AS SeName from Category INNER JOIN #temp ON Category.Id = #temp.data
		 LEFT JOIN UrlRecord UR ON Category.Id = UR.EntityId AND UR.IsActive=1
		 AND UR.LanguageId = 0 AND EntityName = @entityName ORDER BY #temp.OrderBy
		FOR XML PATH('Category'), ROOT('BreadCrumbs'),type)
		,
		 
		(select   *,ROW_NUMBER() OVER(ORDER BY PC.CategoryId),[dbo].[ufn_GetProductPriceDetails](PC.Id),
		(select Weight from [dbo].[ufn_GetProductPriceDetail](PC.Id)) as 'GoldWeight', 
		(select ProductUnit as 'ProductUnit' from [dbo].[ufn_GetProductPriceDetail](PC.Id))  as 'ProductUnit',
		(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
		(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,
		dbo.ufn_GetProductDiscounts(PC.Id)
		FROM  #temptableproducts PC  WHERE  
		PC.RowNumber BETWEEN ((@pageNumber - 1) * @pageSize + 1) AND (@pageNumber * @pageSize)
		FOR XML PATH('Product'), ROOT('Products') , type)  from #cat 
		 
		FOR XML PATH('CategoryProduct') )
	END
	SELECT @XmlResult as XmlResult
   --END

--ELSE
--	BEGIN
--	--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
--	SELECT @XmlResult = ( select Category.Id as 'CategoryId', Category.Name  as 'Name',  MetaKeywords as 'MetaKeywords',
--	MetaTitle as 'MetaTitle', MetaDescription as 'MetaDescription', @slug as 'SeName',
--	CASE WHEN CT.ViewPath IS NULL THEN 'CategoryTemplate.ProductsInGridOrLines' ELSE CT.ViewPath END
--	AS TemplateViewPath, Category.PageSize,
--	(SELECT Name, Slug AS SeName from Category INNER JOIN #temp ON Category.Id = #temp.data
--	 LEFT JOIN UrlRecord UR ON Category.Id = UR.EntityId AND UR.IsActive=1
--	 AND UR.LanguageId = 0 AND EntityName = @entityName ORDER BY #temp.OrderBy
--	FOR XML PATH('Category'), ROOT('BreadCrumbs'),type)
--	,
	 
--	(select   *,ROW_NUMBER() OVER(ORDER BY PC.CategoryId),[dbo].[ufn_GetProductPriceDetails](PC.Id),
--	(select Weight from [dbo].[ufn_GetProductPriceDetail](PC.Id)) as 'GoldWeight', 
--	(select ProductUnit as 'ProductUnit' from [dbo].[ufn_GetProductPriceDetail](PC.Id))  as 'ProductUnit',
--	(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
--	(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice  
--	FROM  #temptableproducts  PC   WHERE PC.CategoryId = Category.Id and PC.RowNumber BETWEEN ((@pageNumber - 1) * @pageSize + 1) AND (@pageNumber * @pageSize)
--	FOR XML PATH('Product'), ROOT('Products') , type)  from Category 
--	LEFT JOIN CategoryTemplate CT ON Category.CategoryTemplateId = CT.Id  
--	where Category.Id = @categoryId
--	FOR XML PATH('CategoryProduct') )

--	SELECT @XmlResult as XmlResult
--	END
END    

GO
PRINT 'Created the procedure usp_Catalog_GetProductsBySlug'
GO  


