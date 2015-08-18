/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 16 Aug 2015
 Description: This function will return order note
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAdminOrderNoteDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAdminOrderNoteDetails]
	PRINT 'Dropped UDF [dbo].[ufn_GetAdminOrderNoteDetails]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAdminOrderNoteDetails]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAdminOrderNoteDetails](@orderId INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.OrderNote' AS ns)
		SELECT @xmlResult = (Select orn.Id,orn.OrderId,orn.Note,orn.CreatedOnUtc from dbo.OrderNote orn inner join [Order] ord on orn.OrderId = ord.Id
					 where orn.OrderId = @orderId for xml path('OrderNote'),Root('OrderNotes'),type)
		    
	 Return @xmlresult

END

GO
PRINT 'Created the UDF ufn_GetAdminOrderNoteDetails'
GO  