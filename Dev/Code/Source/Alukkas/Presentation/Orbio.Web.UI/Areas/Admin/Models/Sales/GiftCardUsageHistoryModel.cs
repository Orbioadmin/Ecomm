using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orbio.Web.UI.Areas.Admin.Models.Sales
{
    public class GiftCardUsageHistoryModel
    {
        public int Id { get; set; }

        public decimal UsedAmount { get; set; }

        public int OrderId { get; set; }

        public DateTime UsedOn { get; set; }
    }
}