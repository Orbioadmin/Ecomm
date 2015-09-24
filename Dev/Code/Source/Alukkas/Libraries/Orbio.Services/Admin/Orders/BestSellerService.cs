using Orbio.Core.Data;
using Orbio.Core.Domain.Admin.Catalog;
using Orbio.Core.Domain.Admin.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Orders
{
    public class BestSellerService : IBestSellerService
    {
        public  BestSellers GetAllDetailsForSearch()
        {
            var model = new BestSellers();
            using (var context = new OrbioAdminContext())
            {
                model.Category = (from c in context.Categories
                                  where !c.Deleted
                                  select new CategoryList()
                                  {
                                      Id=c.Id,
                                      Name=c.Name,
                                  }).ToList();
                model.Manufacturers = (from m in context.Manufacturers
                                       where !m.Deleted
                                       select new ManufacturerDetails()
                                       {
                                           Id = m.Id,
                                           Name = m.Name,
                                       }).ToList();
            }
            return model;
        }

        public List<BestSellers> GetAllSellerDetails(DateTime? StartDate, DateTime? EndDate, int orderStatusId, int paymentStatusId, int categoryId, int manufacturerId)
        {
            using (var context = new OrbioAdminContext())
            {
                var query=(from oi in context.OrderItems
                            join o in context.Orders on oi.OrderId equals o.Id
                             join p in context.Products on oi.ProductId equals p.Id
                               where !o.Deleted && !p.Deleted &&
                               (!StartDate.HasValue || StartDate.Value <= o.CreatedOnUtc) &&
                               (!EndDate.HasValue || EndDate.Value >= o.CreatedOnUtc) &&
                               (orderStatusId==0 || orderStatusId == o.OrderStatusId) &&
                               (paymentStatusId == 0 || paymentStatusId == o.PaymentStatusId) &&
                               (categoryId == 0 || p.Product_Category_Mapping.Count(pc => pc.CategoryId == categoryId) > 0) &&
                               (manufacturerId == 0 || p.Product_Manufacturer_Mapping.Count(pm => pm.ManufacturerId == manufacturerId) > 0)
                               select oi).ToList();

                var result=(from q in query
                                group q by q.ProductId into g
                                select new BestSellers()
                                {
                                    Id = g.Key,
                                    Amount = g.Sum(x => x.PriceExclTax),
                                    Quantity = g.Sum(x => x.Quantity),
                                    Name = (from p in (context.Products.Where(m => m.Id == g.Key))
                                            select p.Name).First(),
                                }).OrderByDescending(m=>m.Quantity).ToList();
                return result;
            }
        }
    }
}
