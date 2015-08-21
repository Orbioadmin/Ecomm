
-- Add Columns ShoppingCartStatusId
PRINT 'Adding columns ShoppingCartStatusId'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='ShoppingCartStatusId' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ShoppingCartItem]') AND name='ShoppingCartStatusId')
	Begin
		ALTER TABLE [dbo].[ShoppingCartItem]
		ADD [ShoppingCartStatusId] INT NULL
		PRINT 'Added column ShoppingCartStatusId for the table ShoppingCartItem'	
	End
	Else
		PRINT 'Already column ShoppingCartStatusId for the table ShoppingCartItem exists'
END
ELSE
	PRINT 'Table ShoppingCartItem Not Exists'
GO