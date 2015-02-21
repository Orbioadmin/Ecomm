
-- Add Columns FirstName,LastName,Gender,DOB,CompanyName,MobileNo 
PRINT 'Adding columns FirstName,LastName,Gender,DOB,CompanyName,MobileNo for the table Customer...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Customer' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND name='FirstName')
	Begin
		ALTER TABLE [dbo].[Customer]
		ADD [FirstName] VARCHAR(50) NULL
		PRINT 'Added column FirstName for the table Customer'	
	End
	Else
		PRINT 'Already column FirstName for the table Customer exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND name='LastName')
	Begin
		ALTER TABLE [dbo].[Customer]
		ADD [LastName] VARCHAR(50) NULL
		PRINT 'Added column LastName for the table Customer'	
	End
	Else
		PRINT 'Already column LastName for the table Customer exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND name='Gender')
	Begin
		ALTER TABLE [dbo].[Customer]
		ADD [Gender] VARCHAR(20) NULL
		PRINT 'Added column Gender for the table Customer'	
	End
	Else
		PRINT 'Already column Gender for the table Customer exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND name='DOB')
	Begin
		ALTER TABLE [dbo].[Customer]
		ADD [DOB] VARCHAR(20) NULL
		PRINT 'Added column DOB for the table Customer'	
	End
	Else
		PRINT 'Already column DOB for the table Customer exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND name='CompanyName')
	Begin
		ALTER TABLE [dbo].[Customer]
		ADD [CompanyName] VARCHAR(50) NULL
		PRINT 'Added column CompanyName for the table Customer'	
	End
	Else
		PRINT 'Already column CompanyName for the table Customer exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND name='MobileNo')
	Begin
		ALTER TABLE [dbo].[Customer]
		ADD [MobileNo] VARCHAR(20) NULL
		PRINT 'Added column MobileNo for the table Customer'	
	End
	Else
		PRINT 'Already column MobileNo for the table Customer exists'
END
ELSE
	PRINT 'Table Customer Not Exists'
GO