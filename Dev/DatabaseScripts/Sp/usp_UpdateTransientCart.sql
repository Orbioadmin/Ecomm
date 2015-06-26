 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateTransientCart]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_UpdateTransientCart]
	PRINT 'Dropped [dbo].[usp_UpdateTransientCart]'
END	
GO

PRINT 'Creating [dbo].[usp_UpdateTransientCart]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_UpdateTransientCart
# File Path:
# CreatedDate: 26-jun-2015
# Author: Madhu M B
# Description: This procedure to use add or update transient cart.
# Output Parameter: transient cart id
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

CREATE PROCEDURE [dbo].[usp_UpdateTransientCart]
	 @transientCartId int,
	 @customerId int,
	 @transientCartXml xml	 
AS
BEGIN
	DECLARE @Id int =  @transientCartId
	
	IF EXISTS(SELECT 1 FROM TransientCart WHERE Id=@transientCartId AND CustomerId= @customerId)
	BEGIN
		UPDATE dbo.TransientCart SET TransientCartXml = @transientCartXml , ModifiedDate = GETDATE()
		WHERE Id = @transientCartId AND CustomerId= @customerId
	END
	ELSE
	BEGIN
		INSERT INTO dbo.TransientCart(CustomerId,TransientCartXml)
		VALUES(@customerId, @transientCartXml)
		SET @transientCartId = SCOPE_IDENTITY()
	END
	
	RETURN @Id
END


GO
PRINT 'Created the procedure usp_UpdateTransientCart'
GO  
