IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Product_UpdateProduct]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Product_UpdateProduct]
	PRINT 'Dropped [dbo].[usp_Product_UpdateProduct]'
END	
GO

PRINT 'Creating [dbo].[usp_Product_UpdateProduct]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Product_UpdateProduct
# File Path:
# CreatedDate: 09-OCT-2015
# Author: Sankar
# Description: This stored procedure update our product
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
CREATE PROCEDURE [dbo].[usp_Product_UpdateProduct] (
@productXml xml)
AS    
BEGIN
DECLARE @productId INT
declare @seName nvarchar(400) 
SELECT @productId = d.value('(Id)[1]','int' )
from @productXml.nodes('/ProductDetail/product') O(d)
SELECT @seName = O.D.value('(seName)[1]','nvarchar(400)' )
from  @productXml.nodes('/ProductDetail') O(D)
BEGIN TRY
	BEGIN TRANSACTION
	
	--Insert a new product
	
	select d.value('(Id)[1]','int' ) as Id,
	   d.value('(ProductTypeId)[1]','int' ) as ProductTypeId, 
	   d.value('(ParentGroupedProductId)[1]','int' ) as ParentGroupedProductId,
	   d.value('(VisibleIndividually)[1]','bit' )as VisibleIndividually,
	   d.value('(Name)[1]','nvarchar(400)' ) as Name,
	   d.value('(ShortDescription)[1]','nvarchar(max)' ) as ShortDescription, 
	   d.value('(FullDescription)[1]','nvarchar(max)' ) as FullDescription,
		d.value('(AdminComment)[1]','nvarchar(max)' ) as AdminComment,
		  d.value('(VendorId)[1]','int' ) as VendorId,
		  d.value('(ShowOnHomePage)[1]','bit' ) as ShowOnHomePage,
		   d.value('(MetaKeywords)[1]','nvarchar(400)' ) as MetaKeywords,
		  d.value('(MetaDescription)[1]','nvarchar(max)' )as MetaDescription,
		   d.value('(MetaTitle)[1]','nvarchar(400)' ) as MetaTitle,
			d.value('(AllowCustomerReviews)[1]','bit' ) as AllowCustomerReviews,
			d.value('(ApprovedRatingSum)[1]','int' ) as ApprovedRatingSum, 
			 d.value('(NotApprovedRatingSum)[1]','int' ) as NotApprovedRatingSum,
			 d.value('(ApprovedTotalReviews)[1]','int' ) as ApprovedTotalReviews,
	   d.value('(SubjectToAcl)[1]','bit' ) as SubjectToAcl,
	   d.value('(LimitedToStores)[1]','bit' ) as LimitedToStores,
	   d.value('(Sku)[1]','nvarchar(400)' ) as Sku, 
	   d.value('(ManufacturerPartNumber)[1]','nvarchar(400)' ) as ManufacturerPartNumber,
		d.value('(Gtin)[1]','nvarchar(400)' ) as Gtin,
		 d.value('(IsGiftCard)[1]','bit' ) as IsGiftCard,
		  d.value('(GiftCardTypeId)[1]','int' ) as GiftCardTypeId,
		  d.value('(IsShipEnabled)[1]','bit' ) as IsShipEnabled,
		   d.value('(IsFreeShipping)[1]','bit' ) as IsFreeShipping,
			d.value('(AdditionalShippingCharge)[1]','decimal(18,4)' ) as AdditionalShippingCharge,
			d.value('(DeliveryDateId)[1]','int' ) as DeliveryDateId, 
			 d.value('(WarehouseId)[1]','int' ) as WarehouseId,
			 d.value('(IsTaxExempt)[1]','bit' ) as IsTaxExempt,
		   d.value('(TaxCategoryId)[1]','int' ) as TaxCategoryId,
		   d.value('(ManageInventoryMethodId)[1]','int' ) as ManageInventoryMethodId,
		   d.value('(StockQuantity)[1]','int' ) as StockQuantity, 
		   d.value('(DisplayStockAvailability)[1]','bit' ) as DisplayStockAvailability,
			d.value('(DisplayStockQuantity)[1]','bit' ) as DisplayStockQuantity,
			 d.value('(MinStockQuantity)[1]','int' ) as MinStockQuantity,
			  d.value('(LowStockActivityId)[1]','int' ) as LowStockActivityId,
			  d.value('(NotifyAdminForQuantityBelow)[1]','int' ) as NotifyAdminForQuantityBelow,
			   d.value('(BackorderModeId)[1]','int' ) as BackorderModeId,
			  d.value('(AllowBackInStockSubscriptions)[1]','bit' ) as AllowBackInStockSubscriptions,
			   d.value('(OrderMinimumQuantity)[1]','int' ) as OrderMinimumQuantity,
				d.value('(OrderMaximumQuantity)[1]','int' ) as OrderMaximumQuantity,
				d.value('(AllowedQuantities)[1]','nvarchar(1000)' ) as AllowedQuantities,
			 d.value('(AvailableForPreOrder)[1]','bit' ) as AvailableForPreOrder,
			 d.value('(PreOrderAvailabilityStartDateTimeUtc)[1]','DATETIME' ) as PreOrderAvailabilityStartDateTimeUtc,
			   d.value('(Price)[1]','decimal(18,4)' ) as Price,
			   d.value('(OldPrice)[1]','decimal(18,4)' ) as OldPrice, 
			   d.value('(ProductCost)[1]','decimal(18,4)' ) as ProductCost,
				d.value('(SpecialPrice)[1]','decimal(18,4)' ) as SpecialPrice,
				 d.value('(SpecialPriceStartDateTimeUtc)[1]','DATETIME' ) as SpecialPriceStartDateTimeUtc,
				  d.value('(SpecialPriceEndDateTimeUtc)[1]','DATETIME' ) as SpecialPriceEndDateTimeUtc,
				  d.value('(Weight)[1]','decimal(18,4)' ) as [Weight],
				   d.value('(Length)[1]','decimal(18,4)' ) as [Length],
				  d.value('(Width)[1]','decimal(18,4)' ) as Width,
				   d.value('(Height)[1]','decimal(18,4)' ) as Height,
					d.value('(AvailableStartDateTimeUtc)[1]','DATETIME' ) as AvailableStartDateTimeUtc,
					d.value('(AvailableEndDateTimeUtc)[1]','DATETIME' ) as AvailableEndDateTimeUtc, 
					 d.value('(DisplayOrder)[1]','int' ) as DisplayOrder,
					 d.value('(Published)[1]','bit' ) as Published,
					 d.value('(IsGift)[1]','bit' ) as IsGift,
					 d.value('(GiftCharge)[1]','decimal(18,4)' ) as GiftCharge,
					  d.value('(ProductUnit)[1]','decimal(18,4)' ) as ProductUnit
					  Into #tempProductDetail
		from @productXml.nodes('/ProductDetail/product') O(d)
	
	
		
		Update p set p.[ProductTypeId] = toproduct.ProductTypeId,p.[ParentGroupedProductId] = toproduct.ParentGroupedProductId,p.[VisibleIndividually] = toproduct.VisibleIndividually
