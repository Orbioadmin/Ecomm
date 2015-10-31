/*
 ===================================================================================================================================================
 Author:  Sankar
 Create date: 19 oct 2015
 Description: This function will return if discount is valid or not
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetCustomerDetailsByDiscountId]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetCustomerDetailsByDiscountId]
	PRINT 'Dropped UDF [dbo].[ufn_GetCustomerDetailsByDiscountId]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetCustomerDetailsByDiscountId]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetCustomerDetailsByDiscountId](@discountId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( select cust.FirstName,cust.LastName,cust.Email from dbo.Customer cust 
		inner join dbo.Discount_AppliedToCustomers dap on dap.Customer_Id = cust.Id
		where dap.Discount_Id = @discountId and cust.Deleted <> 1


 FOR XML PATH('Customer'), ROOT('Customers'), TYPE )
		    
		 
	 Return @xmlresult

END


GO
PRINT 'Created the UDF ufn_GetCustomerDetailsByDiscountId'
GO  