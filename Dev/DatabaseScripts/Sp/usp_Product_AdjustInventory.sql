IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Product_AdjustInventory]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Product_AdjustInventory]
	PRINT 'Dropped [dbo].[usp_Product_AdjustInventory]'
END	
GO

PRINT 'Creating [dbo].[usp_Product_AdjustInventory]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Product_AdjustInventory
# File Path:
# CreatedDate: 15-Jul-2015
# Author: Madhu M B
# Description: This stored procedure adjusts product inventory
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

CREATE PROCEDURE [dbo].[usp_Product_AdjustInventory] (
@productListXml xml)
AS    
BEGIN    
  
 SELECT O.D.value('(ProductId)[1]','int') as ProductId,
  O.D.value('(Quantity)[1]','int') as Quantity,
  O.D.value('(AttributesXml)[1]', 'nvarchar(max)') as AttributesXml
   INTO #prod
  FROM @productListXml.nodes('/OrderItems/OrderItem') O(d)
    
  
  --ManageInventoryMethod.DontManageStock ==0
   DELETE #prod from #prod  
   INNER JOIN Product po on #prod.ProductId = po.Id
   where po.ManageInventoryMethodId = 0  
   --ManageInventoryMethod.ManageStock ==1
   
   UPDATE Product SET StockQuantity = StockQuantity - Quantity ,
    Product.DisableBuyButton = CASE WHEN Product.MinStockQuantity>=  StockQuantity - Quantity THEN
						CASE WHEN Product.LowStockActivityId = 1 --DISABLEBUTTON
						THEN 1 
						ELSE Product.DisableBuyButton END
						ELSE
							Product.DisableBuyButton
						END,
    Product.DisableWishlistButton = CASE WHEN Product.MinStockQuantity>=  StockQuantity - Quantity THEN
						CASE WHEN Product.LowStockActivityId = 1 --DISABLEBUTTON
						THEN 1 
						ELSE Product.DisableWishlistButton END
						ELSE
							Product.DisableWishlistButton
						END,
	Product.Published = CASE WHEN Product.MinStockQuantity>=  StockQuantity - Quantity THEN
						CASE WHEN Product.LowStockActivityId = 2 --UNPUBLISH
						THEN 0 
						ELSE Product.Published END
						ELSE
						Product.Published
						END
   FROM #prod WHERE Product.Id = #prod.ProductId 
   AND Product.ManageInventoryMethodId =1 
   
   --ManageInventoryMethod.ManageStockByAttributes
   UPDATE ProductVariantAttributeCombination SET StockQuantity = ProductVariantAttributeCombination.StockQuantity - Quantity
   FROM #prod  INNER JOIN  Product P ON #PROD.ProductId = P.Id
    WHERE ProductVariantAttributeCombination.ProductId = #prod.ProductId
   AND ProductVariantAttributeCombination.AttributesXml = #PROD.AttributesXml
   AND P.ManageInventoryMethodId = 2 
 
END

GO
PRINT 'Created the procedure usp_Product_AdjustInventory'
GO  