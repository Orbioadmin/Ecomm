
-- Creating anew table ProductComponent
PRINT 'Creating a new table ProductComponent'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='ProductComponent' and type='U')
BEGIN
CREATE TABLE [dbo].[ProductComponent](
	[ComponentId] [int] IDENTITY(1,1) NOT NULL,
	[ComponentName] [nvarchar](200) NOT NULL,
	[Weight] [decimal](18, 0) NOT NULL,
	[UnitPrice] [decimal](18, 0) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedBy] [nvarchar](150) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](150) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductComponent] PRIMARY KEY CLUSTERED 
(
	[ComponentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
		PRINT 'Created a new table ProductComponent'	
END
ELSE
	PRINT 'Table ProductComponent Exists'
GO