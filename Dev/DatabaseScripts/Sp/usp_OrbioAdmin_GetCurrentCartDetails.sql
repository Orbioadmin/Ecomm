IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_GetCurrentCartDetails]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrbioAdmin_GetCurrentCartDetails]
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_GetCurrentCartDetails]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_GetCurrentCartDetails]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_GetCurrentCartDetails
# File Path:
# CreatedDate: 19-Aug-2015
# Author: Sankar T S
# Description: This stored procedure return all current cart details
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

CREATE PROCEDURE [dbo].[usp_OrbioAdmin_GetCurrentCartDetails]
	@shoppingCartTypeId int = null,
	@storeId Int = null
AS
BEGIN

Declare @XmlResult xml;
--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Admin.Customers' AS ns)
SELECT @XmlResult = (select(select cus.Id as 'Id',cus.Email as 'Email',[dbo].[ufn_Admin_GetCartByCustomerId](cus.Id,@storeId,@shoppingCartTypeId) from dbo.Customer cus 
inner join dbo.ShoppingCartItem sc
on cus.Id = sc.CustomerId where sc.ShoppingCartTypeId = @shoppingCartTypeId order by sc.Id desc
for XML path('Customer'),Root('Customers'),type) for XML Path('ShoppingCart'))

SELECT @XmlResult as XmlResult	
	
END

GO
PRINT 'Created the procedure usp_OrbioAdmin_GetCurrentCartDetails'
GO  