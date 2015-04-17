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

Create PROCEDURE [dbo].[usp_Customer_UpdateCustomerAddress] (@sameAddress bit,
@email varchar(50),
@billFirstName varchar(50)=NULL,
@billLastName varchar(50)=NULL,
@billPhoneNo varchar(50)=NULL,
@billAddress varchar(MAX)=NULL,
@billCity varchar(50)=NULL,
@billPinCode varchar(50)=NULL,
@billState varchar(50)=NULL,
@billCountry varchar(50)=NULL,
@shipFirstName varchar(50),
@shipLastName varchar(50),
@shipPhone varchar(50),
@shipAddress varchar(MAX),
@shipCity varchar(50),
@shipPincode varchar(50),
@shipState varchar(50),
@shipCountry varchar(50))

AS
BEGIN
DECLARE @billAddressId int,
		@shipAddressId int,
		@billId int,
		@shipId int

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
SET @billAddressId = (SELECT top 1 BillingAddress_Id from dbo.Customer where Username=@email and Deleted='False')
SET @shipAddressId = (SELECT top 1 ShippingAddress_Id from dbo.Customer where Username=@email and Deleted='False')

	IF((@billAddressId IS NULL OR @billAddressId='' AND @shipAddressId IS NULL OR @shipAddressId='') AND @sameAddress=1)
		BEGIN
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@shipFirstName,@shipLastName,@email,
			(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipCountry),(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipState),
			@shipCity,@shipAddress,'',@shipPincode,@shipPhone,'',GETUTCDATE())
			SET @billId = SCOPE_IDENTITY()
		
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@billId)
		
			UPDATE dbo.Customer SET BillingAddress_Id=@shipId , ShippingAddress_Id= @shipId,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
		
		
		
	ELSE IF((@billAddressId IS NULL OR @billAddressId='' AND @shipAddressId IS NULL OR @shipAddressId='') AND @sameAddress=0)
		BEGIN
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@billFirstName,@billLastName,@email,
			(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT top 1 Id FROM dbo.Country WHERE Name=@billCountry),(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billState),
			@billCity,@billAddress,'',@billPincode,@billPhoneNo,'',GETUTCDATE())
			SET @billid = SCOPE_IDENTITY()
		
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@shipFirstName,@shipLastName,@email,
			(SELECT CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT Id FROM dbo.Country WHERE Name=@shipCountry),(SELECT Id FROM dbo.StateProvince WHERE Name=@shipState),
			@shipCity,@shipAddress,NULL,@shipPincode,@shipPhone,'',GETUTCDATE())
			SET @shipId = SCOPE_IDENTITY()
		
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@billId)
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@shipId)
		
			UPDATE dbo.Customer SET BillingAddress_Id=@billId , ShippingAddress_Id= @shipId,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
		
	ELSE
		if((@billaddressId = @shipAddressId) AND @sameAddress=1)
			BEGIN
				UPDATE dbo.Address SET FirstName=@shipFirstName,LastName=@shipLastName,Email=@email
				,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
				,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipCountry)
				,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipState),City=@shipCity
				,Address1=@shipAddress,Address2=NULL,ZipPostalCode=@shipPincode,PhoneNumber=@shipPhone
				WHERE Id=@shipAddressId				
			END
			
		ELSE if((@billAddressId = @shipAddressId) AND @sameAddress=0)
			BEGIN
			UPDATE dbo.Address SET FirstName=@shipFirstName,LastName=@shipLastName,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipCountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipState),City=@shipCity
			,Address1=@shipAddress,Address2=NULL,ZipPostalCode=@shipPincode,PhoneNumber=@shipPhone
			WHERE Id=@shipAddressId
			
			INSERT INTO dbo.Address (FirstName,LastName,Email,Company,CountryId,StateProvinceId,City,Address1,Address2
			,ZipPostalCode,PhoneNumber,FaxNumber,CreatedOnUtc) VALUES(@billFirstName,@billLastName,@email,
			(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False'),
			(SELECT top 1 Id FROM dbo.Country WHERE Name=@billCountry),(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billState),
			@billCity,@billAddress,NULL,@billPincode,@billPhoneNo,'',GETUTCDATE())
			SET @billId=SCOPE_IDENTITY()
			
			INSERT INTO dbo.CustomerAddresses (Customer_Id,Address_Id) VALUES((SELECT top 1 Id FROM dbo.Customer WHERE Username=@email and Deleted='False'),@shipId)
		
			UPDATE dbo.Customer SET  BillingAddress_Id= @billId,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
		
	ELSE if((@billAddressId <> @shipAddressId) AND @sameAddress=1)
		BEGIN
			UPDATE dbo.Address SET FirstName=@shipFirstName,LastName=@shipLastName,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipCountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipState),City=@shipCity
			,Address1=@shipAddress,Address2=NULL,ZipPostalCode=@shipPincode,PhoneNumber=@shipPhone
			WHERE Id=@shipAddressId
			
			UPDATE dbo.Customer SET BillingAddress_Id=@shipAddressId,ShippingAddress_Id=@shipAddressId,LastActivityDateUtc=GETUTCDATE()
			WHERE Username=@email and Deleted='False'
		END
	ELSE if((@billAddressId <> @shipAddressId) AND @sameAddress=0)
		BEGIN
			UPDATE dbo.Address SET FirstName=@billFirstName,LastName=@billLastName,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@billCountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@billState),City=@billCity
			,Address1=@billAddress,Address2=NULL,ZipPostalCode=@billPincode,PhoneNumber=@billPhoneNo
			WHERE Id=@billAddressId
			
			UPDATE dbo.Address SET FirstName=@shipFirstName,LastName=@shipLastName,Email=@email
			,Company=(SELECT top 1 CompanyName FROM dbo.Customer WHERE Username=@email and Deleted='False')
			,CountryId=(SELECT top 1 Id FROM dbo.Country WHERE Name=@shipCountry)
			,StateProvinceId=(SELECT top 1 Id FROM dbo.StateProvince WHERE Name=@shipState),City=@shipCity
			,Address1=@shipAddress,Address2=NULL,ZipPostalCode=@shipPincode,PhoneNumber=@shipPhone
			WHERE Id=@shipAddressId
		END
	END
GO
PRINT 'Created the procedure usp_Customer_UpdateCustomerAddress'
GO  


