
-- Creating anew table TaxTypes
PRINT 'Creating a new table TaxType'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='TaxType' and type='U')
BEGIN
CREATE TABLE [dbo].[TaxType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaxType] [nvarchar](50) NOT NULL,	 
	[DisplayName] [nvarchar](100)
 CONSTRAINT [PK_TaxType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
		PRINT 'Created a new table TaxType'	
END
ELSE
	PRINT 'Table TaxType Exists'
GO