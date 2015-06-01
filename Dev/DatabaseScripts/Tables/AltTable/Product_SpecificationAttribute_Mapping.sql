
-- Add Columns Product_SpecificationAttribute_Mapping
PRINT 'Adding columns Product_SpecificationAttribute_Mapping for the table Product...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product_SpecificationAttribute_Mapping' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product_SpecificationAttribute_Mapping]') AND name='SubTitle')
	Begin
		ALTER TABLE [dbo].[Product_SpecificationAttribute_Mapping]
		ADD [SubTitle] varchar(50) NULL
		PRINT 'Added column Product_SpecificationAttribute_Mapping for the table Product'	
	End
	Else
		PRINT 'Already column Product_SpecificationAttribute_Mapping for the table Product exists'
END
ELSE
	PRINT 'Table Product_SpecificationAttribute_Mapping Not Exists'
GO