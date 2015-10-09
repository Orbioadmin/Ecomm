IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_SimilarProducts]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_SimilarProducts]
	PRINT 'Dropped [dbo].[usp_Catalog_SimilarProducts]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_SimilarProducts]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_SimilarProducts
# File Path:
# CreatedDate: 24-OCT-2015
# Author: Sankar T.S
# Description: This stored procedure get all similar products by id
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
Create PROCEDURE [dbo].[usp_Catalog_SimilarProducts]
@productid int
AS
BEGIN

select [ProductId2] into #temtable from [dbo].[SimilarProduct] where [ProductId1] = @productid

declare @productIdcount int = (SELECT COUNT(*)FROM #temtable)
DECLARE @currencyCode nvarchar(5) 
		 
SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
		 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )

DECLARE @XmlResult1 xml
  
if(@productIdcount > 0)
	begin
	
	declare @productIds VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.ProductId2)
         FROM #temtable p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
        ,1,1,'') Color
		FROM #temtable p)
		
		SELECT @XmlResult1 = (select(select product.Id Id,product.Name Name,ur.Slug 'SeName',@currencyCode 'CurrencyCode', product.Price Price,[dbo].[ufn_GetProductPriceDetails](product.Id),
		product.[Weight] as 'GoldWeight',product.ProductUnit as 'ProductUnit',
		(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
		(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
		INNER JOIN [dbo].[Picture] Pic ON Pic.Id = PPM.PictureId WHERE P.Id = product.Id

		ORDER BY PPM.DisplayOrder FOR XML PATH ('ProductPicture'),ROOT('ProductPictures'), Type)
		from [dbo].[Product] product
		Left  join UrlRecord ur on product.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1
		where ','+@productIds+',' LIKE '%,'+CAST(product.Id AS varchar)+',%' and product.Deleted <> 1
		FOR XML PATH ('ProductDetail'),ROOT('ProductDetails'), Type) 
		FOR XML PATH('SimilarProduct'))
		
		SELECT @XmlResult1 as XmlResult
	end

else
	begin
		declare @productTagCount int = (SELECT COUNT(P.Id)FROM Product p inner join dbo.Product_ProductTag_Mapping p2 on p.Id = p2.Product_Id WHERE p.Deleted = 0
								AND p.Id = @productid)
		if(@productTagCount > 0)
			begin
				declare @productTags VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p2.ProductTag_Id)
													  FROM Product p1 inner join dbo.Product_ProductTag_Mapping p2 on p1.Id = p2.Product_Id where p2.Product_Id = @productid
													  FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'),1,1,'') ProductTagIds FROM Product p)
				
				
				SELECT @XmlResult1 = (select(select product.Id Id,product.Name Name,ur.Slug 'SeName',@currencyCode 'CurrencyCode', product.Price Price,[dbo].[ufn_GetProductPriceDetails](product.Id),
				product.[Weight] as 'GoldWeight',product.ProductUnit as 'ProductUnit',
				(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
				(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
				INNER JOIN [dbo].[Picture] Pic ON Pic.Id = PPM.PictureId WHERE P.Id = product.Id
				ORDER BY PPM.DisplayOrder FOR XML PATH ('ProductPicture'),ROOT('ProductPictures'), Type)
				from [dbo].[Product] product
				Left  join UrlRecord ur on product.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1
				inner join dbo.Product_ProductTag_Mapping p2 on product.Id = p2.Product_Id
				where ','+@productTags+',' LIKE '%,'+CAST(p2.ProductTag_Id AS varchar)+',%' and product.Deleted <> 1
				and Product.Id <> @productid
				FOR XML PATH ('ProductDetail'),ROOT('ProductDetails'), Type) 
				FOR XML PATH('SimilarProduct'))

				SELECT @XmlResult1 as XmlResult
			end
		else
			begin
				
				declare @categoryId VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p2.CategoryId)
													FROM Product p1 inner join dbo.Product_Category_Mapping p2 on p1.Id = p2.ProductId
													where p2.ProductId = @productid FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'),1,1,'') CategoryIds FROM Product p)
				
				
				SELECT @XmlResult1 = (select(select top 20 product.Id Id,product.Name Name,ur.Slug 'SeName',@currencyCode 'CurrencyCode', product.Price Price,[dbo].[ufn_GetProductPriceDetails](product.Id),
				product.[Weight] as 'GoldWeight',product.ProductUnit as 'ProductUnit',
				(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
				(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
				INNER JOIN [dbo].[Picture] Pic ON Pic.Id = PPM.PictureId WHERE P.Id = product.Id
				ORDER BY PPM.DisplayOrder FOR XML PATH ('ProductPicture'),ROOT('ProductPictures'), Type)
				from [dbo].[Product] product
				Left  join UrlRecord ur on product.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1
				inner join dbo.Product_Category_Mapping p2 on product.Id = p2.ProductId
				where ','+@categoryId+',' LIKE '%,'+CAST(p2.CategoryId AS varchar)+',%' and product.Deleted <> 1
				and Product.Id <> @productid 
				FOR XML PATH ('ProductDetail'),ROOT('ProductDetails'), Type) 
				FOR XML PATH('SimilarProduct'))

				SELECT @XmlResult1 as XmlResult
				
			end
		
	end

END




GO
PRINT 'Created the procedure usp_Catalog_SimilarProducts'
GO  


