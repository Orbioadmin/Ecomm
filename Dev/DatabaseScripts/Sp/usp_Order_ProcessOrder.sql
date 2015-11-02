IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Order_ProcessOrder]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Order_ProcessOrder]
	PRINT 'Dropped [dbo].[usp_Order_ProcessOrder]'
END	
GO

PRINT 'Creating [dbo].[usp_Order_ProcessOrder]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Order_ProcessOrder
# File Path:
# CreatedDate: 14-Jul-2015
# Author: Madhu M B
# Description: This stored procedure process order data
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

CREATE PROCEDURE [dbo].[usp_Order_ProcessOrder] (
@orderXml xml)
AS    
BEGIN
    
 DECLARE @orderItemXml xml, @customerId INT, @storeId INT,@orderId INT
 SELECT @customerId = d.value('(CustomerId)[1]','int' ) ,
	   @storeId = d.value('(StoreId)[1]','int' ) 
 from @orderXml.nodes('/Order') O(d)
 SELECT @orderItemXml = @orderXml.query('/Order/OrderItems')
BEGIN TRY
	BEGIN TRANSACTION
		--adjust inventory first
		
		EXEC [usp_Product_AdjustInventory] @orderItemXml
		
		INSERT INTO [Order](OrderGuid,StoreId,CustomerId,BillingAddressId,
		ShippingAddressId, OrderStatusId, ShippingStatusId, PaymentStatusId,
		PaymentMethodSystemName, CurrencyRate,CustomerTaxDisplayTypeId,
		 OrderSubtotalInclTax,OrderSubtotalExclTax,OrderSubTotalDiscountInclTax,
		 OrderSubTotalDiscountExclTax,OrderShippingInclTax, OrderShippingExclTax,
		 PaymentMethodAdditionalFeeInclTax,PaymentMethodAdditionalFeeExclTax,
		 TaxRates,OrderTax, OrderDiscount, OrderTotal,RefundedAmount, RewardPointsWereAdded,
		 CustomerLanguageId,AffiliateId, CustomerIp, AllowStoringCreditCardNumber,
		 Deleted, CreatedOnUtc
		 )
	select d.value('(OrderGuid)[1]','nvarchar(100)' ), 
	   @storeId ,
	   @customerId as CustomerId,
	   (Select BillingAddress_Id from Customer where Id= d.value('(CustomerId)[1]','int' )),
	   (Select ShippingAddress_Id from Customer where Id= d.value('(CustomerId)[1]','int' )),
	   d.value('(OrderStatusId)[1]','int' ),
	   d.value('(ShippingStatusId)[1]','int' ),
	   d.value('(PaymentStatusId)[1]','int' ),
	   d.value('(PaymentMethodSystemName)[1]','nvarchar(100)' ), 
	   
	   1,  --exchange rate
	   10, --CustomerTaxDisplayTypeId hardcoded,
	   d.value('(OrderSubtotalInclTax)[1]','decimal(18,4)' ),
		d.value('(OrderSubtotalExclTax)[1]','decimal(18,4)' ),
		 d.value('(OrderSubTotalDiscountInclTax)[1]','decimal(18,4)' ),
		  d.value('(OrderSubTotalDiscountExclTax)[1]','decimal(18,4)' ),
		  d.value('(OrderShippingInclTax)[1]','decimal(18,4)' ),
		  d.value('(OrderShippingExclTax)[1]','decimal(18,4)' ),
		  0,--PaymentMethodAdditionalFeeInclTax
		  0,--PaymentMethodAdditionalFeeExclTax
		  d.value('(TaxRates)[1]','nvarchar(100)' ) ,
		   d.value('(OrderTax)[1]','decimal(18,4)' ),
		  d.value('(OrderDiscount)[1]','decimal(18,4)' ),
		   d.value('(OrderTotal)[1]','decimal(18,4)' ),
		   0 ,-- RefundedAmount
		   0,--RewardPointsWereAdded
	    
			d.value('(CustomerLanguageId)[1]','int' ),
			0, --AffiliateId
			d.value('(CustomerIp)[1]','nvarchar(max)' ), 
			 d.value('(AllowStoringCreditCardNumber)[1]','bit' ),
			 0, --Deleted
			  d.value('(CreatedOnUtc)[1]','DATETIME' )
		from @orderXml.nodes('/Order') O(d)
	    
		SET @orderId = SCOPE_IDENTITY()
		
	  INSERT INTO OrderItem (OrderItemGuid, OrderId, ProductId,
	   Quantity, UnitPriceInclTax, UnitPriceExclTax, PriceInclTax,
	   PriceExclTax, DiscountAmountInclTax, DiscountAmountExclTax,
	   OriginalProductCost,AttributeDescription,AttributesXml,DownloadCount,
		IsDownloadActivated,PriceDetailXml)
	  SELECT O.D.value('(OrderItemGuid)[1]','nvarchar(100)'), @orderId,
		  O.D.value('(./Product/Id)[1]','INT'),
		  O.D.value('(Quantity)[1]','INT'),
		  O.D.value('(UnitPriceInclTax)[1]','decimal(18,4)' ),
		   O.D.value('(UnitPriceExclTax)[1]','decimal(18,4)' ),
		  O.D.value('(PriceInclTax)[1]','decimal(18,4)' ),
		   O.D.value('(PriceExclTax)[1]','decimal(18,4)' ), 
		  O.D.value('(DiscountAmountInclTax)[1]','decimal(18,4)' ),
		   O.D.value('(DiscountAmountExclTax)[1]','decimal(18,4)' ),     
		   O.D.value('(OriginalProductCost)[1]','decimal(18,4)' ),
		   O.D.value('(AttributeDescription)[1]','nvarchar(max)'),
		  O.D.value('(AttributesXml)[1]','nvarchar(max)'),
		  0, --DownloadCount
		  0, --IsDownloadActivated,
		  O.D.value('(PriceDetailXml)[1]','nvarchar(max)')
		   FROM
	  @orderXml.nodes('/Order/OrderItems/OrderItem') O(D)
	  
	  INSERT INTO DiscountUsageHistory(DiscountId, OrderId, CreatedOnUtc)
	  SELECT D.value('(DiscountId)[1]','INT') , @orderId,  d.value('(../../CreatedOnUtc)[1]','DATETIME' ) 
	  from
	  @orderXml.nodes('/Order/DiscountUsageHistory/DiscountUsageHistory') O(D)

	  INSERT INTO OrderNote(OrderId,Note,DisplayToCustomer,CreatedOnUtc)
  SELECT @orderId,  O.D.value('(Note)[1]','nvarchar(MAX)'), 
   O.D.value('(DisplayToCustomer)[1]','bit'), d.value('(../../CreatedOnUtc)[1]','DATETIME')
   FROM
  @orderXml.nodes('/Order/OrderNotes/OrderNote') O(D)
  
   COMMIT TRAN
   EXEC usp_Cart_Reset  @customerId, @storeId, @orderItemXml
	 --RETURN @orderId
	 
	  --for stock updation 
   EXEC usp_StockQuantity_Reset @storeId, @orderXml
   
	 	  DECLARE @XmlResult xml;

