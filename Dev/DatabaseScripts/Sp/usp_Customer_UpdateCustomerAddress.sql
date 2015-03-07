IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_UpdateCustomerAddress]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].usp_Customer_UpdateCustomerAddress
	PRINT 'Dropped [dbo].[usp_Customer_UpdateCustomerAddress]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_UpdateCustomerAddress]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_CurrentCustomer
# File Path:
# CreatedDate: 05-feb-2015
# Author: Madhu MB
# Description: This stored procedure gets all the stores
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

CREATE PROCEDURE [dbo].usp_Customer_UpdateCustomerAddress (@sameaddress bit,
@email varchar(50),
@billfirstname varchar(50)=NULL,
@billlastname varchar(50)=NULL,
@billphoneno varchar(50)=NULL,
@billaddress varchar(MAX)=NULL,
@billcity varchar(50)=NULL,
@billpincode varchar(50)=NULL,
@billstate varchar(50)=NULL,
@billcountry varchar(50)=NULL,
@shipfirstname varchar(50)=NULL,
@shiplastname varchar(50)=NULL,
@shipphone varchar(50)=NULL,
@shipaddress varchar(MAX)=NULL,
@shipcity varchar(50)=NULL,
@shippincode varchar(50)=NULL,
@shipstate varchar(50)=NULL,
@shipcountry varchar(50)=NULL)

AS
BEGIN
DECLARE @billaddressid int,
		@shipaddressid int,
		@billid int,
		@shipid int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
SET @billaddressid = (SELECT top 1 BillingAddress_Id from dbo.Customer where Username=@email and Deleted='False')
SET @shipaddressid = (SELECT top 1 ShippingAddress_Id from dbo.Customer where Username=@email and Deleted='False')

	IF((@billaddressid IS NULL OR @billaddressid='' AND @shipaddressid IS NULL OR @shipaddressid='') AND @sameaddress=1)
		BEGIN
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@billfirstname,@billlastname,@email,
			(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT top 1 Id FROM dbo.Country WHERE Name=@billcountry),(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billstate),
			@billcity,@billaddress,'',@billpincode,@billphoneno,'',GETUTCDATE())
			SET @billid = SCOPE_IDENTITY()
		
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@billid)
		
			UPDATE dbo.Customer SET BillingAddress_Id=@billid , ShippingAddress_Id= @billid,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
		
		
		
	ELSE IF((@billaddressid IS NULL OR @billaddressid='' AND @shipaddressid IS NULL OR @shipaddressid='') AND @sameaddress=0)
		BEGIN
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@billfirstname,@billlastname,@email,
			(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT top 1 Id FROM dbo.Country WHERE Name=@billcountry),(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billstate),
			@billcity,@billaddress,'',@billpincode,@billphoneno,'',GETUTCDATE())
			SET @billid = SCOPE_IDENTITY()
		
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@shipfirstname,@shiplastname,@email,
			(SELECT CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT Id FROM dbo.Country WHERE Name=@shipcountry),(SELECT Id FROM dbo.StateProvince WHERE Name=@shipstate),
			@shipcity,@shipaddress,NULL,@shippincode,@shipphone,'',GETUTCDATE())
			SET @shipid = SCOPE_IDENTITY()
		
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@billid)
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@shipid)
		
			UPDATE dbo.Customer SET BillingAddress_Id=@billid , ShippingAddress_Id= @shipid,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
		
	ELSE
		if((@billaddressid = @shipaddressid) AND @sameaddress=1)
			BEGIN
				UPDATE dbo.Address SET FirstName=@billfirstname,LastName=@billlastname,Email=@email
				,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
				,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@billcountry)
				,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billstate),City=@billcity
				,Address1=@billaddress,Address2=NULL,ZipPostalCode=@billpincode,PhoneNumber=@billphoneno
				WHERE Id=@shipaddressid				
			END
			
		ELSE if((@billaddressid = @shipaddressid) AND @sameaddress=0)
			BEGIN
			UPDATE dbo.Address SET FirstName=@shipfirstname,LastName=@shiplastname,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipcountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipstate),City=@billcity
			,Address1=@shipaddress,Address2=NULL,ZipPostalCode=@shippincode,PhoneNumber=@shipphone
			WHERE Id=@shipaddressid
			
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@billfirstname,@billlastname,@email,
			(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT top 1 Id FROM dbo.Country WHERE Name=@billcountry),(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billstate),
			@billcity,@billaddress,NULL,@billpincode,@billphoneno,'',GETUTCDATE())
			SET @billid=SCOPE_IDENTITY()
			
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@shipid)
		
			UPDATE dbo.Customer SET  BillingAddress_Id= @billid,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
		
	ELSE if((@billaddressid <> @shipaddressid) AND @sameaddress=1)
		BEGIN
			UPDATE dbo.Address SET FirstName=@shipfirstname,LastName=@shiplastname,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipcountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipstate),City=@shipcity
			,Address1=@shipaddress,Address2=NULL,ZipPostalCode=@shippincode,PhoneNumber=@shipphone
			WHERE Id=@shipaddressid
			
			UPDATE dbo.Customer SET BillingAddress_Id=@shipaddressid,ShippingAddress_Id=@shipaddressid,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
	ELSE if((@billaddressid <> @shipaddressid) AND @sameaddress=0)
		BEGIN
			UPDATE dbo.Address SET FirstName=@billfirstname,LastName=@billlastname,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@billcountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billstate),City=@billcity
			,Address1=@billaddress,Address2=NULL,ZipPostalCode=@billpincode,PhoneNumber=@billphoneno
			WHERE Id=@billaddressid
			
			UPDATE dbo.Address SET FirstName=@shipfirstname,LastName=@shiplastname,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipcountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipstate),City=@shipcity
			,Address1=@shipaddress,Address2=NULL,ZipPostalCode=@shippincode,PhoneNumber=@shipphone
			WHERE Id=@shipaddressid
		END
	END
GO
PRINT 'Created the procedure usp_Customer_UpdateCustomerAddress'
GO  


