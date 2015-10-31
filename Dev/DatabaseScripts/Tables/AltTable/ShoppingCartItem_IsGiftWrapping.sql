
-- Add Columns IsGiftWrapping
PRINT 'Adding columns IsGiftWrapping for the table ShoppingCartItem...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='ShoppingCartItem' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ShoppingCartItem]') AND name='IsGiftWrapping')
	Begin
		ALTER TABLE [dbo].[ShoppingCartItem]
		ADD [IsGiftWrapping] bit NOT NULL DEFAULT(0)
		PRINT 'Added column IsGiftWrapping for the table ShoppingCartItem'	
	End
	Else
		PRINT 'Already column IsGiftWrapping for the table Product ShoppingCartItem'
END
ELSE
	PRINT 'Table ShoppingCartItem Not Exists'
GO