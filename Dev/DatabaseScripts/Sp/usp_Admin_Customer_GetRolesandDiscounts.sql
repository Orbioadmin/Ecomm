 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Admin_Customer_GetRolesandDiscounts]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Admin_Customer_GetRolesandDiscounts]
	PRINT 'Dropped [dbo].[usp_Admin_Customer_GetRolesandDiscounts]'
END	
GO

PRINT 'Creating [dbo].[usp_Admin_Customer_GetRolesandDiscounts]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Admin_Customer_GetRolesandDiscounts
# File Path:
# CreatedDate: 19-oct-2015
# Author: Roshni
# Description: This stored procedure gets customer roles and discounts
# Output Parameter: insertResult  output
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

CREATE PROCEDURE [dbo].[usp_Admin_Customer_GetRolesandDiscounts]
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
DECLARE @XmlResult xml
	SELECT @XmlResult = (select(select * from CustomerRole FOR XML PATH('CustomerRole'), ROOT('CustomerRoles'),Type),
	(select * from Discount where DiscountTypeId=50 and EndDateUtc>=GETDATE() FOR XML PATH('Discount'), ROOT('Discounts'),Type),
	(select D.* from Discount D inner join Discount_AppliedToCustomers DC on D.Id=DC.Discount_Id where DC.Customer_Id=@id and D.EndDateUtc>=GETDATE()
	 FOR XML PATH('Discount'), ROOT('SelectedDiscount'),Type)
	 FOR XML PATH('AdminCustomer'))
	 
	 SELECT @XmlResult as XmlResult
END

GO
PRINT 'Created the procedure usp_Admin_Customer_GetRolesandDiscounts'
GO  
