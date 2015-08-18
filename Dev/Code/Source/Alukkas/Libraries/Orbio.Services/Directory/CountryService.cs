using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Directory;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Directory
{
    public partial class CountryService:ICountryService
    {
        private readonly IDbContext context;
        /// <summary>
        /// instantiates Country service type
        /// </summary>
        /// <param name="context">db context</param>
        public CountryService(IDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// gets all countries
        /// </summary>
        /// <returns>list of country</returns>
        public List<Country> GetAllCountries()
        {
            var result = context.ExecuteFunction<XmlResultSet>("usp_Country_GetAllCountries", null).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var countries = Serializer.GenericDeSerializer<List<Country>>(result.XmlResult);
                return countries;
            }

            return new List<Country>();
        }
    }
}
