using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Orbio.Core.Domain.Catalog
{
    [DataContract]
    public class CategoryProduct
    {
        [DataMember]
        public int CategoryId { get; set; }
        /// <summary>
        /// gets or sets name 
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        

        /// <summary>
        /// gets or sets Metakeywords
        /// </summary>
        [DataMember]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// gets or sets meta description
        /// </summary>
        [DataMember]
        public string MetaDescription { get; set; }

        /// <summary>
        /// gets or sets metatitle
        /// </summary>
        [DataMember]
        public string MetaTitle { get; set; }

        /// <summary>
        /// gets or sets se name
        /// </summary>
        [DataMember]
        public string SeName { get; set; }

        /// <summary>
        /// Gets or sets products
        /// </summary>
        [DataMember]
        public List<Product> Products { get; set; }

        /// <summary>
        /// gets or sets categories for breadcrumbs
        /// </summary>
        [DataMember]
        public List<Category> BreadCrumbs { get; set; }

        /// <summary>
        /// Gets or sets a value of used category template viewpath
        /// </summary>
        [DataMember]
        public string TemplateViewPath { get; set; }

        /// <summary>
        /// gets or sets the pagesize
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// gets or sets the Total Product Count includes all pages
        /// </summary>
        [DataMember]
        public int TotalProductCount { get; set; }
    }
}
