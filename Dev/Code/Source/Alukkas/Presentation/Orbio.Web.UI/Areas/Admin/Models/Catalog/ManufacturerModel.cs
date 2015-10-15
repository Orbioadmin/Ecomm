using Orbio.Core.Domain.Admin.Catalog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Catalog
{
    public class ManufacturerModel
    {
        public ManufacturerModel()
        {
            ManufacturerTemplates = new List<TemplateModel>();
        }

        public ManufacturerModel(Manufacturers result)
        {
            if (result.Manufacturer != null)
            {
                Id = result.Manufacturer.Id;
                Name = result.Manufacturer.Name;
                Description = result.Manufacturer.Description;
                Published = result.Manufacturer.Published;
                DisplayOrder = result.Manufacturer.DisplayOrder;
                Picture = result.Manufacturer.Picture;
                MetaKeyWords = result.Manufacturer.MetaKeyWords;
                MetaDescription = result.Manufacturer.MetaDescription;
                MetaTitle = result.Manufacturer.MetaTitle;
                SubjectToACL = result.Manufacturer.SubjectToACL;
                ManufacturerTemplate = result.Manufacturer.ManufacturerTemplate;
                SeName = result.Manufacturer.SeName;


                ManufacturerTemplates = (from c in result.Manufacturer.ManufacturerTemplates
                                         select new TemplateModel
                                         {
                                             Id = c.Id,
                                             Name = c.Name,
                                         }).ToList();
            }
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public int ManufacturerTemplate { get; set; }

        public string MetaKeyWords { get; set; }

        public string MetaDescription { get; set; }

        public string MetaTitle { get; set; }

        public string Picture { get; set; }

        public string SeName { get; set; }

        public bool SubjectToACL { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public List<string> SelectedProducts { get; set; }

        public List<TemplateModel> ManufacturerTemplates { get; set; }
    }
}