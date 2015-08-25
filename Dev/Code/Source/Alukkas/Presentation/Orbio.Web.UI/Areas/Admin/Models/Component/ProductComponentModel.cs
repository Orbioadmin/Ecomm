using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Component
{
    public class ProductComponentModel
    {
        public ProductComponentModel()
        { }

        public ProductComponentModel(ProductComponent prod)
        {
            Id = prod.Id;
            Name = prod.ComponentName;
            IsActive = prod.IsActive;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        //public DateTime ModifiedDate { get; set; }
    }
}