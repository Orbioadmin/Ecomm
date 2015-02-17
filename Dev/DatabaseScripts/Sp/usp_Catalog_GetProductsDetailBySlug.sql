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

 DECLARE @productid INT 
 
 SELECT @productid = EntityId FROM UrlRecord where Slug = @slug and EntityName = @entityName
and IsActive = 1 and LanguageId = 0

 DECLARE @XmlResult xml;

	

--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult = (select product.Id Id,product.Name Name,product.ShortDescription ShortDescription,product.FullDescription 'FullDescription',product.Price Price,dbo.ufn_GetAllPictureurls(@productid),
dbo.ufn_GetAllspecificationattributes(@productid), dbo.ufn_GetAllproductattributes(@productid),dbo.ufn_GetAllproductattributevarientvalue(@productid)
from [dbo].[Product] product inner join [dbo].[DeliveryDate] Delivery_date on Delivery_date.Id = product.DeliveryDateId
where product.Id = @productid and product.Deleted <> 1

FOR XML PATH('ProductDetail')
)
 SELECT @XmlResult as XmlResult
   
END

GO
PRINT 'Created the procedure usp_Catalog_GetProductsDetailBySlug'
GO  