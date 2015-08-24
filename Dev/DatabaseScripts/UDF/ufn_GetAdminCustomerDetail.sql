/*
 ===================================================================================================================================================
 Author: Sankar T S
 Create date: 19 aug 2015
 Description: This function will return the customer details by order id
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAdminCustomerDetail]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAdminCustomerDetail]
	PRINT 'Dropped UDF [dbo].[ufn_GetAdminCustomerDetail]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAdminCustomerDetail]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufn_GetAdminCustomerDetail](@orderId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.OrderNote' AS ns)
		SELECT @xmlResult = (select cust.FirstName,cust.LastName,cust.Email from dbo.Customer cust
		inner join [dbo].[Order] ord on cust.Id = ord.CustomerId
		where ord.Id = @orderId for xml path('Customer'),type)
		    
	 Return @xmlresult

END
GO
PRINT 'Created UDF [dbo].[ufn_GetAdminCustomerDetail]`'
GO  