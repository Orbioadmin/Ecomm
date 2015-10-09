IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpdateProductSpecificationAttribute]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_UpdateProductSpecificationAttribute]
	PRINT 'Dropped [dbo].[usp_UpdateProductSpecificationAttribute]'
END	
GO

PRINT 'Creating [dbo].[usp_UpdateProductSpecificationAttribute]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_UpdateProductSpecificationAttribute
# File Path:
# CreatedDate: 09-OCT-2015
# Author: Sankar
# Description: This stored procedure update the display order in product specification attribute
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
CREATE PROCEDURE [dbo].[usp_UpdateProductSpecificationAttribute] (
@specificationIdsXml xml)
AS    
BEGIN
BEGIN TRY
	BEGIN TRANSACTION
	
	SELECT row_number() over (order by (SELECT NULL)) As RowNumber,
	C.value('.','int' ) AS [Id]
    INTO #temptable
    FROM @specificationIdsXml.nodes('/ArrayOfInt/int') as T(C)

	declare @i int  = 1
	declare @totalRecord int = (select COUNT(*) from #temptable)
	while(@i <= @totalRecord)
		begin
				update dbo.Product_SpecificationAttribute_Mapping set DisplayOrder = 0
				where Id = (select Id from #temptable where RowNumber = @i)
						
				update dbo.Product_SpecificationAttribute_Mapping set DisplayOrder = @i
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
PRINT 'Created the procedure usp_UpdateProductSpecificationAttribute'
GO  


