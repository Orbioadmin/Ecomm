using Orbio.Core.Domain.Orders;
using Orbio.Web.UI.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Models.Orders
{
    public class ShoppingCartItemModel : ProductDetailModel
    {
        public ShoppingCartItemModel()
        {
            this.items = new List<ShoppingCartItemModel>();
        }
        public ShoppingCartItemModel(ShoppingCartItem productDetail)
            : base(productDetail)
        {
            this.items = new List<ShoppingCartItemModel>();
            this.Itemcount = productDetail.Itemcount;
            //this.Id = productDetail.Id;
            //this.Name = productDetail.Name;
            //this.SeName = productDetail.SeName;
            //this.ImageRelativeUrl = productDetail.ImageRelativeUrl;
            //this.CurrencyCode = productDetail.CurrencyCode;
            //this.ProductPrice.Price = productDetail.Price.ToString("0.00");
            this.Totalprice = productDetail.Totalprice.ToString("#,##0.00");
            this.CartId = productDetail.CartId;
            //if (productDetail.ProductPictures != null && productDetail.ProductPictures.Count > 0)
            //{
            //    var baseUrl = ConfigurationManager.AppSettings["ImageServerBaseUrl"];
            //    this.ProductPictures = (from pp in productDetail.ProductPictures
            //                            select new PictureModel
            //                            {
            //                                ImageUrl = pp.RelativeUrl != null ? baseUrl + GetThumbImageFileName(pp.RelativeUrl) : string.Empty,
            //                                FullSizeImageUrl = pp.RelativeUrl != null ? baseUrl + pp.RelativeUrl : string.Empty,
            //                                AlternateText = productDetail.Name,
            //                                Title = productDetail.ShortDescription
            //                            }).ToList();

            //    this.DefaultPicture = this.ProductPictures.First();
            //}
            //else
            //{
            //    //TODO: set default picture
            //    if (this.ProductPictures == null || this.ProductPictures.Count == 0)
            //    {
            //        this.ProductPictures = new List<PictureModel>();
            //        this.DefaultPicture = new PictureModel();
            //    }
            //}

            //this.AllowedQuantities = new List<int>();
            //if (!String.IsNullOrWhiteSpace(productDetail.AllowedQuantities))
            //{
            //    productDetail.AllowedQuantities
            //       .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //       .ToList()
            //       .ForEach(qtyStr =>
            //       {
            //           int qty = 0;
            //           if (int.TryParse(qtyStr.Trim(), out qty))
            //           {
            //               this.AllowedQuantities.Add(qty);
            //           }
            //       });
            //}

            //this.OrderMaximumQuantity = productDetail.OrderMaximumQuantity;

            //this.OrderMinimumQuantity = productDetail.OrderMinimumQuantity;

            this.SelectedQuantity = productDetail.Quantity.ToString();

            this.IsRemove = false;
        }
        public int Itemcount { get; set; }
        public string Totalprice { get; set; }

        public int CartId { get; set; }

        public bool IsRemove { get; set; }

        public List<ShoppingCartItemModel> items { get; set; }

        //private static string GetThumbImageFileName(string imageUrl)
        //{
        //    var fileName = imageUrl;
        //    if (imageUrl.IndexOf('?') > 0)
        //    {
        //        fileName = imageUrl.Substring(0, imageUrl.IndexOf('?'));
        //    }

        //    fileName = fileName.Substring(fileName.LastIndexOf('/') + 1, fileName.Length - fileName.LastIndexOf('/') - 1);
        //    fileName = Path.GetFileNameWithoutExtension(fileName);

        //    return fileName.Length>0? imageUrl.Replace(fileName, fileName + "_tb"):fileName;
        //}
    }
}