--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.order' AS ns)
SELECT @XmlResult =(select @orderId as 'OrderId' ,
(select addr.Id,addr.FirstName,addr.LastName,addr.Email,addr.Company,addr.Address1,addr.Address2,
addr.ZipPostalCode,addr.PhoneNumber,addr.FaxNumber,con.Name 'Country' ,sta.Name 'States' from dbo.Customer cus inner join dbo.Address addr 
on cus.BillingAddress_Id = addr.Id inner join dbo.Country con on addr.CountryId = con.Id
inner join dbo.StateProvince sta on addr.StateProvinceId = sta.Id where cus.Id = @customerId for xml path('BillingAddress'),type),

(select addr.Id,addr.FirstName,addr.LastName,addr.Email,addr.Company,addr.Address1,addr.Address2,
addr.ZipPostalCode,addr.PhoneNumber,addr.FaxNumber,con.Name 'Country' ,sta.Name 'States' from dbo.Customer cus inner join dbo.Address addr 
on cus.ShippingAddress_Id = addr.Id inner join dbo.Country con on addr.CountryId = con.Id
inner join dbo.StateProvince sta on addr.StateProvinceId = sta.Id where cus.Id = @customerId for xml path('ShippingAddress'),type),

(Select  [dbo].[ufn_GetOrderProductDetailXmlById](ori.ProductId) from OrderItem ori  where 
ori.OrderId = @orderId  for xml path('Products'),type)

	  for xml path('ProcessOrderResult'))
 SELECT @XmlResult as XmlResult
	 
 END TRY
 --CLEANUP CART
 
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
PRINT 'Created the procedure usp_Order_ProcessOrder'
GO  