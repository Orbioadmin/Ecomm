IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetProductsDetailBySlug]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetProductsDetailBySlug]
	PRINT 'Dropped [dbo].[usp_Catalog_GetProductsDetailBySlug]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetProductsDetailBySlug]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetProductsDetailBySlug
# File Path:
# CreatedDate: 06-Feb-2015
# Author: Sankar T.S
# Description: This stored procedure gets all the product details for the slug
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

CREATE PROCEDURE [dbo].[usp_Catalog_GetProductsDetailBySlug] (@slug nvarchar(400),  
@entityName nvarchar(400))   
AS    
BEGIN    

 DECLARE @productid INT , @categoryId INT, @parentCategoryIds NVARCHAR(1000)
 
 SELECT @productid = EntityId FROM UrlRecord where Slug = @slug and EntityName = @entityName
and IsActive = 1 and LanguageId = 0

SELECT @categoryId= CategoryId FROM Product_Category_Mapping where ProductId = @productId


 
 SELECT @parentCategoryIds = dbo.ufn_GetAllParentCateoryIds(@categoryId,null)
 SET @parentCategoryIds = @parentCategoryIds + CAST(@categoryId as Nvarchar(100))
 
 SELECT * , IDENTITY(int, 1,1) AS OrderBy
INTO #temp  FROM  [dbo].[nop_splitstring_to_table](@parentCategoryIds, ',')

DECLARE @currencyCode nvarchar(5) 
 
 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
 
 DECLARE @XmlResult xml;

	

--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult = (SELECT  (SELECT Name, Slug AS SeName from Category INNER JOIN #temp ON Category.Id = #temp.data
 LEFT JOIN UrlRecord UR ON Category.Id = UR.EntityId AND UR.IsActive=1
 AND UR.LanguageId = 0 AND EntityName = 'Category' ORDER BY #temp.OrderBy
FOR XML PATH('Category'), ROOT('BreadCrumbs'), TYPE), product.Id Id,product.Name Name,product.ShortDescription ShortDescription,product.FullDescription 'FullDescription',product.Price Price,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
INNER JOIN [dbo].[Picture] Pic ON Pic.Id = PPM.PictureId WHERE P.Id = product.Id
ORDER BY PPM.DisplayOrder FOR XML PATH ('ProductPicture'),ROOT('ProductPictures'), Type),
dbo.ufn_GetAllspecificationattributes(@productid), dbo.ufn_GetAllproductattributes(@productid),dbo.ufn_GetAllproductattributevarientvalue(@productid), Delivery_date.Name as DeliveredIn, pt.ViewPath, @currencyCode as CurrencyCode,ManageInventoryMethodId, DisplayStockAvailability,DisplayStockQuantity,IsShipEnabled,IsFreeShipping, StockQuantity
from [dbo].[Product] product 
INNER JOIN ProductTemplate PT ON product.ProductTemplateId = PT.Id
 Left join [dbo].[DeliveryDate] Delivery_date on product.DeliveryDateId= Delivery_date.Id  
where product.Id = @productid and product.Deleted <> 1

FOR XML PATH('ProductDetail')
)
 SELECT @XmlResult as XmlResult
   
END

GO
PRINT 'Created the procedure usp_Catalog_GetProductsDetailBySlug'
GO  