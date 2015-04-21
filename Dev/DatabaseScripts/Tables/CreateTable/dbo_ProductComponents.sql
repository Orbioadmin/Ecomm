
-- Creating anew table ProductComponents
PRINT 'Creating a new table ProductComponents'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='ProductComponents' and type='U')
BEGIN

		CREATE TABLE [dbo].[ProductComponents](
			[ComponentId] [int] IDENTITY(1,1) NOT NULL,
			[ProductId] [int] NOT NULL,
			[ComponentName] [nvarchar](200) NOT NULL,
			[Weight] [decimal](18, 0) NOT NULL,
			[UnitPrice] [decimal](18, 0) NOT NULL,
			[IsActive] [bit] NOT NULL,
			[Deleted] [bit] NOT NULL,
			[CreatedBy] [nvarchar](150) NOT NULL,
			[CreatedDate] [datetime] NOT NULL,
			[ModifiedBy] [nvarchar](150) NOT NULL,
			[ModifiedDate] [datetime] NOT NULL,
		 CONSTRAINT [PK_ProductComponents] PRIMARY KEY CLUSTERED 
		(
			[ComponentId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		ALTER TABLE [dbo].[ProductComponents]  WITH CHECK ADD  CONSTRAINT [FK_ProductComponents_Product] FOREIGN KEY([ProductId])
		REFERENCES [dbo].[Product] ([Id])
		ALTER TABLE [dbo].[ProductComponents] CHECK CONSTRAINT [FK_ProductComponents_Product]
		PRINT 'Created a new table ProductComponents'	
END
ELSE
	PRINT 'Table ProductComponents Exists'
GO