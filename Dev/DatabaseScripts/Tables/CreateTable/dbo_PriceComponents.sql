
-- Creating anew table PriceComponents
PRINT 'Creating a new table PriceComponents'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='PriceComponents' and type='U')
BEGIN

		CREATE TABLE [dbo].[PriceComponents](
			[PriceComponentId] [int] IDENTITY(1,1) NOT NULL,
			[Name] [nvarchar](150) NOT NULL,
			[IsActive] [bit] NOT NULL,
			[Deleted] [bit] NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
			[CreatedBy] [nvarchar](150) NOT NULL,
			[ModifiedDate] [datetime] NOT NULL,
			[ModifiedBy] [nvarchar](150) NOT NULL,
		 CONSTRAINT [PK_PriceComponents] PRIMARY KEY CLUSTERED 
		(
			[PriceComponentId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		PRINT 'Created a new table PriceComponents'	
END
ELSE
	PRINT 'Table PriceComponents Exists'
GO