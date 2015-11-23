IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_UpdatePermission]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].usp_OrbioAdmin_UpdatePermission
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_UpdatePermission]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_UpdatePermission]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_UpdatePermission
# File Path:
# CreatedDate: 15-Jul-2015
# Author: roshni
# Description: This stored procedure update permission
# Output Parameter: int
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

CREATE PROCEDURE usp_OrbioAdmin_UpdatePermission
	-- Add the parameters for the stored procedure here
	@id int,
	@roles xml
AS
BEGIN
	delete from dbo.PermissionRecord_Role_Mapping where PermissionRecord_Id=@id
	
	IF(@roles is not null)
						BEGIN
							insert into dbo.PermissionRecord_Role_Mapping(PermissionRecord_Id,CustomerRole_Id) 
							SELECT @id, i.value('.','int') from @roles.nodes('/ArrayOfInt/int') x(i)	
						END
						select @id
END

GO
PRINT 'Created the procedure usp_OrbioAdmin_UpdatePermission'
GO  