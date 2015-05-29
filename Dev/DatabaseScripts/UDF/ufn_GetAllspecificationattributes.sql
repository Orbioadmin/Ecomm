/*
 =============================================  
 Author:  Sankar T.S
 Create date: 06 Feb 2015
 Description: It will get the all product specification attribute
 Return format: XML
 ===================================================================================================================================================  
*/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ufn_GetAllspecificationattributes]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[ufn_GetAllspecificationattributes]
	PRINT 'Dropped UDF [dbo].[ufn_GetAllspecificationattributes]'
GO
PRINT 'Creating UDF [dbo].[ufn_GetAllspecificationattributes]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_GetAllspecificationattributes](@productid INT)
RETURNS XML
AS --WITH RETURNS NULL ON NULL INPUT 
BEGIN 
	   DECLARE @xmlResult xml;
	 
		--WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/Orbio.Core.Domain.Catalog' AS ns)
		SELECT @xmlResult = ( select SAO.SpecificationAttributeId 'SpecificationAttributeId',SA.Name 'SpecificationAttributeName',SAO.Id 'SpecificationAttributeOptionId' ,CASE WHEN ISNULL(PSA.CustomValue,'')<>'' THEN PSA.CustomValue ELSE SAO.Name END 'SpecificationAttributeOptionName',PSA.SubTitle as 'SubTitle' from dbo.Product product 
inner join dbo.Product_SpecificationAttribute_Mapping PSA on product.Id = PSA.ProductId
inner join dbo.SpecificationAttributeOption SAO on SAO.Id= PSA.SpecificationAttributeOptionId
inner join dbo.SpecificationAttribute SA on SA.Id=SAO.SpecificationAttributeId

 WHERE Product.Id=@productid AND ShowOnProductPage = 1


 FOR XML PATH('SpecificationFilterModel'), ROOT('SpecificationFilters'), TYPE )
		    
		 
	 Return @xmlresult

END



GO
PRINT 'Created UDF: ufn_GetAllspecificationattributes'
GO