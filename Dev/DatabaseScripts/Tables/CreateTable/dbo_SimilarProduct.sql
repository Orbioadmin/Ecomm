
-- Creating a new table SimilarProduct
PRINT 'Creating a new table SimilarProduct'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='SimilarProduct' and type='U')
BEGIN
CREATE TABLE [dbo].[SimilarProduct](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId1] [int] NOT NULL,
	[ProductId2] [int] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

		PRINT 'Created a new table SimilarProduct'	
END
ELSE
	PRINT 'Table SimilarProduct Exists'
GO