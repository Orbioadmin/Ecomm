 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Catalog_GetCustomerReviews]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Catalog_GetCustomerReviews]
	PRINT 'Dropped [dbo].[usp_Catalog_GetCustomerReviews]'
END	
GO

PRINT 'Creating [dbo].[usp_Catalog_GetCustomerReviews]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Catalog_GetCustomerReviews
# File Path:
# CreatedDate: 16-march-2015
# Author: Roshni
# Description: This stored procedure gets all the customer review by product id
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

Create PROCEDURE [dbo].[usp_Catalog_GetCustomerReviews] 
	-- Add the parameters for the stored procedure here
	@ProductId int
AS
BEGIN
declare @Result table(ReviewTitle varchar(50),ReviewText varchar(50),Rating int,CustomerName varchar(50))

   
  insert @Result select Title,ReviewText,Rating,CustomerName from ProductReview
	where IsApproved='True' and ProductId=@ProductId order by Rating desc 
	

	select * from @Result
END

GO
PRINT 'Created the procedure usp_Catalog_GetCustomerReviews'
GO  
