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
	@action varchar(50),
	@id int,
	@email varchar(100),
	@password nvarchar(max),
	@passwordformatid int,
	@passwordsalt nvarchar(max),
	@admincomment nvarchar(max)=NULL,
	@istaxexempt bit,
	@affiliateid int,
	@vendorid int,
	@active bit,
	@deleted bit,
	@issystemaccount bit,
	@systemname nvarchar(max)=NULL,
	@lastipaddress nvarchar(max),
	@createdonutc datetime,
	@lastlogindateutc datetime=NULL,
	@lastactivitydateutc datetime,
	@billingaddress_id int=NULL,
	@shippingaddress_id int=NULL,
	@firstname varchar(50)=NULL,
	@lastname varchar(50)=NULL,
	@gender varchar(100),
	@dob varchar(15)=NULL,
	@companyname varchar(50)=NULL,
	@mobileno varchar(15)=null,
	@customerroles xml=null,
	@customerdiscounts xml=null,
	@insertresult INT OUTPUT
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
 
						values(newid(),@email,@email,@Password,@passwordformatid,@passwordsalt,@admincomment
						,@istaxexempt,@affiliateid,@vendorid,@active,
						@deleted,@issystemaccount,@systemname,@lastipaddress,@createdonutc,@lastlogindateutc,@lastactivitydateutc,
						@billingaddress_id,@shippingaddress_id,@firstname,@lastname,@gender,@dob,@companyname,@mobileno)
						  
						 set @id=SCOPE_IDENTITY()
						  
						IF(@customerroles is not null)
						BEGIN
							insert into dbo.Customer_CustomerRole_Mapping (Customer_Id,CustomerRole_Id) 
							SELECT @id, i.value('.','int') from @customerroles.nodes('/ArrayOfInt/int') x(i)	
						END
						ELSE
						BEGIN
							insert into dbo.Customer_CustomerRole_Mapping (Customer_Id,CustomerRole_Id) 
							values((select Id from dbo.Customer where Username=@email AND Deleted='False')
							,(select top 1 Id from dbo.CustomerRole where SystemName='Registered'))
						END
						
						IF(@customerdiscounts is not null)
						BEGIN
							delete from dbo.Discount_AppliedToCustomers where Customer_Id=@id
							
							insert into dbo.Discount_AppliedToCustomers(Discount_Id,Customer_Id) 
							SELECT i.value('.','int'),@id from @customerdiscounts.nodes('/ArrayOfInt/int') x(i)	
						END
						
						SET @insertresult=1;
					RETURN
					END
				END
			ELSE IF(@Action='Update')
				BEGIN
					UPDATE  dbo.Customer SET Username=@email,Email=@email,Password=@password,PasswordFormatId=@passwordformatid,
					PasswordSalt=@PasswordSalt,AdminComment=@AdminComment,IsTaxExempt=@istaxexempt,VendorId=@vendorid,
					AffiliateId=@affiliateid,Active=@active,Deleted=@deleted,IsSystemAccount=@issystemaccount,
					SystemName=@systemname,LastIpAddress=@lastipaddress,LastLoginDateUtc=@lastlogindateutc,Gender=@gender,
					LastActivityDateUtc=@lastactivitydateutc,MobileNo=@mobileno,FirstName=@firstname,LastName=@lastname, DOB=@dob
					where Id=@id
					
					IF(@customerroles is not null)
						BEGIN
						delete from dbo.Customer_CustomerRole_Mapping where Customer_Id=@Id
						
							insert into dbo.Customer_CustomerRole_Mapping (Customer_Id,CustomerRole_Id) 
							SELECT @id, i.value('.','int') from @customerroles.nodes('/ArrayOfInt/int') x(i)						
						END
						ELSE
						BEGIN
						delete from dbo.Customer_CustomerRole_Mapping where Customer_Id=@Id
						
							insert into dbo.Customer_CustomerRole_Mapping (Customer_Id,CustomerRole_Id) 
							values(@id,(select top 1 Id from dbo.CustomerRole where SystemName='Registered'))
						END
						IF(@customerdiscounts is not null)
						BEGIN
						delete from dbo.Discount_AppliedToCustomers where Customer_Id=@id
						
							insert into dbo.Discount_AppliedToCustomers(Discount_Id,Customer_Id) 
							SELECT i.value('.','int'),@id from @customerdiscounts.nodes('/ArrayOfInt/int') x(i)	
						END
						
					SET @insertresult=1;
					
				END
		END
GO
PRINT 'Created the procedure usp_Customer_InsertCustomer'
GO  
