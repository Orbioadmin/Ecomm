IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_EmailSending]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_EmailSending]
	PRINT 'Dropped [dbo].[usp_EmailSending]'
END	
GO

PRINT 'Creating [dbo].[usp_EmailSending]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_EmailSending
# File Path:
# CreatedDate: 23-feb-2015
# Author: Sankar T.S
# Description: This stored procedure Send email
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
Create PROCEDURE [dbo].[usp_EmailSending]
	@profilename varchar(100),
	@toaddress varchar(500),
	@todisplayname varchar(500),
	@subject varchar(1000),
	@body varchar(max)
AS
BEGIN

declare @fromaddress varchar(500) = (select email_address from msdb.dbo.sysmail_account where name=@profilename)
declare @fromname varchar(500) = (select display_name from msdb.dbo.sysmail_account where name=@profilename)
INSERT INTO [dbo].[QueuedEmail]
           ([Priority]
           ,[From]
           ,[FromName]
           ,[To]
           ,[ToName]
           ,[Subject]
           ,[Body]
           ,[CreatedOnUtc]
           ,[SentTries]
           ,[EmailAccountId])
     VALUES
           (5
           ,@fromaddress
           ,@fromname
           ,@toaddress
           ,@todisplayname
           ,@subject
           ,@body
           ,CONVERT(VARCHAR(230),GETDATE(),21)
           ,3
           ,1)
EXEC msdb.dbo.sp_send_dbmail @profile_name=@profilename, @recipients=@toaddress, @subject=@subject, @body=@body,@body_format = 'HTML'

END
GO
PRINT 'Created the procedure usp_EmailSending'
GO  


