
-- Add Column CustomerName
PRINT 'Adding column CustomerName for the table ProductReview...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='ProductReview' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ProductReview]') AND name='CustomerName')
	Begin
		ALTER TABLE [dbo].[ProductReview]
		ADD [CustomerName] VARCHAR(50) NULL
		PRINT 'Added column CustomerName for the table ProductReview'	
	End
	Else
		PRINT 'Already column CustomerName for the table ProductReview exists'
		END
ELSE
	PRINT 'Table ProductReview Not Exists'
GO