,p.[Name] = toproduct.Name,p.[ShortDescription]=toproduct.ShortDescription,p.[FullDescription]=toproduct.FullDescription,p.[AdminComment]=toproduct.AdminComment,p.[VendorId]=toproduct.VendorId
,p.[ShowOnHomePage] = toproduct.ShowOnHomePage,p.[MetaKeywords] = toproduct.MetaKeywords
,p.[MetaDescription]=toproduct.MetaDescription,p.[MetaTitle]=toproduct.MetaTitle,p.[AllowCustomerReviews]=toproduct.AllowCustomerReviews
,p.[ApprovedRatingSum]=toproduct.ApprovedRatingSum,p.[NotApprovedRatingSum]=toproduct.NotApprovedRatingSum,p.[ApprovedTotalReviews]=toproduct.ApprovedTotalReviews
,p.[SubjectToAcl]=toproduct.SubjectToAcl,p.[LimitedToStores]=toproduct.LimitedToStores,p.[Sku]=toproduct.Sku
,p.[ManufacturerPartNumber]=toproduct.ManufacturerPartNumber,p.[Gtin]=toproduct.Gtin,p.[IsGiftCard]=toproduct.IsGiftCard
,p.[GiftCardTypeId]=toproduct.GiftCardTypeId
,p.[IsShipEnabled] = toproduct.IsShipEnabled,p.[IsFreeShipping]=toproduct.IsFreeShipping,p.[AdditionalShippingCharge]=toproduct.AdditionalShippingCharge
,p.[DeliveryDateId]=toproduct.DeliveryDateId,p.[WarehouseId]=toproduct.WarehouseId,p.[IsTaxExempt]=toproduct.IsTaxExempt,p.[TaxCategoryId]=toproduct.TaxCategoryId
,p.[ManageInventoryMethodId]=toproduct.ManageInventoryMethodId,p.[StockQuantity]=toproduct.StockQuantity,p.[DisplayStockAvailability]=toproduct.DisplayStockAvailability
,p.[DisplayStockQuantity]=toproduct.DisplayStockQuantity,p.[MinStockQuantity]=toproduct.MinStockQuantity,p.[LowStockActivityId]=toproduct.LowStockActivityId,p.[NotifyAdminForQuantityBelow]=toproduct.NotifyAdminForQuantityBelow,p.[BackorderModeId] = toproduct.BackorderModeId
,p.[AllowBackInStockSubscriptions]=toproduct.AllowBackInStockSubscriptions,p.[OrderMinimumQuantity]=toproduct.OrderMinimumQuantity,p.[OrderMaximumQuantity]=toproduct.OrderMaximumQuantity,p.[AllowedQuantities]=toproduct.AllowedQuantities
,p.[AvailableForPreOrder]=toproduct.AvailableForPreOrder,p.[PreOrderAvailabilityStartDateTimeUtc]=toproduct.PreOrderAvailabilityStartDateTimeUtc,p.[Price]=toproduct.Price
,p.[OldPrice]=toproduct.OldPrice,p.[ProductCost]=toproduct.ProductCost,p.[SpecialPrice]=toproduct.SpecialPrice,p.[SpecialPriceStartDateTimeUtc]=toproduct.SpecialPriceStartDateTimeUtc,p.[SpecialPriceEndDateTimeUtc]=toproduct.SpecialPriceEndDateTimeUtc
,p.[Weight] = toproduct.[Weight],p.[Length]=toproduct.[Length]
,p.[Width]=toproduct.Width,p.[Height]=toproduct.Height,p.[AvailableStartDateTimeUtc]=toproduct.AvailableStartDateTimeUtc
,p.[AvailableEndDateTimeUtc]=toproduct.AvailableEndDateTimeUtc,p.[DisplayOrder]=toproduct.DisplayOrder,p.[Published]=toproduct.Published
,p.[ProductUnit]=toproduct.ProductUnit,p.IsGift=toproduct.IsGift,p.GiftCharge=toproduct.GiftCharge
	from [Product] p
		 inner join #tempProductDetail toproduct on
				p.Id = toproduct.Id
				
	-- Insert slug to url record	
	  Update [UrlRecord] set Slug=@seName where EntityId = @productId

	  --insert Product Tags
	  delete from dbo.Product_ProductTag_Mapping where Product_Id = @productId
	  insert into dbo.Product_ProductTag_Mapping(Product_Id,ProductTag_Id)
	  select @productId, O.D.value('.','int' )
			 from @productXml.nodes('/ProductDetail/productTags/int') O(D)
			 
		
	  -- insert to Product Category	Mapping
	 delete from dbo.Product_Category_Mapping where ProductId = @productId
	insert into dbo.Product_Category_Mapping(ProductId,CategoryId,IsFeaturedProduct,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,0 -- IsFeaturedProduct
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/catgoryIds/int') O(D)

	-- insert to Product Manufacturer Mapping
	 delete from dbo.Product_Manufacturer_Mapping where ProductId = @productId
	insert into dbo.Product_Manufacturer_Mapping(ProductId,ManufacturerId,IsFeaturedProduct,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,0 -- IsFeaturedProduct
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/manufactureIds/int') O(D)
	
	-- insert to Related Products
	 delete from dbo.RelatedProduct where ProductId1 = @productId
	insert into dbo.RelatedProduct(ProductId1,ProductId2,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/relatedProductIds/int') O(D)
	
	--insert to similar Products
	delete from dbo.SimilarProduct where ProductId1 = @productId
	  insert into [dbo].[SimilarProduct](ProductId1,ProductId2,DisplayOrder)
	select @productId, O.D.value('.','int' )
	,row_number() over (order by @productId)		 
	from @productXml.nodes('/ProductDetail/similarProductIds/int') O(D)
	
	--insert to Product discount
	delete from dbo.Discount_AppliedToProducts where Product_Id = @productId
	 insert into [dbo].[Discount_AppliedToProducts](Product_Id,Discount_Id)
	select @productId, O.D.value('.','int' )		 
	from @productXml.nodes('/ProductDetail/discountIds/int') O(D)
	
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
PRINT 'Created the procedure usp_Product_UpdateProduct'
GO  


