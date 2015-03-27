IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_updateCustomer]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_updateCustomer]
	PRINT 'Dropped [dbo].[usp_Customer_updateCustomer]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_updateCustomer]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_updateCustomer
# File Path:
# CreatedDate: 18-feb-2015
# Author: Roshni
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

CREATE PROCEDURE [dbo].[usp_Customer_updateCustomer]
	-- Add the parameters for the stored procedure here
	
	@Action varchar(50),
	@cust_id int,
	@email varchar(100),
	@firstName varchar(50),
	@lastName varchar(50),
	@gender varchar(10),
	@dob varchar(15),
	@mobileNo varchar(15)
AS
BEGIN

if(@Action = 'Update')
begin
	update [dbo].[Customer] set FirstName=@firstName,LastName=@lastName, Gender=@gender, DOB=@dob, MobileNo=@mobileNo,
	Email=@email where Id=@cust_id
 End
END

GO
PRINT 'Created the procedure usp_Customer_updateCustomer'
GO  


