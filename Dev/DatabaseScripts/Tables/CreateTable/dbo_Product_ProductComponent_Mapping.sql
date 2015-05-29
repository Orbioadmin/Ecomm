
-- Creating a new table Product_ProductComponent_Mapping
PRINT 'Creating a new table Product_ProductComponent_Mapping'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product_ProductComponent_Mapping' and type='U')
BEGIN
		CREATE TABLE [dbo].[Product_ProductComponent_Mapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ComponentId] [int] NOT NULL,
	[Weight] [decimal](18, 4) NOT NULL,
	[UnitPrice] [decimal](18, 4) NOT NULL,
		 CONSTRAINT [PK_Product_ProductComponent_Mapping] PRIMARY KEY CLUSTERED 
		(
			[Id] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
ALTER TABLE [dbo].[Product_ProductComponent_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductComponent_Mapping_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ALTER TABLE [dbo].[Product_ProductComponent_Mapping] CHECK CONSTRAINT [FK_Product_ProductComponent_Mapping_Product]
ALTER TABLE [dbo].[Product_ProductComponent_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductComponent_Mapping_ProductComponents] FOREIGN KEY([ComponentId])
REFERENCES [dbo].[ProductComponent] ([Id])
ALTER TABLE [dbo].[Product_ProductComponent_Mapping] CHECK CONSTRAINT [FK_Product_ProductComponent_Mapping_ProductComponents]
		PRINT 'Created a new table Product_ProductComponent_Mapping'	
END
ELSE
	PRINT 'Table Product_ProductComponent_Mapping Exists'
GO