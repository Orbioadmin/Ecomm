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

CREATE PROCEDURE [dbo].[usp_Catalog_GetCustomerReviews] 
	-- Add the parameters for the stored procedure here
	@Value varchar(10),
	@ProductId int,
	@pageNumber int, 
	@pageSize int
AS
BEGIN
	if(@Value='Review')
		BEGIN
			declare @Result table(RowNum int ,ReviewTitle varchar(max),ReviewText varchar(max),Rating int,CustomerName varchar(max),OneStarRating int, TwoStarRating int, ThreeStarRating int,FourStarRating int,FiveStarRating int,StarCount int)

			insert @Result select ROW_NUMBER() OVER(ORDER BY Rating) as RowNum,Title,ReviewText,Rating,CustomerName,0,0,0,0,0,0 from ProductReview
			where IsApproved='True' and ProductId=@ProductId order by Rating desc 
			
			update @Result set OneStarRating=(select count(Rating) from @Result where Rating=1),
			TwoStarRating=(select count(Rating) from @Result where Rating=2),
			ThreeStarRating=(select count(Rating) from @Result where Rating=3),
			FourStarRating=(select count(Rating) from @Result where Rating=4),
			FiveStarRating=(select count(Rating) from @Result where Rating=5),
			StarCount=(select count(Rating) from @Result)
		
			select * from @Result where RowNum BETWEEN ((@pageNumber - 1) * @pageSize + 1) AND (@pageNumber * @pageSize)
		END
		
	ELSE IF(@Value='Rating')
		BEGIN
			declare @Results table(OneStarRating int, TwoStarRating int, ThreeStarRating int,FourStarRating int,FiveStarRating int,StarCount int)

			insert @Results select 0,0,0,0,0,0 
			
			update @Results set OneStarRating=(select count(Rating) from ProductReview
			where IsApproved='True' and ProductId=@ProductId and Rating=1),
			TwoStarRating=(select count(Rating) from ProductReview
			where IsApproved='True' and ProductId=@ProductId and Rating=2),
			ThreeStarRating=(select count(Rating) from ProductReview
			where IsApproved='True' and ProductId=@ProductId and Rating=3),
			FourStarRating=(select count(Rating) from ProductReview
			where IsApproved='True' and ProductId=@ProductId and Rating=4),
			FiveStarRating=(select count(Rating) from ProductReview
			where IsApproved='True' and ProductId=@ProductId and Rating=5),
			StarCount=(select count(Rating) from ProductReview
			where IsApproved='True' and ProductId=@ProductId)
	
			select * from @Results
		END
END


GO
PRINT 'Created the procedure usp_Catalog_GetCustomerReviews'
GO  
