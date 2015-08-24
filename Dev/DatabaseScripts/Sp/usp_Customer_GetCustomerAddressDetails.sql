 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_GetCustomerAddressDetails]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_GetCustomerAddressDetails]
	PRINT 'Dropped [dbo].[usp_Customer_GetCustomerAddressDetails]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_GetCustomerAddressDetails]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_GetCustomerAddressDetails
# File Path:
# CreatedDate: 10-march-2015
# Author: Roshni
# Description: This stored procedure gets all the customer billing and shipping address
# Output Parameter: resultset output
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

CREATE PROCEDURE [dbo].[usp_Customer_GetCustomerAddressDetails] 
	-- Add the parameters for the stored procedure here
	@userName varchar(50),
	@value varchar(50),
	@shoppingCartStatusId int = null,
	@shoppingCartTypeId int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Update shopping cart status	
	update [dbo].[ShoppingCartItem] set ShoppingCartStatusId = @shoppingCartStatusId where  
	ShoppingCartTypeId=@shoppingCartTypeId and CustomerId = (SELECT Id from dbo.Customer where Username=@userName and Deleted='False') 
	
if(@value='Billing')
BEGIN
    -- Insert statements for procedure here
	select Cust.BillingAddress_Id,Cust.ShippingAddress_Id,Ad.Id,Ad.FirstName,Ad.LastName,Ad.Address1,Ad.Address2
	,Ad.PhoneNumber,Ad.City,Ad.ZipPostalCode,st.Name as States,ct.Name as Country from dbo.Customer Cust
	inner join dbo.Address Ad on  Cust.BillingAddress_Id = Ad.Id
	left outer join dbo.StateProvince st on Ad.StateProvinceId=st.Id
	left outer join dbo.Country ct on Ad.CountryId=ct.Id
	where Cust.Username=@userName
	return
	END
	
else if(@value='Shipping')	
BEGIN
select Cust.BillingAddress_Id,Cust.ShippingAddress_Id,Ad.Id,Ad.FirstName,Ad.LastName,Ad.Address1,Ad.Address2
	,Ad.PhoneNumber,Ad.City,Ad.ZipPostalCode,st.Name as States,ct.Name as Country from dbo.Customer Cust
	inner join dbo.Address Ad on  Cust.ShippingAddress_Id = Ad.Id
	left outer join dbo.StateProvince st on Ad.StateProvinceId=st.Id
	left outer join dbo.Country ct on Ad.CountryId=ct.Id
	where Cust.Username=@userName
	return
	END
END

GO
PRINT 'Created the procedure usp_Customer_GetCustomerAddressDetails'
GO  
