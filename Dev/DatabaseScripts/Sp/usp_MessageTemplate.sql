IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_MessaeTemplate]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_MessaeTemplate]
	PRINT 'Dropped [dbo].[usp_MessaeTemplate]'
END	
GO

PRINT 'Creating [dbo].[usp_MessaeTemplate]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_MessaeTemplate
# File Path:
# CreatedDate: 24-feb-2015
# Author: Sankar T.S
# Description: This stored procedure get all message template
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
CREATE PROCEDURE [dbo].[usp_MessaeTemplate] 
	-- Add the parameters for the stored procedure here
	@messagename varchar(50)
AS
BEGIN

SELECT * FROM [dbo].[MessageTemplate] where Name = @messagename

END

GO
PRINT 'Created the procedure usp_MessaeTemplate'
GO  


