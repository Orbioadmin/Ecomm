using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Products
{
    public class ProductReviewService : IProductReviewService
    {
        public List<ProductReview> GetAllProductReviews(DateTime? startDate, DateTime? endDate, string message)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductReviews.Include("Product").Include("Customer").Where(m=>!m.Product.Deleted && !m.Customer.Deleted).OrderByDescending(m=>m.Id).ToList();
                if (startDate.HasValue)
                    result = result.Where(m => m.CreatedOnUtc >= startDate).ToList();
                if(endDate.HasValue)
                    result = result.Where(m => m.CreatedOnUtc <= endDate).ToList();
                if(message!=null)
                    result = result.Where(m => m.Title.Contains(message)).ToList();
                return result;
            }
        }

        public ProductReview EditProductReview(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.ProductReviews.Include("Product").Include("Customer").Where(m => m.Id==id).FirstOrDefault();
                return result;
            }
        }

        public int UpdateProductReview(int id, string title, string reviewText, bool isApproved)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.ProductReviews.Where(m => m.Id == id).FirstOrDefault();
                    if (result != null)
                    {
                        result.Title = title;
                        result.ReviewText = reviewText;
                        result.IsApproved = isApproved;
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }

        public int DeleteProductReview(int id)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    var result = context.ProductReviews.Where(m => m.Id == id).FirstOrDefault();
                    context.ProductReviews.Remove(result);
                    context.SaveChanges();
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }

        public int Approve(List<ProductReview> prdReview)
        {
             try
            {
            using (var context = new OrbioAdminContext())
            {
                foreach (var item in prdReview)
                {
                    var result = context.ProductReviews.Where(m => m.Id == item.Id).FirstOrDefault();
                    if(result!=null)
                    {
                        result.IsApproved = true;
                        context.SaveChanges();
                    }
                }
            }
                return 1;
            }
             catch (Exception)
             {
                 return 0;
             }
        }

        public int Disapprove(List<ProductReview> prdReview)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    foreach (var item in prdReview)
                    {
                        var result = context.ProductReviews.Where(m => m.Id == item.Id).FirstOrDefault();
                        if (result != null)
                        {
                            result.IsApproved = false;
                            context.SaveChanges();
                        }
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
             
