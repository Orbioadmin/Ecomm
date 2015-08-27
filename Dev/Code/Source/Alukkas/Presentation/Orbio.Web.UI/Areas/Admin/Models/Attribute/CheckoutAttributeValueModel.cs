using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Attribute
{
    public class CheckoutAttributeValueModel
    {
        public CheckoutAttributeValueModel()
        {

        }

        public CheckoutAttributeValueModel(CheckoutAttributeValue val)
        {
            Id = val.Id;
            Name=val.Name;
            ColorSquares = val.ColorSquaresRgb;
            PriceAdjustment = val.PriceAdjustment;
            WeightAdjustment = val.WeightAdjustment;
            IsPreSelected = val.IsPreSelected;
            DisplayOrder = val.DisplayOrder;
            CheckoutAttributeId = val.CheckoutAttributeId;
        }

        public int Id { get; set; }

        public int CheckoutAttributeId { get; set; }

        public string Name { get; set; }

        public string ColorSquares { get; set; }

        public decimal PriceAdjustment { get; set; }

        public decimal WeightAdjustment { get; set; }

        public bool IsPreSelected { get; set; }

        public int DisplayOrder { get; set; }
    }
}