/*
 ===================================================================================================================================================
 Author:  Madhu MB
 Create date: 26 june 2015
 Description: This function will return valid order/subtotal discounts
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetOrderDiscounts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetOrderDiscounts]
	PRINT 'Dropped UDF [dbo].[ufn_GetOrderDiscounts]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetOrderDiscounts]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ufn_GetOrderDiscounts ()
 
RETURNS Xml
AS
BEGIN
	DECLARE @utcNow datetime
	

 
 SELECT @utcNow = GETUTCDATE()
 DECLARE @xmlResult xml
 
  
  
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
  WHERE ((@utcNow BETWEEN StartDateUtc AND EndDateUtc) OR (StartDateUtc is null and EndDateUtc is null) )
  AND RequiresCouponCode = 0 AND dbo.ufn_CheckDiscountLimitation(Id, 1,null)=1
  AND DiscountTypeId in (1,20) 
 
  
  
   FOR XML PATH('Discount'),ROOT('Discounts'), elements)
  --2 =AssignedToSkus 5=AssignedToCategories
  	 
	 Return @xmlresult
END

GO
PRINT 'Created the UDF ufn_GetOrderDiscounts'
GO  