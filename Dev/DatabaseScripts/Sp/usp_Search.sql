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

CREATE PROCEDURE [dbo].[usp_Search] (@slug nvarchar(400),  
@entityName nvarchar(400), @specificationFilterIds nvarchar(500)=null, @minPrice decimal(18,4)=null,
@maxPrice decimal(18,4)=null,@keyWord varchar(max),@pageNumber int, @pageSize int)  
   
AS    
BEGIN    

SELECT SAO.SpecificationAttributeId,FILTERS.* , IDENTITY(int, 1,1) AS OrderBy
INTO #temptablefilterIds FROM  [dbo].[nop_splitstring_to_table](@specificationFilterIds, ',') FILTERS
INNER JOIN SpecificationAttributeOption SAO ON SAO.Id = FILTERS.data

SELECT  * INTO #temptableproduct FROM  ufn_GetProductsBySearch(0,@keyWord)


declare @catIds VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.CategoryId)
         FROM #temptableproduct p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
        ,1,1,'') Color
FROM #temptableproduct p)


DECLARE @filterId1 NVARCHAR(100)
while EXISTS(SELECT 1 FROM #temptablefilterIds)
BEGIN
	SELECT TOP 1 @filterId1 = SAO.SpecificationAttributeId from #temptablefilterIds INNER JOIN
	SpecificationAttributeOption SAO ON #temptablefilterIds.data = SAO.Id 
	
	  DELETE FROM #temptableproduct WHERE #temptableproduct.Id NOT IN (
	 select ProductId from #temptableproduct INNER JOIN
	  Product_SpecificationAttribute_Mapping PSM 
	 ON 
	 #temptableproduct.Id = PSM.ProductId --and psm.AllowFiltering=1
	 WHERE PSM.SpecificationAttributeOptionId IN (
	 SELECT data FROM #temptablefilterIds WHERE SpecificationAttributeId = @filterId1)
	 )
	
	DELETE FROM #temptablefilterIds WHERE SpecificationAttributeId = @filterId1
	
END
 
 IF(@minPrice IS NOT NULL AND @maxPrice IS NOT NULL)
 BEGIN
   DELETE FROM #temptableproduct WHERE #temptableproduct.Price<@minPrice OR #temptableproduct.Price>@maxPrice
 END

 select *,ROW_NUMBER() OVER(ORDER BY CategoryId) as RowNumber into #temptableproducts from #temptableproduct

DECLARE @XmlResult1 xml


--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult1 = (select @catIds 'CategoryId',(select count(*) FROM  #temptableproduct  PC 
inner join Category on PC.CategoryId = Category.Id
where ','+@catIds+',' LIKE '%,'+CAST(Category.Id AS varchar)+',%'
and ParentCategoryId=0) 'Totalcount',
(select PC.CategoryId,PC.Id,PC.Name,PC.ShortDescription,PC.Price,[dbo].[ufn_GetProductPriceDetails](PC.Id),
(select Weight from [dbo].[ufn_GetProductPriceDetail](PC.Id)) as 'GoldWeight', 
(select ProductUnit as 'ProductUnit' from [dbo].[ufn_GetProductPriceDetail](PC.Id))  as 'ProductUnit',
(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,PC.ViewPath,
PC.CurrencyCode,PC.ImageRelativeUrl,PC.SeName,Pc.RowNumber  FROM  #temptableproducts  PC 
inner join Category on PC.CategoryId = Category.Id and PC.RowNumber BETWEEN ((@pageNumber - 1) * @pageSize + 1) AND (@pageNumber * @pageSize)
where ','+@catIds+',' LIKE '%,'+CAST(Category.Id AS varchar)+',%'
and ParentCategoryId=0
FOR XML PATH('Product'), ROOT('Products') , type)
FOR XML PATH('Search') )

SELECT @XmlResult1 as XmlResult

END




GO
PRINT 'Created the procedure usp_Search'
GO  
