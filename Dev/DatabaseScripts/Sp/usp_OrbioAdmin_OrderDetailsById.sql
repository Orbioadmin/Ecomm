IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_OrderDetailsById]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrbioAdmin_OrderDetailsById]
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_OrderDetailsById]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_OrderDetailsById]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_OrderDetailsById
# File Path:
# CreatedDate: 16-Aug-2015
# Author: Sankar T S
# Description: This stored procedure return order detail by id
# Output Parameter: none
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

CREATE PROCEDURE [dbo].[usp_OrbioAdmin_OrderDetailsById]
	@Id int
AS
BEGIN

--Select ord.* from [Order] as ord
--	inner join [Address] as adr on ord.ShippingAddressId = adr.Id
--	where ord.Deleted <> 1 and ord.Id = @Id
declare @customerId int = (select CustomerId from [dbo].[Order] where Id = @Id)
	 DECLARE @XmlResult xml;
-- get order
--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Admin.Orders' AS ns)
SELECT @XmlResult = (select Id as 'OrderId',OrderGuid,StoreId,CustomerId,BillingAddressId,
		ShippingAddressId, OrderStatusId, ShippingStatusId, PaymentStatusId,
		PaymentMethodSystemName, CurrencyRate,CustomerTaxDisplayTypeId,
		 OrderSubtotalInclTax,OrderSubtotalExclTax,OrderSubTotalDiscountInclTax,
		 OrderSubTotalDiscountExclTax,OrderShippingInclTax, OrderShippingExclTax,
		 PaymentMethodAdditionalFeeInclTax,PaymentMethodAdditionalFeeExclTax,
		 TaxRates,OrderTax, OrderDiscount, OrderTotal,RefundedAmount, RewardPointsWereAdded,
		 CustomerLanguageId,AffiliateId, CustomerIp, AllowStoringCreditCardNumber,
		 Deleted, CreatedOnUtc,[dbo].[ufn_GetAdminOrderProductDetails](Id),[dbo].[ufn_GetAdminOrderNoteDetails](@Id),[dbo].[ufn_GetAdminCustomerDetail](Id),
-- Get Billing Address
(select addr.Id as 'BillingAddress_Id',addr.FirstName,addr.LastName,addr.Email,addr.Company,addr.Address1,addr.Address2,
addr.ZipPostalCode,addr.PhoneNumber,addr.FaxNumber,con.Name 'Country' ,sta.Name 'States' from dbo.Customer cus inner join dbo.Address addr 
on cus.BillingAddress_Id = addr.Id inner join dbo.Country con on addr.CountryId = con.Id
inner join dbo.StateProvince sta on addr.StateProvinceId = sta.Id where cus.Id = @customerId for xml path('BillingAddress'),type),
-- Get shipping Address
(select addr.Id as 'ShippingAddress_Id',addr.FirstName,addr.LastName,addr.Email,addr.Company,addr.Address1,addr.Address2,
addr.ZipPostalCode,addr.PhoneNumber,addr.FaxNumber,con.Name 'Country' ,sta.Name 'States' from dbo.Customer cus inner join dbo.Address addr 
on cus.ShippingAddress_Id = addr.Id inner join dbo.Country con on addr.CountryId = con.Id
inner join dbo.StateProvince sta on addr.StateProvinceId = sta.Id where cus.Id = @customerId for xml path('ShippingAddress'),type) 
from [dbo].[Order] where Id = @Id 
for XML path('Order'))

SELECT @XmlResult as XmlResult	
	
END

--exec usp_OrbioAdmin_OrderDetailsById 56
GO
PRINT 'Created the procedure usp_OrbioAdmin_OrderDetailsById'
GO  