IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrderNote_DeleteOrderNote]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrderNote_DeleteOrderNote]
	PRINT 'Dropped [dbo].[usp_OrderNote_DeleteOrderNote]'
END	
GO

PRINT 'Creating [dbo].[usp_OrderNote_DeleteOrderNote]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrderNote_DeleteOrderNote
# File Path:
# CreatedDate: 17-Aug-2015
# Author: Sankar T S
# Description: Delete order note by id
# Output Parameter: none
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
Create PROCEDURE [dbo].[usp_OrderNote_DeleteOrderNote]
	@id int
AS
BEGIN

delete from dbo.OrderNote where Id = @id
	
END


GO
PRINT 'Created the procedure usp_OrderNote_DeleteOrderNote'
GO  