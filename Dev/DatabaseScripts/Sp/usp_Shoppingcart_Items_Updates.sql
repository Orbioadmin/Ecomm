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

CREATE PROCEDURE [dbo].[usp_Shoppingcart_Items_Updates]
	@list XML
AS
BEGIN
	;WITH XMLNAMESPACES(DEFAULT 'http://schemas.datacontract.org/2004/07/Orbio.Web.UI.Models.Orders','http://schemas.datacontract.org/2004/07/Orbio.Web.UI.Models.Catalog' as sq)
    SELECT C.value('(CartId)[1]','INT') AS [CartId],
	C.value('(sq:SelectedQuantity)[1]','INT') AS [SelectedQuantity],
	C.value('(IsRemove)[1]','bit') AS [IsRemove]
    INTO #temptable
    FROM @list.nodes('/ArrayOfShoppingCartItemModel/ShoppingCartItemModel') as T(C)
	select * from #temptable

	update sc set Quantity = t.SelectedQuantity
		from [dbo].[ShoppingCartItem] sc
			inner join #temptable t on
				sc.Id = t.CartId

				delete sc from [dbo].[ShoppingCartItem] sc
					inner join #temptable t on
					sc.Id = t.CartId where t.IsRemove = 1
END

GO
PRINT 'Created the procedure usp_Shoppingcart_Items_Updates'
GO  
