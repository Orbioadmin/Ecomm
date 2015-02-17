IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Store_GetCurrentStore]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Store_GetCurrentStore]
	PRINT 'Dropped [dbo].[usp_Store_GetCurrentStore]'
END	
GO

PRINT 'Creating [dbo].[usp_Store_GetCurrentStore]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Store_GetCurrentStore
# File Path:
# CreatedDate: 05-feb-2015
# Author: Madhu MB
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

CREATE PROCEDURE [dbo].[usp_Store_GetCurrentStore] (@host varchar(1000)) 
 
AS  
BEGIN  
 
  
  IF EXISTS(SELECT 1  FROM Store
WHERE @HOST IN (SELECT * FROM nop_splitstring_to_table(sTORE.Hosts,',')))
	BEGIN
		SELECT TOP 1 * FROM Store
		WHERE @HOST IN (SELECT * FROM nop_splitstring_to_table(sTORE.Hosts,','))
		ORDER BY DisplayOrder, Id
	END
  ELSE
  BEGIN
		SELECT TOP 1 * FROM Store ORDER BY DisplayOrder, Id
  END
 

END
GO
PRINT 'Created the procedure usp_Store_GetCurrentStore'
GO  


