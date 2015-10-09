IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Product_InsertProduct]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Product_InsertProduct]
	PRINT 'Dropped [dbo].[usp_Product_InsertProduct]'
END	
GO

PRINT 'Creating [dbo].[usp_Product_InsertProduct]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Product_InsertProduct
# File Path:
# CreatedDate: 14-SEP-2015
# Author: Sankar
# Description: This stored procedure insert a new product
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
CREATE PROCEDURE [dbo].[usp_Product_InsertProduct] (
@productXml xml)
AS    
BEGIN
DECLARE @productId INT
BEGIN TRY
	BEGIN TRANSACTION
	
	--Insert a new product
		
		INSERT INTO [Product]([ProductTypeId],[ParentGroupedProductId],[VisibleIndividually]
,[Name],[ShortDescription],[FullDescription],[AdminComment],[ProductTemplateId],[VendorId],[ShowOnHomePage],[MetaKeywords]
,[MetaDescription],[MetaTitle],[AllowCustomerReviews],[ApprovedRatingSum],[NotApprovedRatingSum],[ApprovedTotalReviews]
,[NotApprovedTotalReviews],[SubjectToAcl],[LimitedToStores],[Sku],[ManufacturerPartNumber],[Gtin],[IsGiftCard]
,[GiftCardTypeId],[RequireOtherProducts],[RequiredProductIds],[AutomaticallyAddRequiredProducts],[IsDownload]
,[DownloadId],[UnlimitedDownloads],[MaxNumberOfDownloads],[DownloadExpirationDays],[DownloadActivationTypeId]
,[HasSampleDownload],[SampleDownloadId],[HasUserAgreement],[UserAgreementText],[IsRecurring],[RecurringCycleLength]
,[RecurringCyclePeriodId],[RecurringTotalCycles],[IsShipEnabled],[IsFreeShipping],[AdditionalShippingCharge]
,[DeliveryDateId],[WarehouseId],[IsTaxExempt],[TaxCategoryId],[ManageInventoryMethodId],[StockQuantity],[DisplayStockAvailability]
,[DisplayStockQuantity],[MinStockQuantity],[LowStockActivityId],[NotifyAdminForQuantityBelow],[BackorderModeId]
,[AllowBackInStockSubscriptions],[OrderMinimumQuantity],[OrderMaximumQuantity],[AllowedQuantities],[DisableBuyButton]
,[DisableWishlistButton],[AvailableForPreOrder],[PreOrderAvailabilityStartDateTimeUtc],[CallForPrice],[Price]
,[OldPrice],[ProductCost],[SpecialPrice],[SpecialPriceStartDateTimeUtc],[SpecialPriceEndDateTimeUtc],[CustomerEntersPrice]
,[MinimumCustomerEnteredPrice],[MaximumCustomerEnteredPrice],[HasTierPrices],[HasDiscountsApplied],[Weight],[Length]
,[Width],[Height],[AvailableStartDateTimeUtc],[AvailableEndDateTimeUtc],[DisplayOrder],[Published],[Deleted]
,[CreatedOnUtc],[UpdatedOnUtc],[ProductUnit])
	select d.value('(ProductTypeId)[1]','int' ), 
	   d.value('(ParentGroupedProductId)[1]','int' ),
	   d.value('(VisibleIndividually)[1]','bit' ),
	   d.value('(Name)[1]','nvarchar(400)' ),
	   d.value('(ShortDescription)[1]','nvarchar(max)' ), 
	   d.value('(FullDescription)[1]','nvarchar(max)' ),
		d.value('(AdminComment)[1]','nvarchar(max)' ),
		 d.value('(ProductTemplateId)[1]','int' ),
		  d.value('(VendorId)[1]','int' ),
		  d.value('(ShowOnHomePage)[1]','bit' ) ,
		   d.value('(MetaKeywords)[1]','nvarchar(400)' ),
		  d.value('(MetaDescription)[1]','nvarchar(max)' ),
		   d.value('(MetaTitle)[1]','nvarchar(400)' ),
			d.value('(AllowCustomerReviews)[1]','bit' ),
			d.value('(ApprovedRatingSum)[1]','int' ), 
			 d.value('(NotApprovedRatingSum)[1]','int' ),
			 d.value('(ApprovedTotalReviews)[1]','int' ),
			 0,--NotApprovedTotalReviews
	   d.value('(SubjectToAcl)[1]','bit' ),
	   d.value('(LimitedToStores)[1]','bit' ),
	   d.value('(Sku)[1]','nvarchar(400)' ), 
	   d.value('(ManufacturerPartNumber)[1]','nvarchar(400)' ),
		d.value('(Gtin)[1]','nvarchar(400)' ),
		 d.value('(IsGiftCard)[1]','bit' ),
		  d.value('(GiftCardTypeId)[1]','int' ),
		  d.value('(RequireOtherProducts)[1]','bit' ) ,
		   d.value('(RequiredProductIds)[1]','nvarchar(1000)' ),
		  d.value('(AutomaticallyAddRequiredProducts)[1]','bit' ),
		   d.value('(IsDownload)[1]','bit' ),
			d.value('(DownloadId)[1]','int' ),
			d.value('(UnlimitedDownloads)[1]','bit' ), 
			 d.value('(MaxNumberOfDownloads)[1]','int' ),
			d.value('(DownloadExpirationDays)[1]','int' ),
	   d.value('(DownloadActivationTypeId)[1]','int' ),
	   d.value('(HasSampleDownload)[1]','bit' ),
	   d.value('(SampleDownloadId)[1]','int' ), 
	   d.value('(HasUserAgreement)[1]','bit' ),
		d.value('(UserAgreementText)[1]','nvarchar(max)' ),
		 d.value('(IsRecurring)[1]','bit' ),
		  d.value('(RecurringCycleLength)[1]','int' ),
		  d.value('(RecurringCyclePeriodId)[1]','int' ) ,
		   d.value('(RecurringTotalCycles)[1]','int' ),
		  d.value('(IsShipEnabled)[1]','bit' ),
		   d.value('(IsFreeShipping)[1]','bit' ),
			d.value('(AdditionalShippingCharge)[1]','decimal(18,4)' ),
			d.value('(DeliveryDateId)[1]','int' ), 
			 d.value('(WarehouseId)[1]','int' ),
			 d.value('(IsTaxExempt)[1]','bit' ),
		   d.value('(TaxCategoryId)[1]','int' ),
		   d.value('(ManageInventoryMethodId)[1]','int' ),
		   d.value('(StockQuantity)[1]','int' ), 
		   d.value('(DisplayStockAvailability)[1]','bit' ),
			d.value('(DisplayStockQuantity)[1]','bit' ),
			 d.value('(MinStockQuantity)[1]','int' ),
			  d.value('(LowStockActivityId)[1]','int' ),
			  d.value('(NotifyAdminForQuantityBelow)[1]','int' ) ,
			   d.value('(BackorderModeId)[1]','int' ),
			  d.value('(AllowBackInStockSubscriptions)[1]','bit' ),
			   d.value('(OrderMinimumQuantity)[1]','int' ),
				d.value('(OrderMaximumQuantity)[1]','int' ),
				d.value('(AllowedQuantities)[1]','nvarchar(1000)' ),
			0,--DisableBuyButton
			0,--DisableWishlistButton 
			 d.value('(AvailableForPreOrder)[1]','bit' ),
			 d.value('(PreOrderAvailabilityStartDateTimeUtc)[1]','DATETIME' ),
			   0,--CallForPrice
			   d.value('(Price)[1]','decimal(18,4)' ),
			   d.value('(OldPrice)[1]','decimal(18,4)' ), 
			   d.value('(ProductCost)[1]','decimal(18,4)' ),
				d.value('(SpecialPrice)[1]','decimal(18,4)' ),
				 d.value('(SpecialPriceStartDateTimeUtc)[1]','DATETIME' ),
				  d.value('(SpecialPriceEndDateTimeUtc)[1]','DATETIME' ),
				  0,--CustomerEntersPrice
				  0,--MinimumCustomerEnteredPrice
				  0,--MaximumCustomerEnteredPrice
				  0,--HasTierPrices
				  0,--HasDiscountsApplied
				  d.value('(Weight)[1]','decimal(18,4)' ) ,
				   d.value('(Length)[1]','decimal(18,4)' ),
				  d.value('(Width)[1]','decimal(18,4)' ),
				   d.value('(Height)[1]','decimal(18,4)' ),
					d.value('(AvailableStartDateTimeUtc)[1]','DATETIME' ),
					d.value('(AvailableEndDateTimeUtc)[1]','DATETIME' ), 
					 d.value('(DisplayOrder)[1]','int' ),
					 d.value('(Published)[1]','bit' ),
					 0,--Deleted
					 d.value('(CreatedOnUtc)[1]','DATETIME' ),
					 d.value('(UpdatedOnUtc)[1]','DATETIME' ),
					  d.value('(ProductUnit)[1]','decimal(18,4)' )
		from @productXml.nodes('/ProductDetail/product') O(d)
	    
		SET @productId = SCOPE_IDENTITY()
	
	-- Insert slug to url record	
	  INSERT INTO UrlRecord (EntityId, EntityName, Slug,IsActive, LanguageId)
	  SELECT @productId,
		  'Product',--entity name
		  O.D.value('(seName)[1]','nvarchar(400)' ),
		  1,-- IsActive
		  0--Language Id
		   FROM
	  @productXml.nodes('/ProductDetail') O(D)
	  
	  
	  --insert Product Tags
	  insert into dbo.Product_ProductTag_Mapping(Product_Id,ProductTag_Id)
	  select @productId, O.D.value('.','int' )
			 from @productXml.nodes('/ProductDetail/productTags/int') O(D)
			 
		
	  -- insert to Product Category	Mapping
	insert into dbo.Product_Category_Mapping(ProductId,CategoryId,IsFeaturedProduct,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,0 -- IsFeaturedProduct
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/catgoryIds/int') O(D)

	-- insert to Product Manufacturer Mapping
	insert into dbo.Product_Manufacturer_Mapping(ProductId,ManufacturerId,IsFeaturedProduct,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,0 -- IsFeaturedProduct
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/manufactureIds/int') O(D)
	
		-- insert to Related Products
	insert into dbo.RelatedProduct(ProductId1,ProductId2,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/relatedProductIds/int') O(D)
	
	--insert to similar Products
	  insert into [dbo].[SimilarProduct](ProductId1,ProductId2,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/similarProductIds/int') O(D)
	
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
PRINT 'Created the procedure usp_Product_InsertProduct'
GO  


