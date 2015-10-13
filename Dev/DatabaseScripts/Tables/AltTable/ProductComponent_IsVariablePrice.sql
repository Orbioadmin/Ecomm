
-- Add Columns ProductUnit
PRINT 'Adding columns IsVariablePrice for the table ProductComponent...'
IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='ProductComponent' and type='U')
BEGIN
	IF  NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[ProductComponent]') AND name='IsVariablePrice')
	Begin
		ALTER TABLE [dbo].[ProductComponent]
		ADD [IsVariablePrice] [bit] NOT NULL DEFAULT 0
		PRINT 'Added column IsVariablePrice for the table ProductComponent'	
	End
	Else
		PRINT 'Already column ProducIsVariablePricetUnit for the table ProductComponent exists'
END
ELSE
	PRINT 'Table ProductComponent Not Exists'
GO