using Orbio.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Catalog
{
    public class ReviewModel
    {
        public ReviewModel()
        {
          
        }

        //public ReviewModel(ProductReview productReview):this()
        //{
        //    this.ReviewText = productReview.ReviewText;
        //    this.ReviewTitle = productReview.ReviewText;

        //}

        public string SeName { get; set; }

        [Required(ErrorMessage="Review title required")]
        [DataType(DataType.Text)]
        [Display(Name = "Review Title")]
        public string ReviewTitle { get; set; }

        [Required(ErrorMessage="Review text required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Review Text")]
        public string ReviewText { get; set; }

        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Customer Name Required")]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }

    }
}