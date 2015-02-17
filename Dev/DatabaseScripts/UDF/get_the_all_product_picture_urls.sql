/*
 =============================================  
 Author:  Sankar T.S
 Create date: 06 Feb 2015
 Description: It will get the all product picture urls
 Return Format: XML  
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAllPictureurls]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAllPictureurls]
	PRINT 'Dropped UDF [dbo].[ufn_GetAllPictureurls]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAllPictureurls]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAllPictureurls](@productid INT)
RETURNS xml
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( select product_picture.ProductId 'ProductId',product_picture.PictureId 'PictureId',product_picture.DisplayOrder 'DisplayOrder'  
		,picture.RelativeUrl 'RelativeUrl',picture.MimeType 'MimeType',picture.SeoFilename 'SeoFilename',picture.IsNew 'IsNew' 
		FROM [dbo].[Product] product inner join [dbo].[Product_Picture_Mapping] product_picture on product_picture.ProductId = product.Id
		inner join [dbo].[Picture] picture on picture.Id = product_picture.PictureId
		where Product.Id=@productid 
		FOR XML PATH('ProductPicture'), ROOT('ProductPictures'), TYPE )
		Return @xmlresult
END


GO
PRINT 'Created UDF: ufn_GetAllPictureurls'
GO
