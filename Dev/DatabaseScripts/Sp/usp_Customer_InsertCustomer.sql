 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_InsertCustomer]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_InsertCustomer]
	PRINT 'Dropped [dbo].[usp_Customer_InsertCustomer]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_InsertCustomer]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_InsertCustomer
# File Path:
# CreatedDate: 23-feb-2015
# Author: Roshni
# Description: This stored procedure insert new user
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

CREATE PROCEDURE [dbo].[usp_Customer_InsertCustomer] 
	-- Add the parameters for the stored procedure here
	@Action varchar(50),
	@email varchar(100),
	@Password nvarchar(max),
	@PasswordFormatId int,
	@PasswordSalt nvarchar(max),
	@AdminComment nvarchar(max)=NULL,
	@IsTaxExempt bit,
	@AffiliateId int,
	@VendorId int,
	@Active bit,
	@Deleted bit,
	@IsSystemAccount bit,
	@SystemName nvarchar(max)=NULL,
	@LastIpAddress nvarchar(max),
	@CreatedOnUtc datetime,
	@LastLoginDateUtc datetime=NULL,
	@LastActivityDateUtc datetime,
	@BillingAddress_Id int=NULL,
	@ShippingAddress_Id int=NULL,
	@firstname varchar(50)=NULL,
	@lastname varchar(50)=NULL,
	@gender varchar(100),
	@dob varchar(15)=NULL,
	@companyname varchar(50)=NULL,
	@mobileno varchar(15),
	@insertResult INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
		if(@Action='Insert')
			BEGIN
				
			IF EXISTS(select Id from dbo.Customer where Username=@email AND Deleted='False')
					BEGIN
					SET @insertResult=2;
					RETURN
					END
				ELSE 
					BEGIN
						INSERT INTO dbo.Customer (CustomerGuid,Username,Email,Password,PasswordFormatId,PasswordSalt,AdminComment
						,IsTaxExempt,AffiliateId,VendorId,Active,Deleted,IsSystemAccount,SystemName,LastIpAddress,CreatedOnUtc,LastLoginDateUtc
						,LastActivityDateUtc,BillingAddress_Id,ShippingAddress_Id,FirstName,LastName,Gender,DOB,CompanyName,MobileNo)
 
						values(newid(),@email,@email,@Password,@PasswordFormatId,@PasswordSalt,@AdminComment
						,(select top 1 TaxExempt from dbo.CustomerRole where SystemName='Registered'),@AffiliateId,@VendorId,@Active,
						@Deleted,@IsSystemAccount,@SystemName,@LastIpAddress,@CreatedOnUtc,@LastLoginDateUtc,@LastActivityDateUtc,
						@BillingAddress_Id,@ShippingAddress_Id,@firstname,@lastname,@gender,@dob,@companyname,@mobileno)
						
						insert into dbo.Customer_CustomerRole_Mapping (Customer_Id,CustomerRole_Id) 
						values((select Id from dbo.Customer where Username=@email AND Deleted='False')
						,(select top 1 Id from dbo.CustomerRole where SystemName='Registered'))
						
						SET @insertResult=1;
					RETURN
					END
				END
END
GO
PRINT 'Created the procedure usp_Customer_InsertCustomer'
GO  
