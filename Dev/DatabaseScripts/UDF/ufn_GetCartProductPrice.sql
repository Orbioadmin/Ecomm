/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 27 April 2015
 Description: This function will return the cart products price
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetCartProductPrice]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetCartProductPrice]
	PRINT 'Dropped UDF [dbo].[ufn_GetCartProductPrice]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetCartProductPrice]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetCartProductPrice] (@attribute xml,@productid int)  
RETURNS @TABLE TABLE (PriceAdjustment decimal(18,2))  
AS  
BEGIN 

Declare @attributeTable Table (TextPrompt varchar(50),Name varchar(50),PriceAdjustment decimal(18,2))

Insert @attributeTable(TextPrompt,Name,PriceAdjustment)
Select TextPrompt,Name, case when PriceAdjustment is null then convert(int,'0') else PriceAdjustment end from(
(SELECT C.value('@ID','int') AS Attributeid,
C.value('(ProductVariantAttributeValue/Value)[1]','INT') AS [Value],
C.value('(ProductVariantAttributeValue/Value)[2]','INT') AS [Price]
FROM @attribute.nodes('/Attributes/ProductVariantAttribute') as T(C) )t
inner join
(SELECT PPM.Id , CASE WHEN ISNULL(PPM.TextPrompt, '')<>'' THEN PPM.TextPrompt ELSE PA.Name END 
AS TextPrompt
 FROM Product_ProductAttribute_Mapping PPM
INNER JOIN ProductAttribute PA ON PPM.ProductAttributeId = PA.Id
WHERE PPM.ProductId =@productid)PPM on t.Attributeid = PPM.Id )
inner join
(Select  PV.Id,Pv.Name,PV.PriceAdjustment from [dbo].[ProductVariantAttributeValue] PV )Pv on PV.Id = t.Value


 
    INSERT @TABLE
 select sum(PriceAdjustment) from @attributeTable

 RETURN  
   
END


GO
PRINT 'Created UDF [dbo].[ufn_GetCartProductPrice]`'
GO  