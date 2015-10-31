
-- Add Columns IsGift,GiftCharge
PRINT 'Adding columns IsGift,GiftCharge for the table Product...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name='IsGift')
	Begin
		ALTER TABLE [dbo].[Product]
		ADD [IsGift] bit NOT NULL DEFAULT(0)
		PRINT 'Added column IsGift for the table Product'	
	End
	Else
		PRINT 'Already column IsGift for the table Product exists'
		
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name='GiftCharge')
	Begin
		ALTER TABLE [dbo].[Product]
		ADD [GiftCharge] decimal(18,4) NOT NULL DEFAULT(0)
		PRINT 'Added column GiftCharge for the table Product'	
	End
	Else
		PRINT 'Already column GiftCharge for the table Product exists'
END
ELSE
	PRINT 'Table Product Not Exists'
GO