using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Orders
{
    public partial class OrderAverageReport
    {
        public string monthName { get; set; }

        public int orderPendingCount { get; set; }

        public int orderProcessingCount { get; set; }

        public int orderCompleteCount { get; set; }

        public int orderCancelledCount { get; set; }
    }
}
