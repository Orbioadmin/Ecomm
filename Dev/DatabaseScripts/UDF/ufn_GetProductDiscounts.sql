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
	


 DECLARE @parentCategoryIds varchar(500)
 DECLARE @categoryId INT 
 SELECT @utcNow = GETUTCDATE()
 DECLARE @xmlResult xml
 
 DECLARE @CatIds TABLE 
 (
	Id INT
 );
 
 --SELECT @categoryId= CategoryId FROM Product_Category_Mapping PCM
	--INNER JOIN Category C ON PCM.CategoryId = C.Id  where ProductId = @productId
	--AND C.Deleted = 0 AND C.Published = 1

 --SELECT @parentCategoryIds = dbo.ufn_GetAllParentCateoryIds(@categoryId,null)
 --SET @parentCategoryIds = @parentCategoryIds + CAST(@categoryId as Nvarchar(100))
 
  INSERT INTO @CatIds
  SELECT * from dbo.ufn_GetPreferredCategoryIds(@productId)
  
SELECT @xmlResult = ( SELECT a.* FROM ( SELECT  
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
  AND DiscountTypeId =2 AND DAP.Product_Id = @productId
 
  
  UNION ALL
  
  SELECT  
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
  INNER JOIN Discount_AppliedToCategories DAP ON D.Id = DAP.Discount_Id
  INNER JOIN @CatIds C ON DAP.Category_Id = C.Id
  WHERE ((@utcNow BETWEEN StartDateUtc AND EndDateUtc) OR (StartDateUtc is null and EndDateUtc is null) )
  AND RequiresCouponCode = 0 AND dbo.ufn_CheckDiscountLimitation(D.Id, 1,null)=1
  AND DiscountTypeId =5) a 
  
   FOR XML PATH('Discount'),ROOT('Discounts'), elements)
  --2 =AssignedToSkus 5=AssignedToCategories
  	 
	 Return @xmlresult
END

GO
PRINT 'Created the UDF ufn_GetProductDiscounts'
GO  