using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orbio.Core.Domain.Catalog;

namespace Orbio.Web.UI.Models.Catalog
{
    public class SpecificationAttribute
    {
        public SpecificationAttribute()
        {
            this.SpecificationAttributeOptions = new List<SpecificationAttributeOption>();
        }
        //public SpecificationAttribute(SpecificationFilterModel specFilter):this()
        //{
        //    this.Id = specFilter.SpecificationAttributeId;
        //    this.Name = specFilter.SpecificationAttributeName;

        //}

        /// <summary>
        /// gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets or sets specification type , eg:- specification , price etc
        /// </summary>
        public string Type { get; set; }

        public string SelectedAttributeOptions { get; set; }
        /// <summary>
        /// gets or sets the attribute options
        /// </summary>
        public List<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; }

        /// <summary>
        /// Gets or sets minimum product price
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Gets or sets maximum product price
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Gets or sets sub title
        /// </summary>
        public string SubTitle { get; set; }
    }
}