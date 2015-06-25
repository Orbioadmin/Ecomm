/*
 ===================================================================================================================================================
 Author:  Madhu MB
 Create date: 24 june 2015
 Description: This function will return if discount is valid or not
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_CheckDiscountLimitation]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_CheckDiscountLimitation]
	PRINT 'Dropped UDF [dbo].[ufn_CheckDiscountLimitation]'
GO
PRINT 'Creating UDF [dbo].[ufn_CheckDiscountLimitation]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION ufn_CheckDiscountLimitation 
(
	@discountId int, @customerId int, @orderId int
)
RETURNS bit
AS
BEGIN
	DECLARE @isValidDiscount bit = 0
	DECLARE @discountLimitationId int , @limitationTimes int, @usageCount int
	
	SELECT @discountLimitationId = DiscountLimitationId, @limitationTimes=LimitationTimes FROM Discount WHERE Id=@discountId
	 IF(@discountLimitationId=0) --UNLIMITED
	 BEGIN
		SET @isValidDiscount = 1
	 END
	 ELSE IF(@discountLimitationId=15) --NTimesonly
	 BEGIN
		SELECT @usageCount = COUNT(Id) FROM DiscountUsageHistory WHERE DiscountId=@discountId
		IF(@usageCount<@limitationTimes)
		BEGIN
			SET @isValidDiscount =1 
		END
	 END
	  ELSE IF(@discountLimitationId=25) --NTimesPerCustomer
	 BEGIN
		SELECT @usageCount = COUNT(DUH.Id) FROM DiscountUsageHistory DUH
		INNER JOIN [Order] O ON DUH.OrderId = O.Id AND O.CustomerId = @customerId WHERE DiscountId=@discountId
		
		IF(@usageCount<@limitationTimes)
		BEGIN
			SET @isValidDiscount =1 
		END
	 END
	RETURN @isValidDiscount

END
GO

GO
PRINT 'Created UDF [dbo].[ufn_CheckDiscountLimitation]`'
GO  
