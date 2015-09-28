IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_GetAllDiscount]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrbioAdmin_GetAllDiscount]
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_GetAllDiscount]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_GetAllDiscount]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_GetAllDiscount
# File Path:
# CreatedDate: 25-SEP-2015
# Author: Sankar T.S
# Description: This stored procedure get all discounts
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
Create PROCEDURE [dbo].[usp_OrbioAdmin_GetAllDiscount]
AS
BEGIN
DECLARE @XmlResult1 xml

SELECT @XmlResult1 = (SELECT Id,Name,DiscountTypeId,UsePercentage,DiscountPercentage,DiscountAmount,StartDateUtc,
EndDateUtc,RequiresCouponCode,CouponCode,DiscountLimitationId,LimitationTimes
from dbo.Discount
FOR XML PATH('Discount'),Root('ArrayOfDiscount'))

SELECT @XmlResult1 as XmlResult

END




GO
PRINT 'Created the procedure usp_OrbioAdmin_GetAllDiscount'
GO  


