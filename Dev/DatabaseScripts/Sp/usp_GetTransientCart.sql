 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetTransientCart]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_GetTransientCart]
	PRINT 'Dropped [dbo].[usp_GetTransientCart]'
END	
GO

PRINT 'Creating [dbo].[usp_GetTransientCart]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_GetTransientCart
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

CREATE PROCEDURE [dbo].[usp_GetTransientCart]
	 @transientCartId int,
	 @customerId int
AS
BEGIN
	 DECLARE @XmlResult xml
	 
	 SELECT @xmlResult = TransientCartXml FROM dbo.TransientCart WHERE Id = @transientCartId
	 AND CustomerId = @customerId
	 
	 SELECT @XmlResult  as XmlResult
END


GO
PRINT 'Created the procedure usp_GetTransientCart'
GO  
