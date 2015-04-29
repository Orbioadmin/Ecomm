
-- Creating a new table Product_ProductVarientValue_ProductComponent_Mapping
PRINT 'Creating a new table Product_ProductVarientValue_ProductComponent_Mapping'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Product_ProductVarientValue_ProductComponent_Mapping' and type='U')
BEGIN
CREATE TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ComponentId] [int] NOT NULL,
	[ProductVarientAttributeValueId] [int] NOT NULL,
	[Weight] [decimal](18, 4) NOT NULL,
	[UnitPrice] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Product_ProductVarientValue_ProductComponent_Mapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductVarientValue_ProductComponent_Mapping_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])

ALTER TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping] CHECK CONSTRAINT [FK_Product_ProductVarientValue_ProductComponent_Mapping_Product]

ALTER TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductVarientValue_ProductComponent_Mapping_ProductComponent] FOREIGN KEY([ComponentId])
REFERENCES [dbo].[ProductComponent] ([ComponentId])

ALTER TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping] CHECK CONSTRAINT [FK_Product_ProductVarientValue_ProductComponent_Mapping_ProductComponent]

ALTER TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductVarientValue_ProductComponent_Mapping_ProductVariantAttributeValue] FOREIGN KEY([ProductVarientAttributeValueId])
REFERENCES [dbo].[ProductVariantAttributeValue] ([Id])

ALTER TABLE [dbo].[Product_ProductVarientValue_ProductComponent_Mapping] CHECK CONSTRAINT [FK_Product_ProductVarientValue_ProductComponent_Mapping_ProductVariantAttributeValue]

		PRINT 'Created a new table Product_ProductVarientValue_ProductComponent_Mapping'	
END
ELSE
	PRINT 'Table Product_ProductVarientValue_ProductComponent_Mapping Exists'
GO