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
CREATE FUNCTION ufn_GetOrderDiscounts (
@customerId int,
@storeId int
)
 
RETURNS Xml
AS
BEGIN
  
  DECLARE @utcNow datetime
  SELECT @utcNow = GETUTCDATE()
  DECLARE @xmlResult xml
  DECLARE @couponCode nvarchar(100)
  
 --get coupon discounts also \
 DECLARE @couponDiscount TABLE(Id INT,
 [Name] [nvarchar](200) ,
	[DiscountTypeId] [int] ,
	[UsePercentage] [bit] ,
	[DiscountPercentage] [decimal](18, 4) ,
	[DiscountAmount] [decimal](18, 4),
	[StartDateUtc] [datetime] ,
	[EndDateUtc] [datetime] ,
	[RequiresCouponCode] [bit] ,
	[CouponCode] [nvarchar](100) ,
	[DiscountLimitationId] [int],
	[LimitationTimes] [int],
	IsValid BIT
 )
 
	 SELECT @couponCode = Value FROM GenericAttribute WHERE EntityId = @customerId AND KeyGroup = 'Customer'
		AND [Key]='DiscountCouponCode'

 
	IF(@couponCode IS NOT NULL)
	BEGIN	
		 
		DECLARE @discountId INT
	   
		SELECT @discountId = Id FROM Discount  WHERE CouponCode = @couponCode AND 
			RequiresCouponCode =1 AND ((@utcNow BETWEEN StartDateUtc AND EndDateUtc) OR (StartDateUtc is				null and		EndDateUtc is null) )
		
		IF (@discountId IS NOT NULL AND EXISTS( SELECT DBO.ufn_CheckDiscountLimitation(@discountId,							@customerId,NULL)))
		BEGIN		
			INSERT INTO @couponDiscount
			 SELECT  Id, [Name]
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
			  , 1  
			   FROM Discount WHERE Id = @discountId
		END
		ELSE
		BEGIN
			INSERT INTO @couponDiscount([RequiresCouponCode],CouponCode, IsValid)
			VALUES(1,@couponCode, 0)
			 
		END				
	END
	 
	SELECT @xmlResult = (SELECT a.* FROM ( SELECT  D.Id,
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
      ,1 AS IsValid
  FROM [esybuy].[dbo].[Discount] D
  WHERE ((@utcNow BETWEEN StartDateUtc AND EndDateUtc) OR (StartDateUtc is null and EndDateUtc is null) )
  AND RequiresCouponCode = 0 AND dbo.ufn_CheckDiscountLimitation(Id, 1,null)=1
  AND DiscountTypeId in (1,20) 
  UNION ALL
  SELECT * FROM @couponDiscount
  ) a
   FOR XML PATH('Discount'),ROOT('Discounts'), elements)
  --1 =oRDERsUBTOTAL 20=ORDERTOTAL
  	 
	 Return @xmlresult
END

GO
PRINT 'Created the UDF ufn_GetOrderDiscounts'
GO  