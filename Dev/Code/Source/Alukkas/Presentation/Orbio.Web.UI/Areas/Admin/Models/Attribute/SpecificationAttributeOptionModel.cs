using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class SpecificationAttributeOptionModel
    {
        public SpecificationAttributeOptionModel()
        {
        }

        public SpecificationAttributeOptionModel(SpecificationAttributeOption spec)
        {
            Id = spec.Id;
            Name = spec.Name;
            DisplayOrder = spec.DisplayOrder;
            SpecificationAttributeId = spec.SpecificationAttributeId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public int SpecificationAttributeId { get; set; }

        public int ProductCount { get; set; }
    }
}