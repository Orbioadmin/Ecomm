 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Update_OrderNote]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Update_OrderNote]
	PRINT 'Dropped [dbo].[usp_Update_OrderNote]'
END	
GO

PRINT 'Creating [dbo].[usp_Update_OrderNote]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Update_OrderNote
# File Path:
# CreatedDate: 11-march-2015
# Author: Sankar T S
# Description: This procedure to use update order note.
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
CREATE PROCEDURE [dbo].[usp_Update_OrderNote] 
	-- Add the parameters for the stored procedure here
	@orderId int,
	@note XML
	
AS
BEGIN

INSERT INTO OrderNote(OrderId,Note,DisplayToCustomer,CreatedOnUtc)
SELECT @orderId, O.D.value('(Note)[1]','nvarchar(MAX)'), 
   O.D.value('(DisplayToCustomer)[1]','bit'), d.value('(CreatedOnUtc)[1]','DATETIME')
   FROM
  @note.nodes('/ArrayOfOrderNote/OrderNote') O(D)

END
GO
PRINT 'Created the procedure usp_Update_OrderNote'
GO  
