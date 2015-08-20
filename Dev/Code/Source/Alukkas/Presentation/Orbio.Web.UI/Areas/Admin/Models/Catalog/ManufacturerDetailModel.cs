using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class ManufacturerDetailModel
    {
        public ManufacturerDetailModel()
        {
            Products = new List<ProductModel>();
            ManufacturerList = new List<ManufacturerModel>();
        }

        public ManufacturerDetailModel(Manufacturers result)
        {
            Manufacturer = new ManufacturerModel(result);

            if (result.Products != null)
            {
                Products = (from c in result.Products
                            select new ProductModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                IsFeaturedProduct = c.IsFeaturedProduct,
                                DisplayOrder = c.DisplayOrder,
                            }).ToList();
            }

            if (result.ManufacturerList != null)
            {
                ManufacturerList = (from c in result.ManufacturerList
                                    select new ManufacturerModel
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Published = c.Published,
                                        DisplayOrder = c.DisplayOrder,
                                    }).ToList();
            }
        }
        public ManufacturerModel Manufacturer { get; set; }

        public List<ManufacturerModel> ManufacturerList { get; set; }

        public List<ProductModel> Products { get; set; }

        public string Search { get; set; }
    }
}