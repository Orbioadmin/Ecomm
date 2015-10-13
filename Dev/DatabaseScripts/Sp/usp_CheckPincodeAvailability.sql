 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_CheckPincodeAvailability]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_CheckPincodeAvailability]
	PRINT 'Dropped [dbo].[usp_CheckPincodeAvailability]'
END	
GO

PRINT 'Creating [dbo].[usp_CheckPincodeAvailability]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetFiltersByCategory
# File Path:
# CreatedDate: 03-march-2015
# Author: Sankar T S
# Description: This stored procedure gets all the products for the categoryids
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

CREATE PROCEDURE [dbo].[usp_CheckPincodeAvailability] (@state varchar(200)) 
AS    
BEGIN 
 
 DECLARE @result varchar(100)
    -- Insert statements for procedure here
	if exists(select name from dbo.StateProvince S where Lower(S.Name)=Lower(@state))
	set @result='Available'
	ELSE
	set @result='UnAvailable'
	
	select @result
  
END

GO
PRINT 'Created the procedure usp_CheckPincodeAvailability'
GO  
