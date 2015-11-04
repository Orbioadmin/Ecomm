IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Order_UpdateOrder]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Order_UpdateOrder]
	PRINT 'Dropped [dbo].[usp_Order_UpdateOrder]'
END	
GO

PRINT 'Creating [dbo].[usp_Order_UpdateOrder]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Order_UpdateOrder
# File Path:
# CreatedDate: 17-Aug-2015
# Author: Sankar T S
# Description: This stored procedure update order data
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

CREATE PROCEDURE [dbo].[usp_Order_UpdateOrder] (
@orderXml xml,@updateSucess bit OUTPUT

)
AS    
BEGIN
    
 DECLARE @orderItemXml xml, @customerId INT, @storeId INT,@orderId INT
 SELECT @customerId = d.value('(CustomerId)[1]','int' )
 from @orderXml.nodes('/Order') O(d)
 SELECT @orderItemXml = @orderXml.query('/Order/OrderItems')
BEGIN TRY
	BEGIN TRANSACTION
		--adjust inventory first
		
		
		
		select d.value('(OrderId)[1]','nvarchar(100)' ) as OrderId,
	   d.value('(OrderStatusId)[1]','int' ) as OrderStatusId,
	   d.value('(ShippingStatusId)[1]','int' ) as ShippingStatusId,
	   d.value('(PaymentStatusId)[1]','int' ) as PaymentStatusId, 
	   d.value('(OrderSubtotalInclTax)[1]','decimal(18,4)' ) as OrderSubtotalInclTax,
		d.value('(OrderSubtotalExclTax)[1]','decimal(18,4)' ) as OrderSubtotalExclTax,
		 d.value('(OrderSubTotalDiscountInclTax)[1]','decimal(18,4)' ) as OrderSubTotalDiscountInclTax,
		  d.value('(OrderSubTotalDiscountExclTax)[1]','decimal(18,4)' ) as OrderSubTotalDiscountExclTax,
		  d.value('(OrderShippingInclTax)[1]','decimal(18,4)' ) as OrderShippingInclTax,
		  d.value('(OrderShippingExclTax)[1]','decimal(18,4)' ) as OrderShippingExclTax,
		  d.value('(TaxRates)[1]','nvarchar(100)' ) as TaxRates,
		   d.value('(OrderTax)[1]','decimal(18,4)' ) as OrderTax,
		  d.value('(OrderDiscount)[1]','decimal(18,4)' ) as OrderDiscount,
		   d.value('(ShippingMethod)[1]','nvarchar(100)' ) as ShippingMethod,
		   d.value('(OrderTotal)[1]','decimal(18,4)' ) as OrderTotal,
			 d.value('(AllowStoringCreditCardNumber)[1]','bit' ) as AllowStoringCreditCardNumber,
			 d.value('(CreatedOnUtc)[1]','DATETIME' ) as CreatedOnUtc,
			 d.value('(Deleted)[1]','bit' ) as Deleted --Deleted
			  Into #tempOrderDetail
		from @orderXml.nodes('/Order') O(d)
		
		update ord set ord.OrderStatusId = torder.OrderStatusId, ord.ShippingStatusId = torder.ShippingStatusId, 
		ord.PaymentStatusId = torder.PaymentStatusId,
		 ord.OrderSubtotalInclTax = torder.OrderSubtotalInclTax,ord.OrderSubtotalExclTax = torder.OrderSubtotalExclTax,
		 ord.OrderSubTotalDiscountInclTax = torder.OrderSubTotalDiscountInclTax,
		 ord.OrderSubTotalDiscountExclTax = torder.OrderSubTotalDiscountExclTax,
		 ord.OrderShippingExclTax=torder.OrderShippingExclTax,
		 ord.OrderShippingInclTax=torder.OrderShippingInclTax,
		 ord.TaxRates = torder.TaxRates,ord.OrderTax = torder.OrderTax, ord.OrderDiscount = torder.OrderDiscount, 
		 ord.ShippingMethod = torder.ShippingMethod,ord.OrderTotal = torder.OrderTotal,ord.AllowStoringCreditCardNumber = torder.AllowStoringCreditCardNumber,
		 ord.CreatedOnUtc = torder.CreatedOnUtc,ord.Deleted = torder.Deleted
		 from [Order] ord
		 inner join #tempOrderDetail torder on
				ord.Id = torder.OrderId
				
		--stock updation from admin 
		 EXEC usp_StockQuantity_Reset @storeId, @orderXml		
				
		SELECT O.D.value('(OrderItemGuid)[1]','nvarchar(100)') as OrderItemGuid,
		 (select ProductId from dbo.ufn_GetOrderProductId(@orderXml,O.D.value('(OrderItemGuid)[1]','nvarchar(100)'))) as ProductId,
		  O.D.value('(Quantity)[1]','INT') as Quantity,
		  O.D.value('(UnitPriceInclTax)[1]','decimal(18,4)' ) as UnitPriceInclTax,
		   O.D.value('(UnitPriceExclTax)[1]','decimal(18,4)' ) as UnitPriceExclTax,
		  O.D.value('(PriceInclTax)[1]','decimal(18,4)' ) as PriceInclTax,
		   O.D.value('(PriceExclTax)[1]','decimal(18,4)' ) as PriceExclTax, 
		  O.D.value('(DiscountAmountInclTax)[1]','decimal(18,4)' ) as DiscountAmountInclTax,
		   O.D.value('(DiscountAmountExclTax)[1]','decimal(18,4)' ) as DiscountAmountExclTax,     
		   O.D.value('(OriginalProductCost)[1]','decimal(18,4)' ) as OriginalProductCost,
		   O.D.value('(AttributeDescription)[1]','nvarchar(max)') as AttributeDescription,
		  O.D.value('(AttributesXml)[1]','nvarchar(max)') as AttributesXml,
		  O.D.value('(PriceDetailXml)[1]','nvarchar(max)') as PriceDetailXml
		   into #tempOrderItem
		   FROM 
	  @orderXml.nodes('/Order/OrderItems/OrderItem') O(D)		
		
	  Update ordItem set ordItem.Quantity = tOrderItem.Quantity, ordItem.UnitPriceInclTax = tOrderItem.UnitPriceInclTax, 
	  ordItem.UnitPriceExclTax = tOrderItem.UnitPriceExclTax, ordItem.PriceInclTax = tOrderItem.PriceInclTax,
	   ordItem.PriceExclTax = tOrderItem.PriceExclTax, ordItem.DiscountAmountInclTax = tOrderItem.DiscountAmountInclTax, 
	   ordItem.DiscountAmountExclTax = tOrderItem.DiscountAmountExclTax,
	   ordItem.OriginalProductCost = tOrderItem.OriginalProductCost,
	   ordItem.AttributeDescription = tOrderItem.AttributeDescription,ordItem.AttributesXml = tOrderItem.AttributesXml,
	   ordItem.PriceDetailXml = tOrderItem.PriceDetailXml
		from OrderItem ordItem
		inner join #tempOrderItem tOrderItem
		on ordItem.OrderItemGuid = tOrderItem.OrderItemGuid
		
		--	  INSERT INTO OrderNote(OrderId,Note,DisplayToCustomer,CreatedOnUtc)
  --SELECT O.D.value('(OrderId)[1]','Int'),  O.D.value('(Note)[1]','nvarchar(MAX)'), 
  -- O.D.value('(DisplayToCustomer)[1]','bit'), d.value('(../../CreatedOnUtc)[1]','DATETIME')
  -- FROM
  --@orderXml.nodes('/Order/OrderNotes/OrderNote') O(D)
  
   COMMIT TRAN

		set @updateSucess =1
		RETURN @updateSucess
	 
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
PRINT 'Created the procedure usp_Order_UpdateOrder'
GO  