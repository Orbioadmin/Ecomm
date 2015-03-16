 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catlog_RealatedProducts]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetFiltersByCategory]
	PRINT 'Dropped [dbo].[usp_Catlog_RealatedProducts]'
END	
GO

PRINT 'Creating [dbo].[usp_Catlog_RealatedProducts]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catlog_RealatedProducts
# File Path:
# CreatedDate: 13-march-2015
# Author: Sankar T S
# Description: This stored procedure gets all the related products
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

CREATE PROCEDURE [dbo].[usp_Catlog_RealatedProducts]
@productid int
AS
BEGIN
select [ProductId2] into #temtable from [dbo].[RelatedProduct] where [ProductId1] = @productid

declare @productIds VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.ProductId2)
         FROM #temtable p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
        ,1,1,'') Color
FROM #temtable p)

DECLARE @currencyCode nvarchar(5) 
 
 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  

DECLARE @XmlResult1 xml

SELECT @XmlResult1 = (select(select product.Id Id,product.Name Name,ur.Slug 'SeName',@currencyCode 'CurrencyCode', product.Price Price,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
INNER JOIN [dbo].[Picture] Pic ON Pic.Id = PPM.PictureId WHERE P.Id = product.Id

ORDER BY PPM.DisplayOrder FOR XML PATH ('ProductPicture'),ROOT('ProductPictures'), Type)
from [dbo].[Product] product
Left  join UrlRecord ur on product.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1
where ','+@productIds+',' LIKE '%,'+CAST(product.Id AS varchar)+',%' and product.Deleted <> 1
FOR XML PATH ('ProductDetail'),ROOT('Productdetails'), Type) 
FOR XML PATH('RelatedProduct'))

SELECT @XmlResult1 as XmlResult

END


--exec usp_Catlog_RealatedProducts 55
GO
PRINT 'Created the procedure usp_Catlog_RealatedProducts'
GO  
