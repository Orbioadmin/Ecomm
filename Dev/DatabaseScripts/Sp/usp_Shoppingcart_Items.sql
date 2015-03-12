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

Create PROCEDURE [dbo].[usp_Shoppingcart_Items]
	@action varchar(30),
	@id int,
	@shoppingcarttypeid int,
	@customerid int,
	@productid int,
	@attributexml varchar(max),
	@quantity int
AS
BEGIN

if(@action = 'add')
begin
	
	if exists(select Id from [dbo].[ShoppingCartItem] where ShoppingCartTypeId=@shoppingcarttypeid and CustomerId = @customerid and
				ProductId = @productid and AttributesXml =@attributexml)
		begin
			update [dbo].[ShoppingCartItem] set [AttributesXml] =@attributexml ,[Quantity] = (@quantity+Quantity),UpdatedOnUtc = CONVERT(VARCHAR(30),GETDATE(),121) where  
			ShoppingCartTypeId=@shoppingcarttypeid and CustomerId = @customerid and ProductId = @productid
		end

	else
		begin
			insert into [dbo].[ShoppingCartItem](StoreId,ShoppingCartTypeId,CustomerId,ProductId,CustomerEnteredPrice,AttributesXml,Quantity,CreatedOnUtc,UpdatedOnUtc)
			values(0,@shoppingcarttypeid,@customerid,@productid,0.00,@attributexml,@quantity,CONVERT(VARCHAR(30),GETDATE(),121),CONVERT(VARCHAR(30),GETDATE(),121))
		end

end

if(@action = 'update')
begin
	
	if exists(select Id from [dbo].[ShoppingCartItem] where CustomerId = @customerid)
		begin
			update [dbo].[ShoppingCartItem] set CustomerId =@shoppingcarttypeid ,UpdatedOnUtc = CONVERT(VARCHAR(30),GETDATE(),121) where CustomerId = @customerid
		end
end

if(@action = 'select')
begin

select ProductId into #temp from ShoppingCartItem where shoppingCartTypeId=@shoppingcarttypeid and CustomerId = @customerid group by ProductId

DECLARE @currencyCode nvarchar(5) 
 
 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
 
 DECLARE @XmlResult xml;

	

--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
SELECT @XmlResult = (SELECT (select count(#temp.ProductId) from #temp) as 'Itemcount',( select product.Id Id,product.Name Name,product.Price Price,(SELECT Pic.Id PictureId, PPM.DisplayOrder, Pic.RelativeUrl,Pic.MimeType , Pic.SeoFilename, Pic.IsNew FROM [dbo].[Product]  P INNER JOIN [dbo].[Product_Picture_Mapping] PPM ON PPM.ProductId = P.Id
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
   sc.Quantity as 'CartQuantity',
   (product.Price*sc.Quantity) as 'Totalprice'
from [dbo].[Product] product 
inner join ShoppingCartItem sc on sc.ProductId = product.Id 
Left join [dbo].[DeliveryDate] Delivery_date on product.DeliveryDateId= Delivery_date.Id  
where sc.CustomerId=@customerid and sc.ShoppingCartTypeId=@shoppingcarttypeid and product.Deleted <> 1

FOR XML PATH('ShoppingCartProduct'),ROOT('ShoppingCartProducts'),type
)
FOR XML PATH('ShoppingCartItem'), type )
 SELECT @XmlResult as XmlResult

end

END


GO
PRINT 'Created the procedure usp_Shoppingcart_Items'
GO  
