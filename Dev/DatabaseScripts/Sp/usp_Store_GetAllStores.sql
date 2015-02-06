IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Store_GetAllStores]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Store_GetAllStores]
	PRINT 'Dropped [dbo].[usp_Store_GetAllStores]'
END	
GO

PRINT 'Creating [dbo].[usp_Store_GetAllStores]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Store_GetAllStores
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

CREATE PROCEDURE [dbo].[usp_Store_GetAllStores] (@host varchar(1000)) 
 
AS  
BEGIN  
 
  SELECT Id,Name, Url,SslEnabled,SecureUrl,Hosts, DisplayOrder FROM Store
  ORDER BY DisplayOrder, Id

END
GO
PRINT 'Created the procedure usp_Store_GetAllStores'
GO  


