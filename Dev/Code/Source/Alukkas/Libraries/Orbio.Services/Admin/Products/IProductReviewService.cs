using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Products
{
    public interface IProductReviewService
    {
        List<ProductReview> GetAllProductReviews(DateTime? startDate,DateTime? endDate,string message);

        ProductReview EditProductReview(int id);

        int UpdateProductReview(int id, string title, string reviewText, bool isApproved);

        int DeleteProductReview(int id);

        int Approve(List<ProductReview> result);

        int Disapprove(List<ProductReview> result);
    }
}
