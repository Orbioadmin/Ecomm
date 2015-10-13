using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Data;

namespace Orbio.Services.Admin.Seo
{
    public interface IUrlRecordService
    {
        /// <summary>
        /// Get url record details by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        UrlRecord GetBySlug(string slug);

        /// <summary>
        /// Get active slug by id and entity name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        string GetActiveSlug(int id, string entityName);
    }
}
