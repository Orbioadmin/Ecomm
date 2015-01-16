using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Services.Seo
{
    public interface IUrlRecordService
    {
        /// <summary>
        /// Gets all the url records
        /// </summary>
        /// <returns>list of url records</returns>
        List<UrlRecord> GetAllUrlRecords();

        /// <summary>
        /// gets an url record by slug
        /// </summary>
        /// <returns>an url record</returns>
        UrlRecord GetBySlug(string slug);
    }
}
