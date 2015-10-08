using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Configuration
{
    public class ShippingMethodModel
    {
        public ShippingMethodModel()
        {

        }
        public ShippingMethodModel(ShippingMethod ship)
        {
            Id = ship.Id;
            Name = ship.Name;
            Description = ship.Description;
            DisplayOrder = ship.DisplayOrder;
        }


        public int Id { get; set; }

        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public int DisplayOrder { get; set; }
    }
}