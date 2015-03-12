/****** Object:  UserDefinedTableType [dbo].[ShoppingCartItem]   Script Date: 11/MAR/2015 ******/

IF EXISTS (select * from sys.table_types where name = 'ShoppingCartItem')
BEGIN
	--Get the dependencies of this type, drop them first and then drop it.
	EXEC usp_Maintainance_DropUserDefinedTableType 'ShoppingCartItem'
	
	PRINT 'Dropped the type: [dbo].ShoppingCartItem with its dependencies'
END	
GO

/****** Object:  UserDefinedTableType [dbo].[ShoppingCartItem]    Script Date: 11/MAR/2015  ******/
CREATE TYPE [dbo].[ShoppingCartItem] AS TABLE(
	[CartId] [int] NULL,
	[Remove] [nvarchar](10) NULL,
	[Quantity] [nvarchar](20) NULL
)
GO

print 'Created the type [ShoppingCartItem]'
