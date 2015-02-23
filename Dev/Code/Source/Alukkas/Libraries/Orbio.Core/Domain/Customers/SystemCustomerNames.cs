using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Customers
{
    public static partial class SystemCustomerNames
    {
        public static string SearchEngine { get { return "SearchEngine"; } }
        public static string BackgroundTask { get { return "BackgroundTask"; } }
        public static string Administrators { get { return "Administrators"; } }

        public static string ForumModerators { get { return "ForumModerators"; } }

        public static string Registered { get { return "Registered"; } }

        public static string Guests { get { return "Guests"; } }

        public static string Vendors { get { return "Vendors"; } }
    }
}
