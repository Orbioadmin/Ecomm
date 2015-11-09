
-- Add Columns IsGift,GiftCharge
PRINT 'Adding columns IsGift for the table OrderItem...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='OrderItem' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND name='IsGift')
	Begin
		ALTER TABLE [dbo].[OrderItem]
		ADD [IsGift] bit NOT NULL DEFAULT(0)
		PRINT 'Added column IsGift for the table OrderItem'	
	End
	Else
		PRINT 'Already column IsGift for the table OrderItem exists'
		
	
END
ELSE
	PRINT 'Table OrderItem Not Exists'
GO