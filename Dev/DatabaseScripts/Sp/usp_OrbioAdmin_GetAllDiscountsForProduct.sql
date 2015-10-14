IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_GetAllDiscountsForProduct]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrbioAdmin_GetAllDiscountsForProduct]
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_GetAllDiscountsForProduct]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_GetAllDiscountsForProduct]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_GetAllDiscountsForProduct
# File Path:
# CreatedDate: 14-OCT-2015
# Author: Sankar
# Description: This stored procedure get all discounts for products
 # Return Parameter: None
# History  of changes:
#--------------------------------------------------------------------------------------
# Version No.	Date of Change		Changed By		Reason for change
#--------------------------------------------------------------------------------------
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_OrbioAdmin_GetAllDiscountsForProduct]
@discountTypeId int
AS
BEGIN
DECLARE @XmlResult1 xml

SELECT @XmlResult1 = (SELECT Id, Name,DiscountTypeId,UsePercentage,DiscountPercentage,DiscountAmount,StartDateUtc,
EndDateUtc,RequiresCouponCode,CouponCode,DiscountLimitationId,LimitationTimes
from dbo.Discount where DiscountTypeId = @discountTypeId
FOR XML PATH('Discount'),Root('ArrayOfDiscount'))

SELECT @XmlResult1 as XmlResult

END






GO
PRINT 'Created the procedure usp_OrbioAdmin_GetAllDiscountsForProduct'
GO  


