 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Shoppingcart_Items]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Shoppingcart_Items]
	PRINT 'Dropped [dbo].[usp_Shoppingcart_Items]'
END	
GO

PRINT 'Creating [dbo].[usp_Shoppingcart_Items]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Shoppingcart_Items
# File Path:
# CreatedDate: 09-march-2015
# Author: Sankar T S
# Description: This procedure to use add and select shopping cart items.
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

CREATE PROCEDURE [dbo].[usp_Shoppingcart_Items]
	@action varchar(30),
	@id int,
	@shoppingCartTypeId int,
	@curCustomerId int,
	@customerId int,
	@productId int,
	@attributexml varchar(max),
	@quantity int
AS
BEGIN

if(@action = 'add')
begin
	
	if exists(select Id from [dbo].[ShoppingCartItem] where ShoppingCartTypeId=@shoppingCartTypeId and CustomerId = @customerId and
				ProductId = @productId and AttributesXml =@attributexml)
		begin
			update [dbo].[ShoppingCartItem] set [AttributesXml] =@attributexml ,[Quantity] = (@quantity+Quantity),UpdatedOnUtc = CONVERT(VARCHAR(30),GETDATE(),121) where  
			ShoppingCartTypeId=@shoppingCartTypeId and CustomerId = @customerId and ProductId = @productId and AttributesXml =@attributexml
		end

	else
		begin
			insert into [dbo].[ShoppingCartItem](StoreId,ShoppingCartTypeId,CustomerId,ProductId,CustomerEnteredPrice,AttributesXml,Quantity,CreatedOnUtc,UpdatedOnUtc)
			values(0,@shoppingCartTypeId,@customerId,@productId,0.00,@attributexml,@quantity,CONVERT(VARCHAR(30),GETDATE(),121),CONVERT(VARCHAR(30),GETDATE(),121))
		end

end

if(@action = 'addWishList')
begin
	
	if exists(select Id from [dbo].[ShoppingCartItem] where ShoppingCartTypeId=@shoppingCartTypeId and CustomerId = @customerId and
				ProductId = @productId and AttributesXml =@attributexml)
		begin
			update [dbo].[ShoppingCartItem] set [AttributesXml] =@attributexml ,[Quantity] = (@quantity+Quantity),UpdatedOnUtc = CONVERT(VARCHAR(30),GETDATE(),121) where  
			ShoppingCartTypeId=@shoppingCartTypeId and CustomerId = @customerId and ProductId = @productId and AttributesXml =@attributexml
			select 'updated'
		end

	else
		begin
			insert into [dbo].[ShoppingCartItem](StoreId,ShoppingCartTypeId,CustomerId,ProductId,CustomerEnteredPrice,AttributesXml,Quantity,CreatedOnUtc,UpdatedOnUtc)
			values(0,@shoppingCartTypeId,@customerId,@productId,0.00,@attributexml,@quantity,CONVERT(VARCHAR(30),GETDATE(),121),CONVERT(VARCHAR(30),GETDATE(),121))
			select 'inserted'
		end

end

if(@action = 'update')
begin
	
	if exists(select Id from [dbo].[ShoppingCartItem] where CustomerId = @customerId)
		begin
			select Id,ProductId,ShoppingCartTypeId,AttributesXml into #tempcart from ShoppingCartItem where CustomerId = @customerId 
			select Id,ProductId,ShoppingCartTypeId,AttributesXml into #tempcart1 from ShoppingCartItem where CustomerId = @curCustomerId
			SELECT Id,ProductId
			into #tempcart2
			FROM #tempcart b
			WHERE NOT EXISTS (
								SELECT *
								FROM #tempcart1 a
								WHERE a.ProductId = b.ProductId and a.AttributesXml = b.AttributesXml and a.ShoppingCartTypeId=b.ShoppingCartTypeId)

			declare @cartIds VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.Id)
			FROM #tempcart2 p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
			,1,1,'') Id
			FROM #tempcart2 p)

			update [dbo].[ShoppingCartItem] set CustomerId =@curCustomerId ,UpdatedOnUtc = CONVERT(VARCHAR(30),GETDATE(),121) where ','+@cartIds+',' LIKE '%,'+CAST(Id AS varchar)+',%'
		
		end
end

if(@action='delete')
begin
if exists(select Id from [dbo].[ShoppingCartItem] where Id = @id)
delete from [dbo].[ShoppingCartItem] where Id = @id
select 'deleted'
end

