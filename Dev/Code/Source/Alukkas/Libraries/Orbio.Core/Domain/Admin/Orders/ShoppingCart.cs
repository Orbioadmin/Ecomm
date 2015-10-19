using Orbio.Core.Domain.Admin.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Admin.Orders
{
    public class ShoppingCart
    {
        public List<AdminCustomer> Customers { get; set; }
    }
}
