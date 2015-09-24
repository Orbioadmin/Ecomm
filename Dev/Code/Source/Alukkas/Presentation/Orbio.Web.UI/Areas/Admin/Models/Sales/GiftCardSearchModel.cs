using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Sales
{
    public class GiftCardSearchModel
    {
        public GiftCardSearchModel()
        {
            Activated = new List<SelectListItem>();
        }

        public int IsActive { get; set; }

        public string GiftCardCode { get; set; }

        public IList<SelectListItem> Activated { get; set; }
    }
}