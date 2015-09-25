IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_OrbioAdmin_GetOrderAverageReport]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_OrbioAdmin_GetOrderAverageReport]
	PRINT 'Dropped [dbo].[usp_OrbioAdmin_GetOrderAverageReport]'
END	
GO

PRINT 'Creating [dbo].[usp_OrbioAdmin_GetOrderAverageReport]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_OrbioAdmin_GetOrderAverageReport
# File Path:
# CreatedDate: 23-SEP-2015
# Author: Sankar
# Description: This stored procedure return average report on order
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
CREATE PROCEDURE [dbo].[usp_OrbioAdmin_GetOrderAverageReport]
AS
BEGIN
DECLARE @XmlResult1 xml

SELECT @XmlResult1 = (SELECT   DATENAME(month, CreatedOnUtc) AS "monthName", 
         sum(case when OrderStatusId = 10 then 1 else 0 end) orderPendingCount, 
        sum(case when OrderStatusId = 20 then 1 else 0 end) orderProcessingCount,
         sum(case when OrderStatusId = 30 then 1 else 0 end) orderCompleteCount,
         sum(case when OrderStatusId = 40 then 1 else 0 end) orderCancelledCount
FROM    [dbo].[Order]
where CreatedOnUtc >= DATEADD(month,-12,DATEADD(day,DATEDIFF(day,0,GETDATE()),0)) AND CreatedOnUtc <=DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
GROUP BY DATENAME(month, CreatedOnUtc)
FOR XML PATH('OrderAverageReport'),Root('ArrayOfOrderAverageReport'))

SELECT @XmlResult1 as XmlResult

END
GO
PRINT 'Created the procedure usp_OrbioAdmin_GetOrderAverageReport'
GO  


