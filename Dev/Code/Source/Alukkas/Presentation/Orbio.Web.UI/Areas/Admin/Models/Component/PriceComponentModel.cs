using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Component
{
    public class PriceComponentModel
    {
        public PriceComponentModel()
        { }

        public PriceComponentModel(PriceComponent price)
        {
            Id = price.Id;
            Name = price.Name;
            IsActive = price.IsActive;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}