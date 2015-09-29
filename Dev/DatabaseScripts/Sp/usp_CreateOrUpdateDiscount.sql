IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CreateOrUpdateDiscount]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_CreateOrUpdateDiscount]
	PRINT 'Dropped [dbo].[usp_CreateOrUpdateDiscount]'
END	
GO

PRINT 'Creating [dbo].[usp_CreateOrUpdateDiscount]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_CreateOrUpdateDiscount
# File Path:
# CreatedDate: 28-SEP-2015
# Author: Sankar
# Description: This stored procedure select,create,update and delete for a discount
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
CREATE PROCEDURE [dbo].[usp_CreateOrUpdateDiscount] 
	-- Add the parameters for the stored procedure here
	@action varchar(50) = null,
	@id int = null,
	@name nvarchar(200) = null,
	@discountTypeId int = null,
	@usePercentage bit = null,
	@discountPercentage decimal(18,4) = null,
	@discountAmount decimal(18,4) = null,
	@startDateUtc datetime = null,
	@endDateUtc datetime = null,
	@requiresCouponCode bit = null,
	@couponCode nvarchar(100) = null,
	@discountLimitationId int = null,
	@limitationTime int = null
AS
BEGIN

if(@action = 'selectById')
	begin
	
	DECLARE @XmlResult1 xml

SELECT @XmlResult1 = (SELECT Id, Name,DiscountTypeId,UsePercentage,DiscountPercentage,DiscountAmount,StartDateUtc,
EndDateUtc,RequiresCouponCode,CouponCode,DiscountLimitationId,LimitationTimes,dbo.ufn_GetproductdetailsByDiscountId(Id),
dbo.ufn_GetCategories(Id)
from dbo.Discount 
where Id=@id order by Id desc
FOR XML PATH('Discount'))

SELECT @XmlResult1 as XmlResult
	
	end

else if(@action = 'insert')
	begin
		
		insert into dbo.Discount(Name,DiscountTypeId,UsePercentage,DiscountPercentage,DiscountAmount,StartDateUtc,EndDateUtc,RequiresCouponCode,CouponCode,DiscountLimitationId,LimitationTimes)
		values(@name,@discountTypeId,@usePercentage,@discountPercentage,@discountAmount,@startDateUtc,@endDateUtc,@requiresCouponCode,@couponCode,@discountLimitationId,@limitationTime)
		
	end

else if(@action = 'update')
	begin
		
		update dbo.Discount set Name = @name,DiscountTypeId=@discountTypeId,UsePercentage=@usePercentage,
		DiscountPercentage = @discountPercentage,DiscountAmount = @discountAmount,StartDateUtc = @startDateUtc,EndDateUtc=@endDateUtc,
		RequiresCouponCode=@requiresCouponCode,CouponCode = @couponCode,DiscountLimitationId=@discountLimitationId,LimitationTimes=@limitationTime
		where Id = @id
		
	end

else if(@action = 'delete')
	begin
		
		delete from dbo.Discount where Id=@id
		
	end
	
END



GO
PRINT 'Created the procedure usp_CreateOrUpdateDiscount'
GO  


