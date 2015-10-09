using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Attributes
{
    public interface ISpecificationAttributeService
    {
        List<SpecificationAttribute> GetSpecificationAttributes();

        SpecificationAttribute GetSpecificationAttributesById(int Id);

        int AddOrUpdate(int Id, string Name, int DisplayOrder);

        int DeleteSpecificationAttribute(int Id);

        int DeleteSpecificationAttributeOption(int Id);

        SpecificationAttributeOption GetSpecificationAttributeOptionById(int Id);

        int AddSpecificationOption(int Id,string Name,int DisplayOrder,int SpecificationAttributeId);

        List<SpecificationAttributeOption> GetSpecificationAttributeOptionBySpecId(int Id);
    }
}
