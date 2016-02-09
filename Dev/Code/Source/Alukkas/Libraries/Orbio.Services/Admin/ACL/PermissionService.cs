using Nop.Data;
using Orbio.Core.Data;
using Orbio.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.ACL
{
    public class PermissionService : IPermissionService
    {
         private readonly IDbContext dbContext;

        /// <summary>
        /// instantiates Category service type
        /// </summary>
        /// <param name="context">db context</param>
         public PermissionService(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<PermissionRecord> GetAllPermission()
        {
            using(var context=new OrbioAdminContext())
            {
                var permission= context.PermissionRecords.Include("CustomerRoles").OrderBy(m=>m.SystemName).ToList();
                return permission;
            }
        }

        public void UpdatePermissionRecord(PermissionRecord p)
        {
            var roles = GetAllCustomerRoles(p);
             var roleXml = Serializer.GenericSerializer(roles);
            var result = dbContext.ExecuteFunction<Int32>("usp_OrbioAdmin_UpdatePermission",
                new SqlParameter() { ParameterName = "@id", Value = p.Id, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@roles", Value = roleXml, DbType = System.Data.DbType.Xml }).FirstOrDefault();
        }
        public void RemovePermissionRecord(PermissionRecord p)
        {
            var roles = GetAllCustomerRoles(p);
            var roleXml = Serializer.GenericSerializer(roles);
            var result = dbContext.ExecuteFunction<Int32>("usp_OrbioAdmin_UpdatePermission",
                new SqlParameter() { ParameterName = "@id", Value = p.Id, DbType = System.Data.DbType.Int32 },
            new SqlParameter() { ParameterName = "@roles", Value = null, DbType = System.Data.DbType.Xml }).FirstOrDefault();
        }

        public List<int> GetAllCustomerRoles(PermissionRecord p)
        {
            List<int> roles = new List<int>();

            if(p.CustomerRoles.Count>0)
            {
                foreach(var role in p.CustomerRoles)
                {
                    roles.Add(role.Id);
                }
            }
            return roles;
        }
    }
}
