using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class SpecificationAttributeModel
    {
        public SpecificationAttributeModel()
        {
            SpecificationOption = new List<SpecificationAttributeOptionModel>();
        }
        public SpecificationAttributeModel(SpecificationAttribute spec)
        {
            Id = spec.Id;
            Name = spec.Name;
            DisplayOrder = spec.DisplayOrder;
            SpecificationOption = (from s in spec.SpecificationAttributeOptions
                                   select new SpecificationAttributeOptionModel()
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       DisplayOrder = s.DisplayOrder,
                                       ProductCount = s.Product_SpecificationAttribute_Mapping.Count(),
                                   }).ToList();

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public List<SpecificationAttributeOptionModel> SpecificationOption { get; set; }
    }
}