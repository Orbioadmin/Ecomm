/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 29 SEP 2015
 Description: This function will return the products by discount id
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetproductdetailsByDiscountId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetproductdetailsByDiscountId]
	PRINT 'Dropped UDF [dbo].[ufn_GetproductdetailsByDiscountId]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetproductdetailsByDiscountId]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetproductdetailsByDiscountId](@discountId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( select product.Id Id,product.Name Name,product.ShortDescription shortdescription,product.OldPrice OldPrice,product.Price price,
product.MetaKeywords Metakeywords,product.MetaDescription Metadescription,product.OrderMinimumQuantity Orderminimumquantity,product.OrderMaximumQuantity ordermaximumquantity,
product.AvailableStartDateTimeUtc startdate,product.AvailableEndDateTimeUtc enddate,product.SpecialPrice specialprice,product.SpecialPriceStartDateTimeUtc specialpricestartdate,product.SpecialPriceEndDateTimeUtc specialpriceenddate,
product.DisplayStockAvailability displaystockavailability,product.DisableBuyButton disablebuybutton,product.DisableWishlistButton disablewishlistbtn
from [dbo].[Product] product inner join dbo.Discount_AppliedToProducts dap on dap.Product_Id = product.Id
where dap.Discount_Id = @discountId and product.Deleted <> 1


 FOR XML PATH('Product'), ROOT('Products'), TYPE )
		    
		 
	 Return @xmlresult

END

GO
PRINT 'Created UDF [dbo].[ufn_GetproductdetailsByDiscountId]`'
GO  