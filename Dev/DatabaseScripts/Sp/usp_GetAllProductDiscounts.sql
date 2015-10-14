IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllProductDiscounts]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_GetAllProductDiscounts]
	PRINT 'Dropped [dbo].[usp_GetAllProductDiscounts]'
END	
GO

PRINT 'Creating [dbo].[usp_GetAllProductDiscounts]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_GetAllProductDiscounts
# File Path:
# CreatedDate: 14-OCT-2015
# Author: Sankar
# Description: This stored procedure get all discounts for product by product id
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
CREATE PROCEDURE [dbo].[usp_GetAllProductDiscounts]
@productid int
AS
BEGIN
select [Discount_Id] into #temtable from [dbo].[Discount_AppliedToProducts] where [Product_Id] = @productid

declare @discountIds VARCHAR(MAX) = (SELECT DISTINCT STUFF((SELECT distinct ',' + convert(varchar,p1.Discount_Id)
         FROM #temtable p1
            FOR XML PATH(''), TYPE
            ).value('.', 'VARCHAR(MAX)')
        ,1,1,'') Color
FROM #temtable p)

DECLARE @XmlResult1 xml

SELECT @XmlResult1 = (SELECT Id, Name,DiscountTypeId,UsePercentage,DiscountPercentage,DiscountAmount,StartDateUtc,
EndDateUtc,RequiresCouponCode,CouponCode,DiscountLimitationId,LimitationTimes
from dbo.Discount where ','+@discountIds+',' LIKE '%,'+CAST(Id AS varchar)+',%'
FOR XML PATH('Discount'),Root('ArrayOfDiscount'))

SELECT @XmlResult1 as XmlResult

END




GO
PRINT 'Created the procedure usp_GetAllProductDiscounts'
GO  


