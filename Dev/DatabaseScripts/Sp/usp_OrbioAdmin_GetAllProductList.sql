IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_GetAllProductList]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrbioAdmin_GetAllProductList]
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_GetAllProductList]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_GetAllProductList]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_GetAllProductList
# File Path:
# CreatedDate: 14-SEP-2015
# Author: Sankar
# Description: This stored procedure get all Products details
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
CREATE PROCEDURE [dbo].[usp_OrbioAdmin_GetAllProductList]
@nameOrSku varchar(max) = null,
@categoryId int = null,
@manufatureId int = null
AS
BEGIN

	declare @query varchar(300) = '';
	declare @joinQuery varchar(300) = '';
		
	if(@categoryId is not null and @categoryId > 0)
		begin
			set @joinQuery += '	left join dbo.Product_Category_Mapping pcm on pcm.ProductId = p.Id  and p.Deleted=0 and p.Published = 1  '
			Set @query += ' and pcm.CategoryId = '+convert(varchar,@categoryId)+''
		end
		
	if(@manufatureId is not null and @manufatureId > 0)
		begin
			set @joinQuery += ' left join dbo.Product_Manufacturer_Mapping pmm on pmm.ProductId = p.Id '
			Set @query += ' and pmm.ManufacturerId = '+convert(varchar,@manufatureId)+''
		end
		
	if(@nameOrSku is not null and @nameOrSku <> '')
		begin
			SET @nameOrSku = '%' + @nameOrSku + '%'
			Set @query += ' and p.[Name] like '''+@nameOrSku+''' OR p.Sku like '''+@nameOrSku+''''
		end
		
		DECLARE @currencyCode nvarchar(5) 
		SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
		CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
	
	exec('DECLARE @XmlResult xml
	SELECT @XmlResult =	(select ROW_NUMBER() over (Order by p.Id desc) as ''RowNum'',p.*,[dbo].[ufn_GetProductPriceDetails](p.Id),
	(select Weight from [dbo].[ufn_GetProductPriceDetail](p.Id)) as ''GoldWeight'', 
	(select ProductUnit as ''ProductUnit'' from [dbo].[ufn_GetProductPriceDetail](p.Id))  as ''ProductUnit'',
	(select value from [dbo].[Setting] where Name = ''Product.PriceUnit'') as PriceUnit,
	(select value from [dbo].[Setting] where Name = ''Product.MarketUnitPrice'') as MarketUnitPrice,
	dbo.ufn_GetProductDiscounts(p.Id),
	 '''+@currencyCode+''' as  
	''CurrencyCode'', pic.RelativeUrl  as ''ImageRelativeUrl''
	from  Product p  -- 
	'+@joinQuery+'
	left join Product_Picture_Mapping ppm on p.Id = ppm.ProductId  and ppm.DisplayOrder=1 --get only 1 pic rec
	left join Picture pic on pic.Id = ppm.PictureId and ppm.DisplayOrder=1 --get only 1 pic rec
	where p.Deleted <> 1 and p.StockQuantity > 0 '+@query+' order by p.Id desc 
	FOR XML PATH(''Product''),Root(''ArrayOfProduct'')) SELECT @XmlResult as XmlResult')
		
		
END

GO
PRINT 'Created the procedure usp_OrbioAdmin_GetAllProductList'
GO  


