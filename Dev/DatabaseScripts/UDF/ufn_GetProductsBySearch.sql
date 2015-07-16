/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 02 march 2015
 Description: This function will return the products for given search keyword
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetProductsBySearch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetProductsBySearch]
	PRINT 'Dropped UDF [dbo].[ufn_GetProductsBySearch]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetProductsBySearch]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetProductsBySearch] (@category int,@keword varchar(max))  
RETURNS @TABLE TABLE (RowNum INT, CategoryId INT,
   Id INT,
   Name nvarchar(400),
   ShortDescription nvarchar(max),
   Price decimal(18,4),
   ViewPath nvarchar(400),
   CurrencyCode nvarchar(5),
   ImageRelativeUrl nvarchar(max),
   SeName nvarchar(400))  
AS  
BEGIN  
  
 DECLARE @currencyCode nvarchar(5) 
 
 if(@category = 0)
 begin
 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
    INSERT @TABLE
	select ROW_NUMBER() over (Order by pcm.displayorder, p.Name) as 'RowNum',
	 pcm.CategoryId,  p.Id AS 'Id',  p.Name as 'Name', p.ShortDescription as 'ShortDescription', 
	--p.FullDescription as 'ns:FullDescription',
	p.Price as 'Price', pt.ViewPath as 'ViewPath',
	 @currencyCode as  
	'CurrencyCode', pic.RelativeUrl  as 'ImageRelativeUrl', ur.Slug as SeName
	from  Product p  -- 
	 
	inner join dbo.Product_Category_Mapping pcm 
	 on pcm.ProductId = p.Id  and p.Deleted=0 and p.Published = 1  
	left join Product_Picture_Mapping ppm on p.Id = ppm.ProductId  and ppm.DisplayOrder=1 --get only 1 pic rec
	left join Picture pic on pic.Id = ppm.PictureId and ppm.DisplayOrder=1 --get only 1 pic rec
	inner join ProductTemplate pt on p.ProductTemplateId = pt.Id 
	Left  join UrlRecord ur on p.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1
	AND ur.LanguageId=0
	where p.Name like '%'+@keword+'%'
	ORDER BY pcm.DisplayOrder, p.Name
	END
	ELSE
	BEGIN
	 SELECT @currencyCode = CurrencyCode FROM Currency WHERE Id = (SELECT   
 CAST(value as INT) FROM Setting  WHERE Name = 'currencysettings.primarystorecurrencyid' )  
    INSERT @TABLE
	select ROW_NUMBER() over (Order by pcm.displayorder, p.Name) as 'RowNum',
	 pcm.CategoryId,  p.Id AS 'Id',  p.Name as 'Name', p.ShortDescription as 'ShortDescription', 
	--p.FullDescription as 'ns:FullDescription',
	p.Price as 'Price', pt.ViewPath as 'ViewPath',
	 @currencyCode as  
	'CurrencyCode', pic.RelativeUrl  as 'ImageRelativeUrl', ur.Slug as SeName
	from  dbo.Product_Category_Mapping pcm  
	 
	INNER JOIN Product p on pcm.ProductId = p.Id  and p.Deleted=0 and p.Published = 1  
	LEFT JOIN Product_Picture_Mapping ppm on p.Id = ppm.ProductId  AND ppm.DisplayOrder=1
	LEFT JOIN Picture pic on pic.Id = ppm.PictureId and ppm.DisplayOrder=1 --get only 1 pic rec
	inner join ProductTemplate pt on p.ProductTemplateId = pt.Id 
	Left  join UrlRecord ur on p.Id = ur.EntityId AND EntityName='Product' AND ur.IsActive=1
	AND ur.LanguageId=0
	where --p.Name like '%'+@keword+'%' and
	 pcm.CategoryId = @category
	ORDER BY pcm.DisplayOrder, p.Name
	end
 RETURN  
   
END

GO
PRINT 'Created UDF [dbo].[ufn_GetProductsBySearch]`'
GO  