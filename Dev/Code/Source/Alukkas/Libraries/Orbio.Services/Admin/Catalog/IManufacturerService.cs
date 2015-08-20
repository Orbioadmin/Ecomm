using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Catalog
{
    public interface IManufacturerService
    {
        Manufacturers GetAllManufacturers();

        Manufacturers SearchManufacturerByName(string Name);

        int AddOrUpdateManufacturer(ManufacturerDetails Manufacturer);

        int DeleteManufacturer(int Id);

        Manufacturers GetManufacturerDetailsById(int Id);

        Manufacturers GetManufacturerDetails();

        //int DeleteProductMapping(int ManufacturerId, int ProductId);
    }
}
