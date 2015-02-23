 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CustomerRole_GetCustomerRoleBySystemName]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].usp_CustomerRole_GetCustomerRoleBySystemName
	PRINT 'Dropped [dbo].[usp_CustomerRole_GetCustomerRoleBySystemName]'
END	
GO

PRINT 'Creating [dbo].[usp_CustomerRole_GetCustomerRoleBySystemName]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_CustomerRole_GetCustomerRoleBySystemName
# File Path:
# CreatedDate: 23-feb-2015
# Author: Roshni
# Description: This stored procedure get customer role
# Output Parameter: None
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

CREATE PROCEDURE [dbo].[usp_CustomerRole_GetCustomerRoleBySystemName]
	@systemname nvarchar(255)
	
AS
BEGIN
	SELECT * from dbo.CustomerRole cr  where cr.SystemName=@SystemName order by cr.Id
	RETURN
END
GO
PRINT 'Created the procedure usp_CustomerRole_GetCustomerRoleBySystemName'
GO  
