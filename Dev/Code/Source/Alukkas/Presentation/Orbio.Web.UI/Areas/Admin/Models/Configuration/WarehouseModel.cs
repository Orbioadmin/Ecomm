using Orbio.Core.Data;
using Orbio.Web.UI.Area.Admin.Models.CheckOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Configuration
{
    public class WarehouseModel
    {
        public WarehouseModel()
        {
            this.WarehouseDetails = new AddressModel();
        }

        public WarehouseModel(Warehouse result)
        {
            Id = result.Id;
            Name = result.Name;
            AddressId = result.AddressId;
            WarehouseDetails = new AddressModel(result.Address);
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int AddressId { get; set; }

        public AddressModel WarehouseDetails { get; set; }
    }
}