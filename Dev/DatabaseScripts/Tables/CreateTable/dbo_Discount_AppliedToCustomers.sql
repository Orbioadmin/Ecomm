
-- Creating a new table SimilarProduct
PRINT 'Creating a new table Discount_AppliedToCustomers'
IF NOT EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE name='Discount_AppliedToCustomers' and type='U')
BEGIN
CREATE TABLE [dbo].[Discount_AppliedToCustomers](
	[Discount_Id] [int] NOT NULL,
	[Customer_Id] [int] NOT NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[Discount_AppliedToCustomers]  WITH CHECK ADD  CONSTRAINT [FK_Discount_AppliedToCustomers_Customer] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[Discount_AppliedToCustomers] CHECK CONSTRAINT [FK_Discount_AppliedToCustomers_Customer]

ALTER TABLE [dbo].[Discount_AppliedToCustomers]  WITH CHECK ADD  CONSTRAINT [FK_Discount_AppliedToCustomers_Discount] FOREIGN KEY([Discount_Id])
REFERENCES [dbo].[Discount] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[Discount_AppliedToCustomers] CHECK CONSTRAINT [FK_Discount_AppliedToCustomers_Discount]

		PRINT 'Created a new table Discount_AppliedToCustomers'	
END
ELSE
	PRINT 'Table Discount_AppliedToCustomers Exists'
GO