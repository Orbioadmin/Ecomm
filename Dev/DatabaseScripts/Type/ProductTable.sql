/****** Object:  UserDefinedTableType [dbo].[ProductTable]   Script Date: 17/DEC/2012 ******/

IF EXISTS (select * from sys.table_types where name = 'ProductTable')
BEGIN
	--Get the dependencies of this type, drop them first and then drop it.
	EXEC usp_Maintainance_DropUserDefinedTableType 'ProductTable'
	
	PRINT 'Dropped the type: [dbo].ProductTable with its dependencies'
END	
GO

/****** Object:  UserDefinedTableType [dbo].[ProductTable]    Script Date: 17/DEC/2012  ******/
CREATE TYPE [dbo].[ProductTable] AS TABLE(
   Id INT,
   Name nvarchar(400),
   ShortDescription nvarchar(max),
   Price decimal(18,4),
   ViewPath nvarchar(400),
   CurrencyCode nvarchar(5),
   ImageRelativeUrl nvarchar(max),
   Slug nvarchar(400)
)
GO

print 'Created the type [ProductTable]'
