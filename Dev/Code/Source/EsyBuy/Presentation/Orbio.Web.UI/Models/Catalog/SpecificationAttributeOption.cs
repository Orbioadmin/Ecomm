
namespace Orbio.Web.UI.Models.Catalog
{
    public class SpecificationAttributeOption
    {
        /// <summary>
        /// gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// gets or sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets element name if needed to render in hidden element for example
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// gets or sets if this option is selected
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// url to post for this filter
        /// </summary>
        public string FilterUrl { get; set; }
    }
}