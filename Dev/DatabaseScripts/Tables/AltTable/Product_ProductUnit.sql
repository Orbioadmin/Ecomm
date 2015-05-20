
-- Add Columns ProductUnit
PRINT 'Adding columns ProductUnit for the table Product...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name='ProductUnit')
	Begin
		ALTER TABLE [dbo].[Product]
		ADD [ProductUnit] decimal(18,4) NULL
		PRINT 'Added column ProductUnit for the table Product'	
	End
	Else
		PRINT 'Already column ProductUnit for the table Product exists'
END
ELSE
	PRINT 'Table Product Not Exists'
GO