IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Cart_Reset]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Cart_Reset]
	PRINT 'Dropped [dbo].[usp_Cart_Reset]'
END	
GO

PRINT 'Creating [dbo].[usp_Cart_Reset]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Cart_Reset
# File Path:
# CreatedDate: 15-Jul-2015
# Author: Madhu M B
# Description: This stored procedure resets cart and any generic attribute
# Output Parameter: string
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

CREATE PROCEDURE [dbo].[usp_Cart_Reset] (@customerId int, @storeId int, @orderItemsList xml)
AS    
BEGIN    
  
   SELECT O.D.value('(ProductId)[1]','int') as ProductId 
   INTO #prod
  FROM @orderItemsList.nodes('/OrderItems/OrderItem') O(d)
  
  DELETE ShoppingCartItem FROM ShoppingCartItem 
  INNER JOIN #prod ON ShoppingCartItem.ProductId= #prod.ProductId
  AND CustomerId = @customerId --AND StoreId = @storeId  --storeid not stored in shoppingcart
  AND ShoppingCartTypeId = 1
  
  DELETE FROM GenericAttribute  WHERE KeyGroup='Customer' 
  and StoreId = @storeId AND [Key]='DiscountCouponCode'
  
  DELETE FROM TransientCart  WHERE CustomerId = @customerId
   
END

GO
PRINT 'Created the procedure usp_Cart_Reset'
GO  