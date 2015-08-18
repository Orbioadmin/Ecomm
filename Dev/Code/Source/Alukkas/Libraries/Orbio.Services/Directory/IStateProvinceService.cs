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
    public partial interface IStateProvinceService
    {
        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>State/province collection</returns>
        List<StateProvince> GetStateProvincesByCountryId(int? countryId);
    }
}
