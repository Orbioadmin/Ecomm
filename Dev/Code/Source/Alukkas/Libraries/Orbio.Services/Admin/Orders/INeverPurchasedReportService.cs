using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public interface INeverPurchasedReportService
    {
        List<Product> GetAllNeverPurchasedProductReport(DateTime? StartDate, DateTime? EndDate);
    }
}
