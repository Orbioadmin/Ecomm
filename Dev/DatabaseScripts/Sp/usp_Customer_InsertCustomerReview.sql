IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_InsertCustomerReview]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_InsertCustomerReview]
	PRINT 'Dropped [dbo].[usp_Customer_InsertCustomerReview]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_InsertCustomerReview]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_InsertCustomerReview
# File Path:
# CreatedDate: 12.03.2015
# Author: Roshni
# Description: This stored procedure insert customer review
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
Create PROCEDURE [dbo].[usp_Customer_InsertCustomerReview]
	-- Add the parameters for the stored procedure here
	@customerid int,
	@productid int,
	@isapproved bit,
	@reviewtitle varchar(MAX)=null,
	@reviewtext varchar(MAX)=null,
	@rating int,
	@name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into dbo.ProductReview (CustomerId,ProductId,IsApproved,Title,ReviewText,Rating,HelpfulYesTotal,HelpfulNoTotal,CreatedOnUtc,CustomerName)
	values(@customerid,@productid,@isapproved,@reviewtitle,@reviewtext,@rating,0,0,GETUTCDATE(),@name)
	return scope_identity()
END

GO
PRINT 'Created the procedure usp_Customer_InsertCustomerReview'
GO  


