using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;
using Nop.Data;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Services.Seo
{
    public class UrlRecordService : IUrlRecordService
    {
        private readonly IDbContext dbContext;
        private readonly ICacheManager cacheManager;

        public UrlRecordService(IDbContext dbContext, ICacheManager cacheManager)
        {
            this.dbContext = dbContext;
            this.cacheManager = cacheManager;
        }

        public List<UrlRecord> GetAllUrlRecords()
        {
            throw new NotImplementedException();
        }

        public UrlRecord GetBySlug(string slug)
        {
            throw new NotImplementedException();
        }
    }
}
