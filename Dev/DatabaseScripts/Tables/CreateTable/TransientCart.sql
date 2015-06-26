
-- Creating anew table TransientCarts
PRINT 'Creating a new table TransientCart'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='TransientCart' and type='U')
BEGIN
CREATE TABLE [dbo].[TransientCart](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[TransientCartXml] [xml] NOT NULL,
	[CreatedDate] [datetime] NOT NULL default getdate(),
	[ModifiedDate] [datetime] NOT NULL default getdate(),
 CONSTRAINT [PK_TransientCart] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
		PRINT 'Created a new table TransientCart'	
END
ELSE
	PRINT 'Table TransientCart Exists'
GO