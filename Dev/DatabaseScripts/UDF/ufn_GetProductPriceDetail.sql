/*
 ===================================================================================================================================================
 Author:  Sankar T S
 Create date: 16 june 2015
 Description: This function will return the Product unit and weight
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetProductPriceDetail]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetProductPriceDetail]
	PRINT 'Dropped UDF [dbo].[ufn_GetProductPriceDetail]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetProductPriceDetail]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[ufn_GetProductPriceDetail] (@productid int)  
RETURNS @TABLE TABLE (Weight decimal(18,4),ProductUnit decimal(18,4))  
AS  
BEGIN  
    INSERT @TABLE
 Select Weight,ProductUnit from [dbo].[Product] where Id= @productid and ProductUnit is not null
 RETURN  
   
END


GO
PRINT 'Created UDF [dbo].[ufn_GetProductPriceDetail]`'
GO  