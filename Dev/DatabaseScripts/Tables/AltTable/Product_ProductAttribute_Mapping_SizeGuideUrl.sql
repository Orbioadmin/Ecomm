PRINT 'Adding columns SizeGuideUrl for the table Product_ProductAttribute_Mapping...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product_ProductAttribute_Mapping' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Product_ProductAttribute_Mapping]') AND name='SizeGuideUrl')
	Begin
		ALTER TABLE [dbo].[Product_ProductAttribute_Mapping]
		ADD SizeGuideUrl VARCHAR(50) NULL
		PRINT 'Added column SizeGuideUrl for the table Product_ProductAttribute_Mapping'	
	End
	Else
		PRINT 'Already column SizeGuideUrl for the table Product_ProductAttribute_Mapping exists'

	
END
ELSE
	PRINT 'Table Product_ProductAttribute_Mapping Not Exists'
GO