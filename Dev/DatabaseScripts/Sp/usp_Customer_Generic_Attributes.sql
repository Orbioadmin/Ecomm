 IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Customer_Generic_Attributes]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[usp_Customer_Generic_Attributes]
	PRINT 'Dropped [dbo].[usp_Customer_Generic_Attributes]'
END	
GO

PRINT 'Creating [dbo].[usp_Customer_Generic_Attributes]'
GO

/* ******************************** PROLOG *******************************************
# Procedure Name: usp_Customer_Generic_Attributes
# File Path:
# CreatedDate: 09-march-2015
# Author: 30-mar-2015
# Description: This procedure will get or save all generic attributes for customer
# History  of changes:
#--------------------------------------------------------------------------------------
# Version No.	Date of Change		Changed By		Reason for change
#--------------------------------------------------------------------------------------
*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_Customer_Generic_Attributes]

@action varchar(30),
@entityId int,
@keyGroup varchar(50),
@key varchar(100),
@value varchar(300),
@storeId int

AS
BEGIN

-- Get all generic attributes using by customer id and key
if(@action = 'select')
	begin
		select [Value] from [dbo].[GenericAttribute] where [EntityId] = @entityId and [Key] = @key
	end

-- Insert customer generic attributes to [GenericAttribute] table
if(@action = 'save')
	begin

	if not exists(select Id from [dbo].[GenericAttribute] where [EntityId] = @entityId and [Key] = @key and [KeyGroup] = @keyGroup)
		begin
			insert into [dbo].[GenericAttribute](EntityId,KeyGroup,[Key],Value,StoreId) values(@entityId,@keyGroup,@key,@value,@storeId)
		end
	else if exists(select Id from [dbo].[GenericAttribute] where [EntityId] = @entityId and [Key] = @key and [KeyGroup] = @keyGroup)
		begin
			update [dbo].[GenericAttribute] set Value = @value where [EntityId] = @entityId and [Key] = @key
		end

	end

END

GO
PRINT 'Created the procedure usp_Customer_Generic_Attributes'
GO  
