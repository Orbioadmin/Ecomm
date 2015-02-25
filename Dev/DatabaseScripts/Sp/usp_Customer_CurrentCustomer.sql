IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_CurrentCustomer]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_CurrentCustomer]
	PRINT 'Dropped [dbo].[usp_Customer_CurrentCustomer]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_CurrentCustomer]'
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

CREATE PROCEDURE [dbo].[usp_Customer_CurrentCustomer] (@checkBackgroundCustomer bit=0,
@isSearchEngine bit=0, @authenticatedCustomerData nvarchar(MAX)=NULL,
@customerByCookieGuid varchar(500)=NULL, @ipAddress nvarchar(1000)
) 
 
AS  
BEGIN  
  
  IF(@checkBackgroundCustomer = 1)
  BEGIN
     IF EXISTS(SELECT 1 FROM Customer  WHERE SystemName = 'BackgroundTask' AND Deleted = 0 AND Active=1)
     BEGIN
		SELECT TOP 1 *, CAST(0 as BIT) as IsRegistered  FROM Customer WHERE SystemName = 'BackgroundTask'
		AND Deleted = 0 AND Active=1		
		RETURN
     END
  END
  IF ( @isSearchEngine =1 )
  BEGIN	
	 IF EXISTS(SELECT 1 FROM Customer  WHERE SystemName = 'SearchEngine' AND Deleted = 0 AND Active=1)
     BEGIN
		SELECT TOP 1 *, CAST(0 as BIT) as IsRegistered  FROM Customer WHERE SystemName = 'SearchEngine'
		AND Deleted = 0 AND Active=1
		RETURN
     END
  END
  IF(@authenticatedCustomerData IS NOT NULL)
  BEGIN
	DECLARE @userNamesEnabled bit
	SELECT @userNamesEnabled = CAST(Value AS BIT)FROM Setting WHERE Name = 'customersettings.usernamesenabled'
	IF(@userNamesEnabled = 1 AND EXISTS(SELECT  1 FROM Customer INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id WHERE Username = @authenticatedCustomerData AND CustomerRole.SystemName ='Registered'
		AND Deleted = 0 AND Customer.Active=1))
	BEGIN
		SELECT TOP 1 *, CAST(1 as BIT) as IsRegistered  FROM Customer INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id WHERE Username = @authenticatedCustomerData AND CustomerRole.SystemName ='Registered'
		AND Deleted = 0 AND Customer.Active=1
		RETURN
	END	
	ELSE IF EXISTS(SELECT  1 FROM Customer INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id  WHERE Email = @authenticatedCustomerData AND CustomerRole.SystemName ='Registered'
		AND Deleted = 0 AND Customer.Active=1)
	BEGIN
		SELECT  TOP 1 *, CAST(1 as BIT) as IsRegistered  FROM Customer INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id  WHERE Email = @authenticatedCustomerData AND CustomerRole.SystemName ='Registered'
		AND Deleted = 0 AND Customer.Active=1
		RETURN
	END
  END
  --SKIPPING IMPERSONATED USER 
  IF(@customerByCookieGuid IS NOT NULL)
  BEGIN
	IF EXISTS(SELECT 1 FROM Customer WHERE CustomerGuid=@customerByCookieGuid) AND NOT EXISTS(SELECT 1 FROM Customer INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id
	 WHERE CustomerGuid = @customerByCookieGuid
		AND Deleted = 0 AND Customer.Active=1 AND CustomerRole.Active=1
		AND CustomerRole.SystemName = 'Registered')
		BEGIN
			SELECT TOP 1 *, CAST(0 as BIT) as IsRegistered  FROM Customer 
			--INNER JOIN Customer_CustomerRole_Mapping CCM ON 
			--Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id
			 WHERE CustomerGuid = @customerByCookieGuid
				AND Deleted = 0 AND Customer.Active=1 --AND CustomerRole.Active=1
				--AND CustomerRole.SystemName <> 'Registered'
			RETURN
		END
  END
  --NONE OF THE ABOVE RETURNS A CUSTOMER THEN CREATE GUEST CUSTOMER
  DECLARE @guestCustomerId INT
  INSERT INTO Customer(CustomerGuid,Active,CreatedOnUtc,LastActivityDateUtc, PasswordFormatId, IsTaxExempt,
	AffiliateId,VendorId,  Deleted,IsSystemAccount ,LastIpAddress)
  SELECT NEWID() , 1, GETUTCDATE(), GETUTCDATE(),0 ,0,0,0, 0,0,@ipAddress
  SET @guestCustomerId = SCOPE_IDENTITY()
  
  INSERT INTO Customer_CustomerRole_Mapping
  SELECT @guestCustomerId, Id FROM CustomerRole WHERE SystemName='Guests'
  
  SELECT *, CAST(0 as BIT) as IsRegistered  FROM Customer WHERE Id = @guestCustomerId
 
END
GO
PRINT 'Created the procedure usp_Customer_CurrentCustomer'
GO  


