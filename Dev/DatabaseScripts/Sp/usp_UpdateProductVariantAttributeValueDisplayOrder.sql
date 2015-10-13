IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateProductVariantAttributeValueDisplayOrder]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_UpdateProductVariantAttributeValueDisplayOrder]
	PRINT 'Dropped [dbo].[usp_UpdateProductVariantAttributeValueDisplayOrder]'
END	
GO

PRINT 'Creating [dbo].[usp_UpdateProductVariantAttributeValueDisplayOrder]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_UpdateProductVariantAttributeValueDisplayOrder
# File Path:
# CreatedDate: 13-OCT-2015
# Author: Sankar
# Description: This stored procedure update the display order in product variant attribute value
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
CREATE PROCEDURE [dbo].[usp_UpdateProductVariantAttributeValueDisplayOrder] (
@pvavIdsXml xml)
AS    
BEGIN
BEGIN TRY
	BEGIN TRANSACTION
	
	SELECT row_number() over (order by (SELECT NULL)) As RowNumber,
	C.value('.','int' ) AS [Id]
    INTO #temptable
    FROM @pvavIdsXml.nodes('/ArrayOfInt/int') as T(C)

	declare @i int  = 1
	declare @totalRecord int = (select COUNT(*) from #temptable)
	while(@i <= @totalRecord)
		begin
				update dbo.ProductVariantAttributeValue set DisplayOrder = 0
				where Id = (select Id from #temptable where RowNumber = @i)
						
				update dbo.ProductVariantAttributeValue set DisplayOrder = @i
				where Id = (select Id from #temptable where RowNumber = @i)
			Set @i = @i+1
		end

	
   COMMIT TRAN
	 
 END TRY

 BEGIN CATCH
		ROLLBACK TRANSACTION

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE()
			,@ErrorSeverity = ERROR_SEVERITY()
			,@ErrorState = ERROR_STATE();

		-- Use RAISERROR inside the CATCH block to return error
		-- information about the original error that caused
		-- execution to jump to the CATCH block.
		RAISERROR (
				@ErrorMessage
				,-- Message text.
				@ErrorSeverity
				,-- Severity.
				@ErrorState -- State.
				);
	END CATCH
END




GO
PRINT 'Created the procedure usp_UpdateProductVariantAttributeValueDisplayOrder'
GO  


