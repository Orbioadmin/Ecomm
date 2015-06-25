/*
 ===================================================================================================================================================
 Author:  Madhu MB
 Create date: 24 june 2015
 Description: This function will return if discount is valid or not
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetProductDiscounts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetProductDiscounts]
	PRINT 'Dropped UDF [dbo].[ufn_GetProductDiscounts]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetProductDiscounts]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ufn_GetProductDiscounts 
(
	@productId int	
)
RETURNS Xml
AS
BEGIN
	DECLARE @utcNow datetime
	DECLARE @xmlResult xml;
SELECT @utcNow = GETUTCDATE()
SELECT @xmlResult = ( SELECT  
      [Name]
      ,[DiscountTypeId]
      ,[UsePercentage]
      ,[DiscountPercentage]
      ,[DiscountAmount]
      ,[StartDateUtc]
      ,[EndDateUtc]
      ,[RequiresCouponCode]
      ,[CouponCode]
      ,[DiscountLimitationId]
      ,[LimitationTimes] 
  FROM [esybuy].[dbo].[Discount] D
  INNER JOIN Discount_AppliedToProducts DAP ON D.Id = DAP.Discount_Id
  WHERE ((@utcNow BETWEEN StartDateUtc AND EndDateUtc) OR (StartDateUtc is null and EndDateUtc is null) )
  AND RequiresCouponCode = 0 AND dbo.ufn_CheckDiscountLimitation(Id, 1,null)=1
  AND DiscountTypeId in (2,5) AND DAP.Product_Id = @productId
  FOR XML PATH('Discount'),ROOT('Discounts'), elements)
  
  --2 =AssignedToSkus 5=AssignedToCategories
  	 
	 Return @xmlresult
END