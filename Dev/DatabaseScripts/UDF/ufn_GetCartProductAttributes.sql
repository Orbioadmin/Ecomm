/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 18 march 2015
 Description: This function will return the cart products attributes
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetCartProductAttributes]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetCartProductAttributes]
	PRINT 'Dropped UDF [dbo].[ufn_GetCartProductAttributes]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetCartProductAttributes]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetCartProductAttributes] (@attribute xml,@productid int)  
RETURNS @TABLE TABLE (VariantValueId int,TextPrompt varchar(500), Name Varchar(100),PriceAdjustment decimal(18,4),WeightAdjustment decimal(18,4))  
AS  
BEGIN  
    INSERT @TABLE
 Select VariantValueId,TextPrompt,Name, case when PriceAdjustment is null then convert(int,'0') else PriceAdjustment end,WeightAdjustment from(
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
(Select  PV.Id VariantValueId,Pv.Name,PV.PriceAdjustment,pv.WeightAdjustment from [dbo].[ProductVariantAttributeValue] PV )Pv on PV.VariantValueId = t.Value
 RETURN  
   
END

GO
PRINT 'Created UDF [dbo].[ufn_GetCartProductAttributes]`'
GO  