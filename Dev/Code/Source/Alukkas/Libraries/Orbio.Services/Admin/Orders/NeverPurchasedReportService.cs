using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public class NeverPurchasedReportService : INeverPurchasedReportService
    {
        public  List<Product> GetAllNeverPurchasedProductReport(DateTime? StartDate, DateTime? EndDate)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = (from oi in context.OrderItems
                             join o in context.Orders on oi.OrderId equals o.Id
                             where (!StartDate.HasValue || StartDate.Value <= o.CreatedOnUtc) &&
                              (!EndDate.HasValue || EndDate.Value >= o.CreatedOnUtc) &&
                              (!o.Deleted)
                             select oi.ProductId).Distinct();

                var result = (from p in context.Products
                             orderby p.Name
                              where (!query.Contains(p.Id)) &&
                                   (!p.Deleted)
                             select p).ToList();
                return result;
            }
        }
    }
}