if(@action='addtocart')
begin
if not exists(select Id from [dbo].[ShoppingCartItem] where ShoppingCartTypeId=@shoppingCartTypeId and CustomerId = (select CustomerId from [dbo].[ShoppingCartItem] where Id = @id ) and
				ProductId = (select ProductId from [dbo].[ShoppingCartItem] where Id = @id ) and AttributesXml =(select AttributesXml from [dbo].[ShoppingCartItem] where Id = @id ))
			begin
				
				if exists(select Id from [dbo].[ShoppingCartItem] where Id = @id and AttributesXml = '')
					begin
						
						declare @productVarient int = (select dbo.ufn_GetProductAttributeVarientValueById((select ProductId from [dbo].[ShoppingCartItem] where Id = @id )))
						if (@productVarient IS NOT NULL)
								begin
									select slug from UrlRecord where EntityId = (select ProductId from [dbo].[ShoppingCartItem] where Id = @id ) and EntityName='Product' and IsActive=1 
								end

						else
							begin
								
								insert into [dbo].[ShoppingCartItem](StoreId,ShoppingCartTypeId,CustomerId,ProductId,CustomerEnteredPrice,AttributesXml,Quantity,CreatedOnUtc,UpdatedOnUtc)
								values(0,@shoppingCartTypeId, (select CustomerId from [dbo].[ShoppingCartItem] where Id = @id ),(select ProductId from [dbo].[ShoppingCartItem] where Id = @id ),0.00,(select AttributesXml from [dbo].[ShoppingCartItem] where Id = @id ),(select Quantity from [dbo].[ShoppingCartItem] where Id = @id ),(select CreatedOnUtc from [dbo].[ShoppingCartItem] where Id = @id ),(select UpdatedOnUtc from [dbo].[ShoppingCartItem] where Id = @id ))
								select 'ShoppingCart'

							end

					end

				else
					begin

						insert into [dbo].[ShoppingCartItem](StoreId,ShoppingCartTypeId,CustomerId,ProductId,CustomerEnteredPrice,AttributesXml,Quantity,CreatedOnUtc,UpdatedOnUtc)
						values(0,@shoppingCartTypeId, (select CustomerId from [dbo].[ShoppingCartItem] where Id = @id ),(select ProductId from [dbo].[ShoppingCartItem] where Id = @id ),0.00,(select AttributesXml from [dbo].[ShoppingCartItem] where Id = @id ),(select Quantity from [dbo].[ShoppingCartItem] where Id = @id ),(select CreatedOnUtc from [dbo].[ShoppingCartItem] where Id = @id ),(select UpdatedOnUtc from [dbo].[ShoppingCartItem] where Id = @id ))
						select 'ShoppingCart'

					end

			end
	else
		begin
			
			select 'ShoppingCart'

		end
end


if(@action = 'select')
begin

select ProductId into #temp from ShoppingCartItem where shoppingCartTypeId=@shoppingCartTypeId and CustomerId = @customerId 

DECLARE @currencyCode nvarchar(5) 
 
 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
 
DECLARE @XmlResult xml;

	

--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult = (Select(SELECT (select count(#temp.ProductId) from #temp) as 'ItemCount',product.Id Id,product.Name Name,ur.Slug as SeName,product.Price Price,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
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
   (Select TextPrompt,
   (Select Name from ufn_GetCartProductAttribute(sc.AttributesXml,product.Id)  FOR XML PATH('ProductVariantAttributeValue'), ROOT('ProductVariantAttributeValues'), type)
   from ufn_GetCartProductAttribute(sc.AttributesXml,product.Id)
   FOR XML PATH('ProductAttributeVariant'), ROOT('ProductAttributeVariants'),type),
   (product.Price*sc.Quantity) as 'TotalPrice'
from [dbo].[Product] product 
inner join ShoppingCartItem sc on sc.ProductId = product.Id 
Left join [dbo].[DeliveryDate] Delivery_date on product.DeliveryDateId= Delivery_date.Id
Left  join UrlRecord ur on product.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1    
where sc.CustomerId=@customerId and sc.ShoppingCartTypeId=@shoppingCartTypeId and product.Deleted <> 1 order by product.Id 
FOR XML PATH('ShoppingCartItem'),Root('ShoppingCartProductItems'), type )For XML PATH('ShoppingCartItems'),type)
SELECT @XmlResult as XmlResult
end

END



GO
PRINT 'Created the procedure usp_Shoppingcart_Items'
GO  
