
-- Add Columns FirstName,LastName,Gender,DOB,CompanyName,MobileNo 
PRINT 'Adding columns GoldCost,StoneCost,MakingCost for the table Product...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name='GoldCost')
	Begin
		ALTER TABLE [dbo].[Product]
		ADD [GoldCost] decimal(18,4) NULL
		PRINT 'Added column GoldCost for the table Product'	
	End
	Else
		PRINT 'Already column GoldCost for the table Product exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name='StoneCost')
	Begin
		ALTER TABLE [dbo].[Product]
		ADD [StoneCost] decimal(18,4) NULL
		PRINT 'Added column StoneCost for the table Product'	
	End
	Else
		PRINT 'Already column StoneCost for the table Product exists'

	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name='MakingCost')
	Begin
		ALTER TABLE [dbo].[Product]
		ADD [MakingCost] decimal(18,4)NULL
		PRINT 'Added column MakingCost for the table Product'	
	End
	Else
		PRINT 'Already column MakingCost for the table Product exists'
	
END
ELSE
	PRINT 'Table Product Not Exists'
GO