using Orbio.Core.Domain.Directory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Directory
{
    /// <summary>
    /// Country service interface
    /// </summary>
    public partial interface ICountryService
    {
        /// <summary>
        /// Gets all countries 
        /// </summary>
        /// <returns>Country List</returns>
        List<Country> GetAllCountries();
    }
}
