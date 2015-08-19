IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Delete_OrderItem]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Delete_OrderItem]
	PRINT 'Dropped [dbo].[usp_Delete_OrderItem]'
END	
GO

PRINT 'Creating [dbo].[usp_Delete_OrderItem]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Delete_OrderItem
# File Path:
# CreatedDate: 17-Aug-2015
# Author: Sankar T S
# Description: This stored procedure delete order item
# History  of changes:
#--------------------------------------------------------------------------------------
# Version No.	Date of Change		Changed By		Reason for change
#--------------------------------------------------------------------------------------
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Delete_OrderItem]
	@id int
AS
BEGIN

delete from dbo.OrderItem where Id = @id
	
END

GO
PRINT 'Created the procedure usp_Delete_OrderItem'
GO  