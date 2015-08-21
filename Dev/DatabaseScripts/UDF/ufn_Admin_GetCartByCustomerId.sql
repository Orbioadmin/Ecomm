/*
 ===================================================================================================================================================
 Author: Sankar T S
 Create date: 19 aug 2015
 Description: This function will return the car details
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_Admin_GetCartByCustomerId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_Admin_GetCartByCustomerId]
	PRINT 'Dropped UDF [dbo].[ufn_Admin_GetCartByCustomerId]'
GO
PRINT 'Creating UDF [dbo].[ufn_Admin_GetCartByCustomerId]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_Admin_GetCartByCustomerId](@customerId INT,@storeId Int,@shoppingCartTypeId int)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
DECLARE @currencyCode nvarchar(5) 
 
 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
 
DECLARE @XmlResult xml;

	

SELECT @XmlResult = (SELECT dbo.ufn_GetOrderDiscounts(@customerId, @storeId)
,  
(SELECT --(select count(#temp.ProductId) from #temp) as 'ItemCount',
product.Id Id,
product.Name Name,ur.Slug as SeName,product.Price Price,[dbo].[ufn_GetProductPriceDetails](product.Id),
product.[Weight] as 'GoldWeight',
product.ProductUnit as 'ProductUnit', product.TaxCategoryId,
(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
INNER JOIN [dbo].[Picture] Pic ON Pic.Id = PPM.PictureId WHERE P.Id = product.Id
ORDER BY PPM.DisplayOrder FOR XML PATH ('ProductPicture'),ROOT('ProductPictures'), Type),
  @currencyCode as CurrencyCode,
  DisplayStockAvailability,
  DisplayStockQuantity,
   StockQuantity,
	OrderMinimumQuantity,
   OrderMaximumQuantity,
   AllowedQuantities,
   sc.Id as 'CartId',
   sc.Quantity as 'Quantity',
   sc.ShoppingCartStatusId as 'ShoppingCartStatusId',
   sc.UpdatedOnUtc as 'UpdatedOnUtc',
   (Select TextPrompt,
   (Select Name,PriceAdjustment,WeightAdjustment,[dbo].[ufn_GetProductPriceDetailsByVarientValue](product.Id,VariantValueId) ,(select value from [dbo].[Setting] where Name = 'Product.PriceUnit') as PriceUnit,
(select value from [dbo].[Setting] where Name = 'Product.MarketUnitPrice') as MarketUnitPrice,(select Weight from [dbo].[ufn_GetProductPriceDetail](product.Id)) as 'GoldWeight', (select ProductUnit as 'ProductUnit' from [dbo].[ufn_GetProductPriceDetail](product.Id))  as 'ProductUnit' from ufn_GetCartProductAttribute(sc.AttributesXml,product.Id,TextPrompt)  FOR XML PATH('ProductVariantAttributeValue'), ROOT('ProductVariantAttributeValues'), type)
   from ufn_GetCartProductAttributes(sc.AttributesXml,product.Id) 
   FOR XML PATH('ProductAttributeVariant'), ROOT('ProductAttributeVariants'),type),AttributesXml , dbo.ufn_GetProductDiscounts(product.Id)
from [dbo].[Product] product 
inner join ShoppingCartItem sc on sc.ProductId = product.Id 
Left join [dbo].[DeliveryDate] Delivery_date on product.DeliveryDateId= Delivery_date.Id
Left  join UrlRecord ur on product.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1    
where sc.CustomerId=@customerId and sc.ShoppingCartTypeId=@shoppingCartTypeId and product.Deleted <> 1 order by product.Id 
FOR XML PATH('ShoppingCartItem'),Root('ShoppingCartItems'), type ) For XML PATH('Cart')) --,type)

	 Return @XmlResult

END

GO
PRINT 'Created UDF [dbo].[ufn_Admin_GetCartByCustomerId]`'
GO  