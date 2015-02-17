IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_ValidateAndGetCustomer]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_ValidateAndGetCustomer]
	PRINT 'Dropped [dbo].[usp_Customer_ValidateAndGetCustomer]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_ValidateAndGetCustomer]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_ValidateAndGetCustomer
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

CREATE PROCEDURE [dbo].[usp_Customer_ValidateAndGetCustomer] (@usernameOrEmail nvarchar(1000), @loginResult INT OUTPUT
) 
 
AS  
BEGIN  
  DECLARE @userNamesEnabled bit
  SELECT @userNamesEnabled = CAST(Value AS BIT)FROM Setting WHERE Name = 'customersettings.usernamesenabled'
  IF(@userNamesEnabled=1 AND NOT EXISTS(SELECT 1 FROM Customer WHERE Username = @usernameOrEmail))
  BEGIN
	SET @loginResult = 2
	RETURN
  END
  ELSE IF(@userNamesEnabled=1)
  BEGIN
	IF EXISTS(SELECT 1 FROM Customer WHERE Username = @usernameOrEmail AND Deleted=1)
	BEGIN
		SET @loginResult = 5
		RETURN
	END
	IF EXISTS(SELECT 1 FROM Customer WHERE Username = @usernameOrEmail AND Active=0)
	BEGIN
		SET @loginResult = 4
		RETURN
	END
	IF EXISTS(SELECT 1 FROM Customer  INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id
	WHERE Username = @usernameOrEmail AND  CustomerRole.SystemName <> 'Registered')
	BEGIN
		SET @loginResult = 6
		RETURN
	END
  END
  
  IF(@userNamesEnabled=0 AND NOT EXISTS(SELECT 1 FROM Customer WHERE Email = @usernameOrEmail))
  BEGIN
	SET @loginResult = 2
	RETURN
  END
  ELSE IF(@userNamesEnabled=0)
  BEGIN
	IF EXISTS(SELECT 1 FROM Customer WHERE Email = @usernameOrEmail AND Deleted=1)
	BEGIN
		SET @loginResult = 5
		RETURN
	END
	IF EXISTS(SELECT 1 FROM Customer WHERE Email = @usernameOrEmail AND Active=0)
	BEGIN
		SET @loginResult = 4
		RETURN
	END
	IF NOT EXISTS(SELECT 1 FROM Customer  INNER JOIN Customer_CustomerRole_Mapping CCM ON 
	Customer.Id = CCM.Customer_Id INNER JOIN CustomerRole ON CCM.CustomerRole_Id = CustomerRole.Id
	WHERE Email = @usernameOrEmail AND  CustomerRole.SystemName = 'Registered')
	BEGIN
		SET @loginResult = 6
		RETURN
	END
  END
  
  IF(@userNamesEnabled=1)
  BEGIN
	SELECT *, CAST(1 as BIT) as IsRegistered FROM Customer WHERE Username = @usernameOrEmail
  END
  ELSE
  BEGIN
	SELECT *, CAST(1 as BIT) as IsRegistered FROM Customer WHERE Email = @usernameOrEmail
  END
  
 
END
GO
PRINT 'Created the procedure usp_Customer_ValidateAndGetCustomer'
GO  


