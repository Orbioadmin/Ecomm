using Nop.Core.Domain;
using Nop.Data;
using Orbio.Core.Domain.Directory;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Directory
{
    public partial class StateProvinceService:IStateProvinceService
    {
        private readonly IDbContext context;
        /// <summary>
        /// instantiates StateProvinceService service type
        /// </summary>
        /// <param name="context">db context</param>
        public StateProvinceService(IDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>State/province collection</returns>
        public List<StateProvince> GetStateProvincesByCountryId(int? countryId)
        {
            var sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter() { ParameterName = "@countryId", Value = (object)countryId ?? DBNull.Value, DbType = System.Data.DbType.Int32 });
            var result = context.ExecuteFunction<XmlResultSet>("usp_StateProvince_GetStateProvince", sqlParamList.ToArray()).FirstOrDefault();
            if (result != null && result.XmlResult != null)
            {
                var stateProvince = Serializer.GenericDeSerializer<List<StateProvince>>(result.XmlResult);
                return stateProvince;
            }

            return new List<StateProvince>();
        }
    }
}
