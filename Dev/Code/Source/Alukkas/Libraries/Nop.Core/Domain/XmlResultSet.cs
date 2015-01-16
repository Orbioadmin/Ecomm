using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain
{
    /// <summary>
    /// a class to hold xml result
    /// </summary>
    public class XmlResultSet
    {
        /// <summary>
        /// Property to hold xml result, column in sp should be same name as of this property
        /// </summary>
        public string XmlResult { get; set; }
    }
}
