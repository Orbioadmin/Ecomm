IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_ChangePassword]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_ChangePassword]
	PRINT 'Dropped [dbo].[usp_Customer_ChangePassword]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_ChangePassword]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_ChangePassword
# File Path:
# CreatedDate: 21-feb-2015
# Author: Sankar T.S
# Description: This stored procedure update customer password
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
Create PROCEDURE [dbo].[usp_Customer_ChangePassword]
	-- Add the parameters for the stored procedure here
	@cust_id int,
	@newpwd varchar(100),
	@passwordformat int

AS
BEGIN

	update dbo.Customer set Password=@newpwd,PasswordFormatId=PasswordFormatId where Id=@cust_id
END

GO
PRINT 'Created the procedure usp_Customer_ChangePassword'
GO  


