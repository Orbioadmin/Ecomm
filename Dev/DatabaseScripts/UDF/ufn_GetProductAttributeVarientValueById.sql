/*
 =============================================  
 Author:  Sankar T.S
 Create date: 02 April 2015
 Description: It will get the all product attribute varient value id
 Return format: int
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetProductAttributeVarientValueById]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetProductAttributeVarientValueById]
	PRINT 'Dropped UDF [dbo].[ufn_GetProductAttributeVarientValueById]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetProductAttributeVarientValueById]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetProductAttributeVarientValueById](@productId INT)
RETURNS int
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @result int;
	 
		SELECT @result = ( SELECT top 1 PVAV.Id
	  FROM dbo.Product_ProductAttribute_Mapping PAM
		inner join dbo.ProductVariantAttributeValue PVAV on PVAV.ProductVariantAttributeId=PAM.Id where PAM.ProductId=@productid  )
		    
		 
	 Return @result

END



GO
PRINT 'Created UDF: ufn_GetProductAttributeVarientValueById'
GO