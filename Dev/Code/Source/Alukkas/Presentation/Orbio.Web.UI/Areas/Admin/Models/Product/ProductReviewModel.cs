using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Product
{
    public class ProductReviewModel
    {
        public ProductReviewModel()
        {

        }

        public ProductReviewModel(ProductReview result)
        {
            Id = result.Id;
            ProductId = result.ProductId;
            ProductName = result.Product.Name;
            CustomerId = result.CustomerId;
            Email = result.Customer.Email;
            Title = result.Title;
            ReviewText = result.ReviewText;
            Rating = result.Rating;
            IsApproved = result.IsApproved;
            CreatedOn = result.CreatedOnUtc;
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int CustomerId { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}