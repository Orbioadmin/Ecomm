
-- Add Column PriceDetailXml 
PRINT 'Adding column PriceDetailXml for the table OrderItem...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='OrderItem' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name='PriceDetailXml')
	Begin
		ALTER TABLE [dbo].[OrderItem]
		ADD [PriceDetailXml] NVARCHAR(MAX) NULL
		PRINT 'Added column PriceDetailXml for the table OrderItem'	
	End
	Else
		PRINT 'Already column PriceDetailXml for the table OrderItem exists'
END
ELSE
	PRINT 'Table OrderItem Not Exists'
GO