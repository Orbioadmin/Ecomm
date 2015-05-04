using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Domain.Catalog.Abstract;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class ProductVariantAttributeValue : IPriceComponent
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the product variant attribute mapping identifier
        /// </summary>
         [DataMember]
        public int ProductVariantAttributeId { get; set; }

        /// <summary>
        /// Gets or sets the attribute value type identifier
        /// </summary>
         [DataMember]
        public int AttributeValueTypeId { get; set; }

        /// <summary>
        /// Gets or sets the associated product identifier (used only with AttributeValueType.AssociatedToProduct)
        /// </summary>
         [DataMember]
        public int AssociatedProductId { get; set; }

        /// <summary>
        /// Gets or sets the product variant attribute name
        /// </summary>
         [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the color RGB value (used with "Color squares" attribute type)
        /// </summary>
         [DataMember]
        public string ColorSquaresRgb { get; set; }

        /// <summary>
        /// Gets or sets the price adjustment (used only with AttributeValueType.Simple)
        /// </summary>
         [DataMember]
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the weight adjustment (used only with AttributeValueType.Simple)
        /// </summary>
         [DataMember]
        public decimal WeightAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the attibute value cost (used only with AttributeValueType.Simple)
        /// </summary>
         [DataMember]
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the quantity of associated product (used only with AttributeValueType.AssociatedToProduct)
        /// </summary>
         [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is pre-selected
        /// </summary>
         [DataMember]
        public bool IsPreSelected { get; set; }

         /// <summary>
         /// Gets or sets the order in which value is displayed
         /// </summary>
         [DataMember]
         public int DisplayOrder { get; set; }

        ///// <summary>
        ///// Gets or sets the display order
        ///// </summary>
        // [DataMember]
        //public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the picture (url) associated with this value
        /// </summary>
         [DataMember]
         public string PictureUrl { get; set; }

         /// <summary>
         /// Gets or sets ProductPriceDetail
         /// </summary>
         [DataMember]
         public ProductPriceDetail ProductPriceDetail { get; set; }

         /// <summary>
         /// Gets or sets the attribute value type
         /// </summary>
         public AttributeValueType AttributeValueType
         {
             get
             {
                 return (AttributeValueType)this.AttributeValueTypeId;
             }
             set
             {
                 this.AttributeValueTypeId = (int)value;
             }
         }

         public decimal Price
         {
             get
             {
                 return this.PriceAdjustment;
             }
             set
             {
                 this.PriceAdjustment = value;
             }
         }

         public decimal GoldWeight
         {
             get
             {
                 return this.WeightAdjustment;
             }
             set
             {
                 this.WeightAdjustment = value;
             }
         }

         public int ProductUnit
         {
             get;
             set;
         }

         public int PriceUnit
         {
             get;
             set;
         }

         public decimal MarketUnitPrice
         {
             get;
             set;
         }
    }
}
