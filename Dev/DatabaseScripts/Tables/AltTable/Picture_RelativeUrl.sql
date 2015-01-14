
-- Add Column RelativeUrl 
PRINT 'Adding column RelativeUrl for the table Picture...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Picture' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Picture]') AND name='RelativeUrl')
	Begin
		ALTER TABLE [dbo].[Picture]
		ADD [RelativeUrl] NVARCHAR(MAX) NULL
		PRINT 'Added column RelativeUrl for the table Picture'	
	End
	Else
		PRINT 'Already column RelativeUrl for the table Picture exists'
END
ELSE
	PRINT 'Table Picture Not Exists'
GO