 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetFixedTaxRate]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_GetFixedTaxRate]
	PRINT 'Dropped [dbo].[usp_GetFixedTaxRate]'
END	
GO

PRINT 'Creating [dbo].[usp_GetFixedTaxRate]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_GetFixedTaxRate
# File Path:
# CreatedDate: 02-july-2015
# Author: Madhu M B
# Description: This procedure to get fixed tax rates for all given taxcategoryid
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

CREATE PROCEDURE [dbo].[usp_GetFixedTaxRate]
	@taxCategoryIds nvarchar(500)
	
AS
BEGIN
 
 SELECT *, CAST('' as nvarchar(200)) Name INTO #taxCatIds FROM dbo.nop_splitstring_to_table(@taxCategoryIds,',')
 UPDATE #taxCatIds SET  Name = 'Tax.TaxProvider.FixedRate.TaxCategoryId' + data 
 
 SELECT #taxCatIds.data AS Name, Value FROM Setting  INNER JOIN #taxCatIds
 ON Setting.Name = #taxCatIds.Name
 
END


GO
PRINT 'Created the procedure usp_GetFixedTaxRate'
GO  
