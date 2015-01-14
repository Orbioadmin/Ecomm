IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Maintainance_DropUserDefinedTableType]') AND type in (N'P', N'PC'))
BEGIN
                       DROP PROCEDURE [dbo].[usp_Maintainance_DropUserDefinedTableType]
                       PRINT 'Dropped [dbo].[usp_Maintainance_DropUserDefinedTableType]'
END                       
GO
PRINT 'Creating [dbo].[usp_Maintainance_DropUserDefinedTableType]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Manju DJ
-- Create date: 25 Dec 2011
-- Description:	This procedure used to drop User Defined Data Table Type
-- =============================================

--Exec usp_Maintainance_DropUserDefinedTableType 'UTT Name'
Create Proc usp_Maintainance_DropUserDefinedTableType
@TypeName varchar(255)
AS
Begin

	IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = @TypeName)
	BEGIN
		Print 'Dropping Dependent object of '+@TypeName
		Create table #tmp (ObjName varchar(1000), ObjType Varchar(10))
		Insert into #tmp (ObjName,ObjType)
		select OBJECT_NAME(Sc.id),so.type 
		from sys.syscomments SC 
		inner join sys.sysobjects SO on sc.id=so.id
		where text like '%'+@TypeName+'%'
		
		
		Declare @ObjName varchar(1000)
		Declare @ObjType varchar(10)
		Declare @strSQL varchar(max)
		Set @strSQL=''
		Declare tmpCur Cursor For Select ObjName,ObjType from #tmp 
		
		Open tmpCur
		FETCH NEXT FROM tmpCur 
		INTO @ObjName, @ObjType;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			IF @ObjType='P'
			Begin
				--Set @strSQL=@strSQL + ' DROP PROCEDURE ' +  @ObjName
				IF(@ObjName!='usp_Maintainance_DropUserDefinedTableType')
				Begin
					Execute('DROP PROCEDURE '+@ObjName)
					Print 'Dropped SP '+@ObjName
			End	End
			IF @ObjType  in (N'FN', N'IF', N'TF', N'FS', N'FT')
			Begin
				--Set @strSQL=@strSQL + ' DROP FUNCTION ' +  @ObjName
				Execute(' DROP FUNCTION ' +  @ObjName)
				Print 'Dropped FUNCTION '+@ObjName
			End
			FETCH NEXT FROM tmpCur 
			INTO @ObjName, @ObjType;
		END
		Close tmpCur
		Deallocate tmpCur
		Print 'Dropped Dependent object of '+@TypeName
		Execute('DROP TYPE '+@TypeName)
		Print 'Dropped Type ' + @TypeName

	END
End
GO
PRINT 'Creating [dbo].[usp_Maintainance_DropUserDefinedTableType]'
GO
