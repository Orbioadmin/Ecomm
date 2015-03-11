 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Shoppingcart_Items_Updates]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Shoppingcart_Items_Updates]
	PRINT 'Dropped [dbo].[usp_Shoppingcart_Items_Updates]'
END	
GO

PRINT 'Creating [dbo].[usp_Shoppingcart_Items_Updates]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Shoppingcart_Items_Updates
# File Path:
# CreatedDate: 11-march-2015
# Author: Sankar T S
# Description: This procedure to use update and delete shopping cart items.
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

Create PROCEDURE [dbo].[usp_Shoppingcart_Items_Updates]
	@list ShoppingCartItem READONLY
AS
BEGIN

select * into #temptable from @list

declare @i int =1
declare @totalrowupdata int = (SELECT COUNT([CartId]) FROM #temptable)
WHILE @i <= @totalrowupdata BEGIN

	declare @cartid int
	
set @cartid = (SELECT [CartId]
		FROM 
			(
			SELECT ROW_NUMBER() OVER(ORDER BY  getdate() DESC) AS RowNumber, 
			[CartId] FROM #temptable 
			)#temptable
			WHERE #temptable.RowNumber = @i
			)

	declare @remove varchar(10)
	
set @remove = (SELECT [Remove]
		FROM 
			(
			SELECT ROW_NUMBER() OVER(ORDER BY  getdate() DESC) AS RowNumber, 
			[Remove] FROM #temptable
			)#temptable
			WHERE #temptable.RowNumber = @i
			)

	declare @quantity varchar(20)
	
set @quantity = (SELECT [Quantity]
		FROM 
			(
			SELECT ROW_NUMBER() OVER(ORDER BY  getdate() DESC) AS RowNumber, 
			[Quantity] FROM #temptable
			)#temptable
			WHERE #temptable.RowNumber = @i
			)

if(@remove = '1')
begin
	delete from [dbo].[ShoppingCartItem] where Id = @cartid
end

else
begin
	update [dbo].[ShoppingCartItem] set Quantity = convert(int,@quantity) where Id = @cartid
end
set @i=@i+1
END

END



GO
PRINT 'Created the procedure usp_Shoppingcart_Items_Updates'
GO  
