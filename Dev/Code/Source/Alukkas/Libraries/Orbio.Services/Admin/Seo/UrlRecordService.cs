using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Data;

namespace Orbio.Services.Admin.Seo
{
    public partial class UrlRecordService : IUrlRecordService
    {
        /// <summary>
        /// Get url record details by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public UrlRecord GetBySlug(string slug)
        {
            using (var context = new OrbioAdminContext())
            {
                return context.UrlRecords.Where(u => u.Slug == slug).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get active slug by id and entity name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public string GetActiveSlug(int id, string entityName)
        {
            using (var context = new OrbioAdminContext())
            {
                var urlRecord = context.UrlRecords.Where(u => u.EntityId == id && u.EntityName == entityName && u.IsActive).FirstOrDefault();
                return urlRecord.Slug;
            }
        }
    }
}
