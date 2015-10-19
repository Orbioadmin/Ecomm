/*
 ===================================================================================================================================================
 Author:  Sankar
 Create date: 19 oct 2015
 Description: This function will return if discount is valid or not
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_CheckDiscountForCustomer]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_CheckDiscountForCustomer]
	PRINT 'Dropped UDF [dbo].[ufn_CheckDiscountForCustomer]'
GO
PRINT 'Creating UDF [dbo].[ufn_CheckDiscountForCustomer]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_CheckDiscountForCustomer] 
(
	@discountId int, @customerId int
)
RETURNS bit
AS
BEGIN
	DECLARE @isValidDiscount bit = 0
	
	IF EXISTS(SELECT * FROM Discount D INNER JOIN Discount_AppliedToCustomers DAC ON D.Id = DAC.Discount_Id
				WHERE D.Id = @discountId AND DAC.Customer_Id=@customerId)
			BEGIN
				SET @isValidDiscount = 1
			END
	RETURN @isValidDiscount

END


GO
PRINT 'Created the UDF ufn_CheckDiscountForCustomer'
GO  